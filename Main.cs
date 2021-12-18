using Newtonsoft.Json.Linq;
using SuchByte.HomeAssistantPlugin.Actions;
using SuchByte.HomeAssistantPlugin.GUI;
using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SuchByte.HomeAssistantPlugin
{
    public class Main : MacroDeckPlugin
    {
        public static HomeAssistant HomeAssistant;
        public override string Description => "(Beta) This plugin can control your Home Assistant smart home";
        public override Image Icon => Properties.Resources.Home_Assistant_Plugin;
        public override bool CanConfigure => true;
        public override void Enable()
        {
            HomeAssistant = new HomeAssistant();
            HomeAssistant.OnStateChanged += StateChanged;
            HomeAssistant.OnAuthSuccess += OnAuthSuccess;
            List<Dictionary<string, string>> credentialsList = PluginCredentials.GetPluginCredentials(this);
            if (credentialsList != null)
            {
                Dictionary<string, string> credentials = null;
                if (credentialsList.Count > 0)
                {
                    credentials = credentialsList[0];
                }
                if (credentials != null)
                {
                    Task.Run(() =>
                    {
                        HomeAssistant.Connect(credentials["host"], credentials["token"], bool.Parse(credentials["ssl"]));
                    });
                }
            }
            
            this.Actions = new List<PluginAction>
            {
                new CallServiceAction(),
            };
        }

        private void OnAuthSuccess(object sender, EventArgs e)
        {
            this.UpdateAllVariables();
        }

        public void UpdateAllVariables()
        {
            try
            {
                Task.Run(() =>
                {
                    // Wait 5000ms to wait until everything is started. Not pretty but works.
                    Thread.Sleep(5000);
                    JArray results = HomeAssistant.GetStates()["result"] as JArray;
                    foreach (JObject result in results)
                    {
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
                    if (this == null) return;
                    string variablesEntityIds = PluginConfiguration.GetValue(this, "variablesEntityIds");
                    if (variablesEntityIds == null || variablesEntityIds.Length == 0) return;
                    JArray variablesArray = JArray.Parse(variablesEntityIds);
                    if (variablesArray == null) return;
                    string[] suggestions = new string[0];
                    if (variablesArray.ToObject<List<string>>().Contains(newState["entity_id"].ToString()))
                    {
                        string value = newState["state"].ToString().Replace("On", "True", StringComparison.OrdinalIgnoreCase).Replace("Off", "False", StringComparison.OrdinalIgnoreCase);
                        VariableType type = VariableType.String;
                        if (Boolean.TryParse(value, out bool b))
                        {
                            type = VariableType.Bool;
                        }
                        else if (int.TryParse(value, out int i))
                        {
                            type = VariableType.Integer;
                        }
                        else if (float.TryParse(value, out float f))
                        {
                            type = VariableType.Float;
                        }
                        VariableManager.SetValue(newState["entity_id"].ToString(), value, type, this, suggestions, false);
                    }
                    
                }
                catch { }
            });
        }

        public override void OpenConfigurator()
        {
            using (var pluginConfig = new PluginConfig(this))
            {
                pluginConfig.ShowDialog();
            }
        }
    }

}
