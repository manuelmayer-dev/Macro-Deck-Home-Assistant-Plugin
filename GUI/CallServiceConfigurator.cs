using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuchByte.HomeAssistantPlugin.GUI
{
    public partial class CallServiceConfigurator : ActionConfigControl
    {
        JObject _services;
        PluginAction _macroDeckAction;

        event EventHandler OnServicesLoaded;
        event EventHandler OnEntitiesLoaded;

        public CallServiceConfigurator(PluginAction macroDeckAction, ActionConfigurator actionConfigurator)
        {
            this._macroDeckAction = macroDeckAction;
            InitializeComponent();

            if (this._macroDeckAction.Configuration.Length > 0)
            {
                this.OnServicesLoaded += ServicesLoaded;
                if (JObject.Parse(this._macroDeckAction.Configuration)["entityId"] != null) {
                    this.OnEntitiesLoaded += EntitiesLoaded;
                }
            }

            actionConfigurator.ActionSave += OnActionSave;
        }

        private void OnActionSave(object sender, EventArgs e)
        {
            this.SaveConfig();
        }

        private void ServicesLoaded(object sender, EventArgs e)
        {
            this.OnServicesLoaded -= ServicesLoaded;
            if (this._macroDeckAction.Configuration.Length > 0)
            {
                JObject currentConfiguration = JObject.Parse(this._macroDeckAction.Configuration);
                this.Invoke(new Action(() =>  this.servicesBox.Text = currentConfiguration["service"].ToString()));
            }
        }

        private void EntitiesLoaded(object sender, EventArgs e)
        {
            this.OnEntitiesLoaded -= EntitiesLoaded;
            if (this._macroDeckAction.Configuration.Length > 0)
            {
                JObject currentConfiguration = JObject.Parse(this._macroDeckAction.Configuration);
                this.Invoke(new Action(() => this.entityBox.Text = currentConfiguration["entityId"].ToString()));
            }
        }

        private void CallServiceConfigurator_Load(object sender, EventArgs e)
        {
            if (Main.HomeAssistant == null || !Main.HomeAssistant.IsLoggedIn)
            {
                this.Invoke(new Action(() =>
                {
                    using (var msgBox = new MacroDeck.GUI.CustomControls.MessageBox())
                    {
                        msgBox.ShowDialog("Not ready", "Home Assistant plugin is not configured. Please go to the package manager and configure the Home Assistant plugin.", MessageBoxButtons.OK);
                    }
                    this.servicesBox.Enabled = false;
                }));

                return;
            }
            else
            {
                LoadServices();
            }
        }

        private void LoadServices()
        {
            this.servicesBox.Items.Clear();
            Task.Run(() =>
            {
                JObject result = Main.HomeAssistant.GetServices()["result"] as JObject;
                if (result == null) return;
                this._services = result;
                foreach (var domain in result.Children())
                {
                    var domainName = ((JProperty)domain).Name;

                    foreach (var service in result[domainName].Children())
                    {
                        var serviceName = ((JProperty)service).Name;
                        this.Invoke(new Action(() =>
                        {
                            servicesBox.Items.Add(domainName + "." + serviceName);
                        }));
                    }

                    foreach (var service in result["homeassistant"].Children())
                    {
                        var serviceName = ((JProperty)service).Name;
                        this.Invoke(new Action(() =>
                        {
                            servicesBox.Items.Add(domainName + "." + serviceName);
                        }));
                    }

                }
                if (OnServicesLoaded != null)
                {
                    OnServicesLoaded(null, EventArgs.Empty);
                }
            });
        }

        private void LoadEntities(string domain = "")
        {
            this.entityBox.Items.Clear();
            Task.Run(() =>
            {
                List<string> entities = null;
                if (domain != "")
                {
                    entities = Main.HomeAssistant.GetEntityIds().FindAll(entity => entity.Split(".")[0].Equals(domain) || entity.Split(".")[0].Equals("group"));
                } else
                {
                    entities = Main.HomeAssistant.GetEntityIds();
                }
                foreach (string entityId in entities)
                {
                    this.Invoke(new Action(() =>
                    {
                        entityBox.Items.Add(entityId);
                    }));
                }
                if (OnEntitiesLoaded != null)
                {
                    OnEntitiesLoaded(null, EventArgs.Empty);
                }
            });
        }

        private void SaveConfig()
        {
            try
            {
                JObject configuration = new JObject();
                configuration["service"] = this.servicesBox.Text;
                configuration["service"] = this.servicesBox.Text;
                configuration["entityId"] = this.entityBox.Text;
                this._macroDeckAction.DisplayName = this._macroDeckAction.Name + " -> " + this.servicesBox.Text;
                this._macroDeckAction.Configuration = configuration.ToString();
                Debug.WriteLine("HA action config saved");
            } catch { }
        }

        private void ServicesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var domainName = this.servicesBox.Text.Split(".")[0];
            var serviceName = this.servicesBox.Text.Split(".")[1];
            this.lblDescription.Text = this._services[domainName][serviceName]["description"].ToString();
            this.lblName.Text = this._services[domainName][serviceName]["name"].ToString();
            bool selectEntity = this._services[domainName][serviceName]["target"] != null;
            this.targetSelector.Visible = selectEntity;
            if (selectEntity)
            {
                LoadEntities(domainName);
            }
        }

        private void EntityBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
