using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuchByte.MacroDeck.Logging;
using SuchByte.MacroDeck.Variables;
using WatsonWebsocket;

namespace SuchByte.HomeAssistantPlugin;

public class HomeAssistant
{
    public bool IsConnected => _webSocketClient != null && _webSocketClient.Connected;
    
    public bool IsLoggedIn { get; private set; }
    
    private const string ApiString = "/api/websocket";
    private string Protocol => _ssl ? "wss://" : "ws://";

    private string _homeAssistantHost = "";

    private string _homeAssistantToken = "";

    private bool _ssl; 

    private WatsonWsClient _webSocketClient;

    private int _nextId = 0;

    private ConcurrentDictionary<int, TaskCompletionSource<JObject>> _responseHandlers;

    public delegate void StateChangedEventArgs(object sender, JObject data);
    public event StateChangedEventArgs OnStateChanged;
    public event EventHandler OnAuthSuccess;
    public event EventHandler OnAuthFailed;
    public event EventHandler<bool> ConnectionStateChanged;

    private CancellationTokenSource _autoReconnectCancellationTokenSource;

    public async Task Connect(string host, string token, bool ssl)
    {
        if (_webSocketClient != null)
        {
            if (_webSocketClient.Connected)
            {
                _autoReconnectCancellationTokenSource?.Cancel();
                _autoReconnectCancellationTokenSource = null;
                await _webSocketClient.StopAsync();
            }
            _webSocketClient.ServerConnected -= ServerConnected;
            _webSocketClient.ServerDisconnected -= ServerDisconnected;
            _webSocketClient.MessageReceived -= MessageReceived;
            _webSocketClient.Dispose();
            IsLoggedIn = false;
        }
        _homeAssistantHost = host;
        _homeAssistantToken = token;
        _ssl = ssl;
        _responseHandlers = new ConcurrentDictionary<int, TaskCompletionSource<JObject>>();
            
        try
        {
            _webSocketClient = new WatsonWsClient(new Uri(Protocol + _homeAssistantHost + ApiString));
            _webSocketClient.AcceptInvalidCertificates = true;
            _webSocketClient.KeepAliveInterval = 10;
            _webSocketClient.ServerConnected += ServerConnected;
            _webSocketClient.ServerDisconnected += ServerDisconnected;
            _webSocketClient.MessageReceived += MessageReceived;
            await _webSocketClient.StartAsync();
        } catch
        {
            OnAuthFailed?.Invoke(null, EventArgs.Empty);
            ConnectionStateChanged?.Invoke(this, false);
        }
    }

    private void StartAutoReconnectHandler(CancellationToken cancellationToken)
    {
        try
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (IsConnected)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                        continue;
                    }

                    await Connect(_homeAssistantHost, _homeAssistantToken, _ssl);
                }
            }, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            // ignore
        }
    }

    public async Task<JObject> GetStates()
    {
        return await SendRequest("get_states");
    }

    public async Task<JObject> GetServices()
    {
        return await SendRequest("get_services");
    }

    public async Task<List<string>> GetEntityIds()
    {
        var entityIds = new List<string>();
        var results = await GetStates();
        if (results["result"] is not JArray result)
        {
            return entityIds;
        }
        foreach (var jObject in result)
        {
            if (jObject["entity_id"] is JObject entityId)
            {
                entityIds.Add(entityId.ToString());
            }
        }

        return entityIds;
    }

    public async Task CallServiceAsync(string service, string entityId = "", JObject serviceData = null)
    {
        var body = new JObject
        {
            ["domain"] = service.Split(".")[0],
            ["service"] = service.Split(".")[1],
            ["target"] = new JObject
            {
                ["entity_id"] = entityId,
            },
        };

        if (serviceData != null)
        {
            body["service_data"] = serviceData;
        }

        await SendRequest("call_service", body);
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        var body = JObject.Parse(Encoding.UTF8.GetString(e.Data));
        if (body["type"] == null)
        {
            return;
        }
        
        switch (body["type"].ToString())
        {
            case "result":
                HandleResultReceived(body);
                break;
            case "event":
                HandleEventReceived(body);
                break;
            case "auth_ok":
                IsLoggedIn = true;
                OnAuthSuccess?.Invoke(null, EventArgs.Empty);
                ConnectionStateChanged?.Invoke(this, true);
                VariableManager.SetValue("homeassistant_connected", true, VariableType.Bool, Main.Instance, null);
                break;
            case "auth_invalid":
                IsLoggedIn = false;
                OnAuthFailed?.Invoke(null, EventArgs.Empty);
                ConnectionStateChanged?.Invoke(this, false);
                VariableManager.SetValue("homeassistant_connected", false, VariableType.Bool, Main.Instance, null);
                break;
        }
    }

    private void HandleResultReceived(JObject body)
    {
        var messageId = int.Parse(body["id"]?.ToString() ?? "-1");

        if (_responseHandlers.TryRemove(messageId, out var handler))
        {
            handler.SetResult(body);
        }
    }

    private void HandleEventReceived(JObject body)
    {
        if (body["event"] is not { } eventBody)
        {
            return;
        }

        if (eventBody["event_type"] is not { } eventType)
        {
            return;
        }

        if (!eventType.ToString().Equals("state_changed", StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        if (eventBody["data"] is not { } eventData)
        {
            return;
        }

        var entityId = eventData["entity_id"]?.ToString();
        if (eventData["new_state"] is JObject newState && entityId is not null)
        {
            OnStateChanged?.Invoke(entityId, newState);
        }
    }

    private async void ServerConnected(object sender, EventArgs args)
    {
        if (_autoReconnectCancellationTokenSource == null)
        {
            _autoReconnectCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _autoReconnectCancellationTokenSource.Token;
            StartAutoReconnectHandler(cancellationToken);
        }
        await Authenticate();
        await SubscribeEvents();
    }

    private void ServerDisconnected(object sender, EventArgs args)
    {
        VariableManager.SetValue("homeassistant_connected", false, VariableType.Bool, Main.Instance, null);
        ConnectionStateChanged?.Invoke(this, false);
        IsLoggedIn = false;
        var unusedHandlers = _responseHandlers.ToArray();
        _responseHandlers.Clear();
        foreach (var cb in unusedHandlers)
        {
            var tcs = cb.Value;
            tcs.TrySetCanceled();
        }
    }

    private async Task<JObject> SendRequest(string type, JObject additionalFields = null)
    {
        JObject result = null;

        try
        {
            var body = new JObject
            {
                ["type"] = type
            };

            if (additionalFields != null)
            {
                body.Merge(additionalFields);
            }

            var taskCompletionSource = new TaskCompletionSource<JObject>();
            do
            {
                if (_responseHandlers == null)
                {
                    break;
                }

                var messageId = GetNewMessageId();
                if (_responseHandlers.TryAdd(messageId, taskCompletionSource))
                {
                    body.Add("id", messageId);
                    break;
                }
            } while (true);

            await _webSocketClient.SendAsync(body.ToString());

            await taskCompletionSource.Task;
            if (!taskCompletionSource.Task.IsCanceled)
            {
                result = taskCompletionSource.Task.Result;
            }
        }
        catch (Exception ex)
        {
            MacroDeckLogger.Error(Main.Instance, $"Error while sending message to Home Assistant\n{ex}");
        }

        return result;
    }

    private async Task Authenticate()
    {
        var authObject = new JObject
        {
            ["type"] = "auth",
            ["access_token"] = _homeAssistantToken
        };
        await _webSocketClient.SendAsync(authObject.ToString());
    }

    private async Task SubscribeEvents()
    {
        var subscribeObject = new JObject
        {
            ["event_type"] = "state_changed"
        };
        await SendRequest("subscribe_events", subscribeObject);
    }
        
    private int GetNewMessageId()
    {
        _nextId++;
        return _nextId;
    }
}