using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuchByte.HomeAssistantPlugin.GUI;

public partial class CallServiceConfigurator : ActionConfigControl
{
    private JObject _services;
    private readonly PluginAction _macroDeckAction;

    private event EventHandler OnServicesLoaded;
    private event EventHandler OnEntitiesLoaded;

    public CallServiceConfigurator(PluginAction macroDeckAction)
    {
        _macroDeckAction = macroDeckAction;
        InitializeComponent();

        if (!string.IsNullOrWhiteSpace(_macroDeckAction.Configuration))
        {
            OnServicesLoaded += ServicesLoaded;
            if (JObject.Parse(_macroDeckAction.Configuration)["entityId"] != null) {
                OnEntitiesLoaded += EntitiesLoaded;
            }
        }

    }

    public override bool OnActionSave()
    {
        if (string.IsNullOrWhiteSpace(servicesBox.Text))
        {
            return false;
        }
        var configuration = new JObject
        {
            ["service"] = servicesBox.Text,
            ["service"] = servicesBox.Text,
            ["entityId"] = entityBox.Text
        };
        _macroDeckAction.ConfigurationSummary = servicesBox.Text + (!string.IsNullOrWhiteSpace(entityBox.Text) ? " -> " + entityBox.Text : "");
        _macroDeckAction.Configuration = configuration.ToString();
        return true;
    }

    private void ServicesLoaded(object sender, EventArgs e)
    {
        OnServicesLoaded -= ServicesLoaded;
        if (_macroDeckAction.Configuration.Length > 0)
        {
            var currentConfiguration = JObject.Parse(_macroDeckAction.Configuration);
            Invoke(new Action(() =>  servicesBox.Text = currentConfiguration["service"]?.ToString() ?? "-"));
        }
    }

    private void EntitiesLoaded(object sender, EventArgs e)
    {
        OnEntitiesLoaded -= EntitiesLoaded;
        if (_macroDeckAction.Configuration.Length > 0)
        {
            var currentConfiguration = JObject.Parse(_macroDeckAction.Configuration);
            Invoke(new Action(() => entityBox.Text = currentConfiguration["entityId"]?.ToString() ?? "-"));
        }
    }

    private void CallServiceConfigurator_Load(object sender, EventArgs e)
    {
        if (Main.HomeAssistant == null || !Main.HomeAssistant.IsLoggedIn)
        {
            Invoke(() =>
            {
                using (var msgBox = new MacroDeck.GUI.CustomControls.MessageBox())
                {
                    msgBox.ShowDialog("Not ready", "Home Assistant plugin is not configured. Please go to the package manager and configure the Home Assistant plugin.", MessageBoxButtons.OK);
                }
                servicesBox.Enabled = false;
            });
        }
        else
        {
            LoadServices();
        }
    }

    private void LoadServices()
    {
        servicesBox.Items.Clear();
        Task.Run(async () =>
        {
            var taskResult = await Main.HomeAssistant.GetServices();
            if (taskResult["result"] is not JObject result)
            {
                return;
            }
            
            _services = result;
            foreach (var domain in result.Children())
            {
                var domainName = ((JProperty)domain).Name;

                foreach (var service in result[domainName]?.Children() ?? JEnumerable<JToken>.Empty)
                {
                    var serviceName = ((JProperty)service).Name;
                    Invoke(() =>
                    {
                        servicesBox.Items.Add(domainName + "." + serviceName);
                    });
                }

                foreach (var service in result["homeassistant"]?.Children() ?? JEnumerable<JToken>.Empty)
                {
                    var serviceName = ((JProperty)service).Name;
                    Invoke(() =>
                    {
                        servicesBox.Items.Add(domainName + "." + serviceName);
                    });
                }

            }

            OnServicesLoaded?.Invoke(null, EventArgs.Empty);
        });
    }

    private void LoadEntities(string domain = "")
    {
        entityBox.Items.Clear();
        Task.Run(async () =>
        {
            List<string> entities;
            if (domain != "")
            {
                entities = (await Main.HomeAssistant.GetEntityIds()).FindAll(entity =>
                    entity.Split(".")[0].Equals(domain) || entity.Split(".")[0].Equals("group"));
            } else
            {
                entities = await Main.HomeAssistant.GetEntityIds();
            }
            foreach (var entityId in entities)
            {
                Invoke(() =>
                {
                    entityBox.Items.Add(entityId);
                });
            }

            OnEntitiesLoaded?.Invoke(null, EventArgs.Empty);
        });
    }

    private void ServicesBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        var domainName = servicesBox.Text.Split(".")[0];
        var serviceName = servicesBox.Text.Split(".")[1];
        lblDescription.Text = _services[domainName]?[serviceName]?["description"]?.ToString() ?? string.Empty;
        lblName.Text = _services[domainName]?[serviceName]?["name"]?.ToString() ?? string.Empty;
        var selectEntity = _services[domainName]?[serviceName]?["target"] != null;
        targetSelector.Visible = selectEntity;
        if (selectEntity)
        {
            LoadEntities(domainName);
        }
    }

    private void EntityBox_SelectedIndexChanged(object sender, EventArgs e)
    {
            
    }
}