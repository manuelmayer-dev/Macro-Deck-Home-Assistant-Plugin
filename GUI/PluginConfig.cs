using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI.CustomControls;
using SuchByte.MacroDeck.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuchByte.HomeAssistantPlugin.GUI
{
    public partial class PluginConfig : DialogForm
    {

        MacroDeckPlugin _main;

        public PluginConfig(MacroDeckPlugin main)
        {
            this._main = main;
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (Main.HomeAssistant.IsConnected && Main.HomeAssistant.IsLoggedIn)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            if (this.inputUrl.Text.Length == 0)
            {
                using (var messageBox = new SuchByte.MacroDeck.GUI.CustomControls.MessageBox())
                {
                    messageBox.ShowDialog("Unable to connect to Home Assistant", "The host cannot be empty", MessageBoxButtons.OK);
                }
                return;
            }
            if (this.token.Text.Length == 0)
            {
                using (var messageBox = new SuchByte.MacroDeck.GUI.CustomControls.MessageBox())
                {
                    messageBox.ShowDialog("Unable to connect to Home Assistant", "The token cannot be empty", MessageBoxButtons.OK);
                }
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            this.btnOk.Enabled = false;
            Task.Run(() =>
            {
                Main.HomeAssistant.Connect(this.inputUrl.Text, this.token.Text, this.checkSSL.Checked);
            });
        }

        private void AuthFailed(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                this.btnOk.Enabled = true;
                using (var messageBox = new MacroDeck.GUI.CustomControls.MessageBox())
                {
                    messageBox.ShowDialog("Unable to connect to Home Assistant", "Perhaps the host is wrong or the token invalid?", MessageBoxButtons.OK);
                }
            }));
        }

        private void AuthSuccess(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                this.btnOk.Enabled = true;

                Dictionary<string, string> credentials = new Dictionary<string, string>();
                credentials["host"] = inputUrl.Text;
                credentials["token"] = token.Text;
                credentials["ssl"] = checkSSL.Checked.ToString();
                PluginCredentials.SetCredentials(this._main, credentials);
                this.Invoke(new Action(() =>
                {
                    Cursor.Current = Cursors.Default;
                    using (var messageBox = new MacroDeck.GUI.CustomControls.MessageBox())
                    {
                        messageBox.ShowDialog("Success", "Successfully connected to Home Assistant.", MessageBoxButtons.OK);
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }));
            }));
        }

        private void PluginConfig_Load(object sender, EventArgs e)
        {
            Main.HomeAssistant.OnAuthFailed += AuthFailed;
            Main.HomeAssistant.OnAuthSuccess += AuthSuccess;
            List<Dictionary<string, string>> credentialsList = PluginCredentials.GetPluginCredentials(this._main);
            Dictionary<string, string> credentials = null;
            if (credentialsList.Count > 0)
            {
                credentials = credentialsList[0];
            }
            if (credentials != null)
            {
                this.inputUrl.Text = credentials["host"];
                this.token.Text = credentials["token"];
                this.checkSSL.Checked = bool.Parse(credentials["ssl"]);
            }
        }

        private void BtnVariables_Click(object sender, EventArgs e)
        {
            using (var entitySelector = new EntitySelector(PluginConfiguration.GetValue(_main, "variablesEntityIds")))
            {
                if (entitySelector.ShowDialog() == DialogResult.OK)
                {
                    PluginConfiguration.SetValue(_main, "variablesEntityIds", JArray.FromObject(entitySelector.SelectedEntities).ToString());
                    ((Main)this._main).UpdateAllVariables();
                }
            }
        }
    }
}
