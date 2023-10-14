using Newtonsoft.Json.Linq;
using SuchByte.HomeAssistantPlugin.Actions;
using SuchByte.HomeAssistantPlugin.GUI;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;

namespace SuchByte.HomeAssistantPlugin;

public class Main : MacroDeckPlugin
{
    public static Main Instance { get; private set; }
    
    public static HomeAssistant HomeAssistant;
    public override bool CanConfigure => true;
    
    private ContentSelectorButton _statusButton = new();

    private readonly ToolTip _statusToolTip = new();

    private MainWindow _mainWindow;

    public Main()
    {
        Instance = this;
    }
    
    public override void Enable()
    {
        HomeAssistant = new HomeAssistant();
        HomeAssistant.OnStateChanged += StateChanged;
        HomeAssistant.OnAuthSuccess += OnAuthSuccess;
        HomeAssistant.ConnectionStateChanged += HomeAssistantOnConnectionStateChanged;
        VariableManager.SetValue("homeassistant_connected", false, VariableType.Bool, this, null);
        MacroDeck.MacroDeck.OnMainWindowLoad += MacroDeckOnOnMainWindowLoad;
        if (MacroDeck.MacroDeck.MainWindow != null && !MacroDeck.MacroDeck.MainWindow.IsDisposed)
        {
            MacroDeckOnOnMainWindowLoad(MacroDeck.MacroDeck.MainWindow, EventArgs.Empty);
        }
        var credentialsList = PluginCredentials.GetPluginCredentials(this);
        if (credentialsList != null)
        {
            Dictionary<string, string> credentials = null;
            if (credentialsList.Count > 0)
            {
                credentials = credentialsList[0];
            }
            if (credentials != null)
            {
                Task.Run(async () =>
                {
                    await HomeAssistant.Connect(credentials["host"], credentials["token"], bool.Parse(credentials["ssl"]));
                });
            }
        }
            
        Actions = new List<PluginAction>
        {
            new CallServiceAction(),
        };
    }

    private void HomeAssistantOnConnectionStateChanged(object sender, bool e)
    {
        UpdateStatusIcon();
    }

    private void MacroDeckOnOnMainWindowLoad(object sender, EventArgs e)
    {
        _mainWindow = sender as MainWindow;

        _statusButton = new ContentSelectorButton
        {
            BackgroundImageLayout = ImageLayout.Stretch,

        };
        _mainWindow?.contentButtonPanel.Controls.Add(_statusButton);
        UpdateStatusIcon();
    }

    private void UpdateStatusIcon()
    {
        if (_mainWindow is null || _mainWindow.IsDisposed || _statusButton is null || _statusButton.IsDisposed)
        {
            return;
        }
        
        _mainWindow.Invoke(() =>
        {
            _statusButton.BackgroundImage = HomeAssistant.IsConnected
                ? Properties.Resources.home_assistant_connected
                : Properties.Resources.home_assistant_disconnected;
            _statusToolTip.SetToolTip(_statusButton,
                "Home Assistant " + (HomeAssistant.IsConnected ? " Connected" : "Disconnected"));
        });
    }

    private void OnAuthSuccess(object sender, EventArgs e)
    {
        UpdateAllVariables();
    }

    public void UpdateAllVariables()
    {
        try
        {
            Task.Run(async () =>
            {
                // Wait 5000ms to wait until everything is started. Not pretty but works.
                Thread.Sleep(5000);
                var results = await HomeAssistant.GetStates();
                if (results["result"] is not JArray resultsArray)
                {
                    return;
                }
                foreach (var jToken in resultsArray)
                {
                    var result = (JObject)jToken;
                    UpdateVariable(result);
                }
            });
        }
        catch { }
    }

    private void StateChanged(object entityId, JObject newState)
    {
        UpdateVariable(newState);
    }

    private void UpdateVariable(JObject newState)
    {
        Task.Run(() =>
        {
            try
            {
                var variablesEntityIds = PluginConfiguration.GetValue(this, "variablesEntityIds");
                if (variablesEntityIds == null || variablesEntityIds.Length == 0) return;
                var variablesArray = JArray.Parse(variablesEntityIds);
                if (variablesArray == null) return;
                var entityId = newState["entity_id"]?.ToString();
                if (string.IsNullOrWhiteSpace(entityId) || !variablesArray.ToObject<List<string>>().Contains(entityId))
                {
                    return;
                }

                var value = newState["state"]?.ToString().Replace("On", "True", StringComparison.OrdinalIgnoreCase)
                    .Replace("Off", "False", StringComparison.OrdinalIgnoreCase);
                var type = VariableType.String;
                if (bool.TryParse(value, out _))
                {
                    type = VariableType.Bool;
                }
                else if (int.TryParse(value, out _))
                {
                    type = VariableType.Integer;
                }
                else if (float.TryParse(value, out _))
                {
                    type = VariableType.Float;
                }

                if (value is not null && !string.IsNullOrWhiteSpace(entityId))
                {
                    VariableManager.SetValue(entityId, value, type, this, null);
                }
            }
            catch
            {
                // ignore
            }
        });
    }

    public override void OpenConfigurator()
    {
        using var pluginConfig = new PluginConfig(this);
        pluginConfig.ShowDialog();
    }
}