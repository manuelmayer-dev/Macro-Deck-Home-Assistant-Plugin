using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WatsonWebsocket;

namespace SuchByte.HomeAssistantPlugin
{
    public class HomeAssistant
    {
        private readonly string _apiString = "/api/websocket";
        private string _protocol => (_ssl ? "wss://" : "ws://");

        private string _homeAssistantHost = "";

        private string _homeAssistantToken = "";

        private bool _ssl = false;

        private bool _loggedIn = false;
        public bool IsConnected => (_webSocketClient != null && _webSocketClient.Connected);
        public bool IsLoggedIn => _loggedIn;

        private WatsonWsClient _webSocketClient;

        private int _nextId = 0;

        private ConcurrentDictionary<int, TaskCompletionSource<JObject>> _responseHandlers;


        /// Events
        public delegate void StateChangedEventArgs(object sender, JObject data);
        public event StateChangedEventArgs OnStateChanged;
        public event EventHandler OnAuthSuccess;
        public event EventHandler OnAuthFailed;

        public HomeAssistant()
        {

        }

        public void Connect(string host, string token, bool ssl)
        {
            if (this._webSocketClient != null)
            {
                if (this._webSocketClient.Connected)
                {
                    this._webSocketClient.Stop();
                }
                this._webSocketClient.ServerConnected -= ServerConnected;
                this._webSocketClient.ServerDisconnected -= ServerDisconnected;
                this._webSocketClient.MessageReceived -= MessageReceived;
                this._webSocketClient.Dispose();
                this._loggedIn = false;
            }
            this._homeAssistantHost = host;
            this._homeAssistantToken = token;
            this._ssl = ssl;
            this._responseHandlers = new ConcurrentDictionary<int, TaskCompletionSource<JObject>>();
            
            try
            {
                this._webSocketClient = new WatsonWsClient(new Uri(this._protocol + this._homeAssistantHost + this._apiString));
                this._webSocketClient.ServerConnected += ServerConnected;
                this._webSocketClient.ServerDisconnected += ServerDisconnected;
                this._webSocketClient.MessageReceived += MessageReceived;
                this._webSocketClient.Start();
            } catch
            {
                if (OnAuthFailed != null)
                {
                    OnAuthFailed(null, EventArgs.Empty);
                }
            }
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            JObject body = JObject.Parse(Encoding.UTF8.GetString(e.Data));
            if (body["type"] != null)
            {
                switch (body["type"].ToString())
                {
                    case "result":
                        int messageId = Int32.Parse(body["id"].ToString());

                        if (_responseHandlers.TryRemove(messageId, out TaskCompletionSource<JObject> handler))
                        {
                            handler.SetResult(body);
                        }
                        break;
                    case "event":
                        JObject eventBody = body["event"] as JObject;
                        if (eventBody["event_type"].ToString() == "state_changed")
                        {
                            string entityId = eventBody["data"]["entity_id"].ToString();
                            JObject newState = eventBody["data"]["new_state"] as JObject;
                            if (OnStateChanged != null)
                            {
                                OnStateChanged(entityId, newState);
                            }
                        }
                        break;
                    case "auth_ok":
                        this._loggedIn = true;
                        if (OnAuthSuccess != null)
                        {
                            OnAuthSuccess(null, EventArgs.Empty);
                        }
                        break;
                    case "auth_invalid":
                        this._loggedIn = false;
                        if (OnAuthFailed != null)
                        {
                            OnAuthFailed(null, EventArgs.Empty);
                        }
                        break;
                }

            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void ServerConnected(object sender, EventArgs args)
        {
            Authenticate();
            SubscribeEvents();
        }

        private void ServerDisconnected(object sender, EventArgs args)
        {
            this._loggedIn = false;
            var unusedHandlers = this._responseHandlers.ToArray();
            _responseHandlers.Clear();
            foreach (var cb in unusedHandlers)
            {
                var tcs = cb.Value;
                tcs.TrySetCanceled();
            }
        }

        private JObject SendRequest(string type, JObject additionalFields = null)
        {
            int messageId;
            JObject result = null;

            try
            {
                JObject body = new JObject();
                body["type"] = type;

                if (additionalFields != null)
                {
                    body.Merge(additionalFields);
                }

                var taskCompletionSource = new TaskCompletionSource<JObject>();
                do
                {
                    if (this._responseHandlers == null)
                    {
                        break;
                    }
                    messageId = GetNewMessageId();
                    if (this._responseHandlers.TryAdd(messageId, taskCompletionSource))
                    {
                        body.Add("id", messageId);
                        break;
                    }

                } while (true);


                this._webSocketClient.SendAsync(body.ToString());

                taskCompletionSource.Task.Wait();
                if (!taskCompletionSource.Task.IsCanceled)
                {
                    result = taskCompletionSource.Task.Result;
                }
            } catch { }
           

            

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return result;
        }

        public void Authenticate()
        {
            JObject authObject = new JObject();
            authObject["type"] = "auth";
            authObject["access_token"] = this._homeAssistantToken;
            this._webSocketClient.SendAsync(authObject.ToString());
        }

        public void SubscribeEvents()
        {
            JObject subscribeObject = new JObject();
            subscribeObject["event_type"] = "state_changed";
            SendRequest("subscribe_events", subscribeObject);
        }
        public JObject GetStates()
        {
            return SendRequest("get_states");
        }

        public JObject GetServices()
        {
            return SendRequest("get_services");
        }

        public List<string> GetEntityIds()
        {
            List<string> entityIds = new List<string>();
            JArray results = GetStates()["result"] as JArray;
            foreach (JObject jObject in results)
            {
                entityIds.Add(jObject["entity_id"].ToString());
            }

            return entityIds;
        }

        public void CallServiceAsync(string service, string entityId = "", JObject serviceData = null)
        {
            Task.Run(() =>
            {
                JObject body = new JObject
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

                SendRequest("call_service", body);
            });

        }


        protected int GetNewMessageId()
        {
            this._nextId++;
            return this._nextId;
        }

    }
}
