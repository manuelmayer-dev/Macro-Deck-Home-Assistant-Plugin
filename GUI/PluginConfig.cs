using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuchByte.HomeAssistantPlugin.GUI;

public partial class PluginConfig : DialogForm
{

    MacroDeckPlugin _main;

    public PluginConfig(MacroDeckPlugin main)
    {
        _main = main;
        InitializeComponent();
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        if (Main.HomeAssistant.IsConnected && Main.HomeAssistant.IsLoggedIn)
        {
            DialogResult = DialogResult.OK;
            Close();
            return;
        }
        if (inputUrl.Text.Length == 0)
        {
            using (var messageBox = new SuchByte.MacroDeck.GUI.CustomControls.MessageBox())
            {
                messageBox.ShowDialog("Unable to connect to Home Assistant", "The host cannot be empty", MessageBoxButtons.OK);
            }
            return;
        }
        if (token.Text.Length == 0)
        {
            using (var messageBox = new SuchByte.MacroDeck.GUI.CustomControls.MessageBox())
            {
                messageBox.ShowDialog("Unable to connect to Home Assistant", "The token cannot be empty", MessageBoxButtons.OK);
            }
            return;
        }
        Cursor.Current = Cursors.WaitCursor;
        btnOk.Enabled = false;
        Task.Run(async () =>
        {
            await Main.HomeAssistant.Connect(inputUrl.Text, token.Text, checkSSL.Checked);
        });
    }

    private void AuthFailed(object sender, EventArgs e)
    {
        Invoke(() =>
        {
            btnOk.Enabled = true;
            using var messageBox = new MacroDeck.GUI.CustomControls.MessageBox();
            messageBox.ShowDialog("Unable to connect to Home Assistant", "Perhaps the host is wrong or the token invalid?", MessageBoxButtons.OK);
        });
    }

    private void AuthSuccess(object sender, EventArgs e)
    {
        Invoke(() =>
        {
            btnOk.Enabled = true;

            var credentials = new Dictionary<string, string>();
            credentials["host"] = inputUrl.Text;
            credentials["token"] = token.Text;
            credentials["ssl"] = checkSSL.Checked.ToString();
            PluginCredentials.SetCredentials(_main, credentials);
            Invoke(() =>
            {
                Cursor.Current = Cursors.Default;
                using (var messageBox = new MacroDeck.GUI.CustomControls.MessageBox())
                {
                    messageBox.ShowDialog("Success", "Successfully connected to Home Assistant.", MessageBoxButtons.OK);
                }
                DialogResult = DialogResult.OK;
                Close();
            });
        });
    }

    private void PluginConfig_Load(object sender, EventArgs e)
    {
        Main.HomeAssistant.OnAuthFailed += AuthFailed;
        Main.HomeAssistant.OnAuthSuccess += AuthSuccess;
        var credentialsList = PluginCredentials.GetPluginCredentials(_main);
        if (credentialsList != null && credentialsList.Count > 0)
        {
            Dictionary<string, string> credentials = null;
            if (credentialsList.Count > 0)
            {
                credentials = credentialsList[0];
            }
            if (credentials != null)
            {
                inputUrl.Text = credentials["host"];
                token.Text = credentials["token"];
                checkSSL.Checked = bool.Parse(credentials["ssl"]);
            }
        }
    }

    private void BtnVariables_Click(object sender, EventArgs e)
    {
        using (var entitySelector = new EntitySelector(PluginConfiguration.GetValue(_main, "variablesEntityIds")))
        {
            if (entitySelector.ShowDialog() == DialogResult.OK)
            {
                PluginConfiguration.SetValue(_main, "variablesEntityIds", JArray.FromObject(entitySelector.SelectedEntities).ToString());
                ((Main)_main).UpdateAllVariables();
            }
        }
    }
}