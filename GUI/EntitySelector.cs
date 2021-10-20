using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI.CustomControls;
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
    public partial class EntitySelector : DialogForm
    {
        public List<string> SelectedEntities = new List<string>();

        public EntitySelector(string selectedEntitiesJson)
        {
            InitializeComponent();
            if (selectedEntitiesJson.Length > 0)
            {
                try
                {
                    SelectedEntities = JArray.Parse(selectedEntitiesJson).ToObject<List<string>>();
                }
                catch { }
            }
        }


        private void EntitySelector_Load(object sender, EventArgs e)
        {
            if (Main.HomeAssistant.IsConnected == false || Main.HomeAssistant.IsLoggedIn == false) return;

            Task.Run(() =>
            {
                JArray results = Main.HomeAssistant.GetStates()["result"] as JArray;
                foreach (JObject result in results)
                {
                   this.Invoke(new Action(() => this.entityList.Items.Add(result["entity_id"].ToString(), SelectedEntities.Contains(result["entity_id"].ToString()))));
                }
            });
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedEntities.Clear();
            foreach (string entity in this.entityList.CheckedItems)
            {
                SelectedEntities.Add(entity);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
