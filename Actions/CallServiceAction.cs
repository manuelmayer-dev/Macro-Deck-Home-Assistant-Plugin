using Newtonsoft.Json.Linq;
using SuchByte.HomeAssistantPlugin.GUI;
using SuchByte.MacroDeck.ActionButton;
using SuchByte.MacroDeck.GUI;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Threading.Tasks;

namespace SuchByte.HomeAssistantPlugin.Actions;

public class CallServiceAction : PluginAction
{
    public override string Name => "Call service";
    public override string Description => "Calls a Home Assistant service";
    public override bool CanConfigure => true;
    public override ActionConfigControl GetActionConfigControl(ActionConfigurator actionConfigurator)
    {
        return new CallServiceConfigurator(this);
    }
    public override void Trigger(string clientId, ActionButton actionButton)
    {
        if (!Main.HomeAssistant.IsLoggedIn || string.IsNullOrWhiteSpace(Configuration))
        {
            return;
        }

        try
        {
            var configurationObject = JObject.Parse(Configuration);
            Task.Run(async () =>
            {
                await Main.HomeAssistant.CallServiceAsync(configurationObject["service"]?.ToString(),
                    configurationObject["entityId"]?.ToString());
            });
        }
        catch { }
    }

}