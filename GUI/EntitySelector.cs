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

        private List<string> _entities = new List<string>();

        public EntitySelector(string selectedEntitiesJson)
        {
            InitializeComponent();
            this.searchBox.TextChanged += SearchBox_TextChanged;
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
                    this._entities.Add(result["entity_id"].ToString());
                }
                this.Invoke(new Action(() => UpdateEntityList()));
            });
        }

        private void UpdateEntityList()
        {
            this.entityList.Items.Clear();
            foreach (string entity in this._entities.FindAll(e => this.searchBox.Text.Length < 2 || MacroDeck.Utils.StringSearch.StringContains(e, this.searchBox.Text)))
            {
                this.entityList.Items.Add(entity, SelectedEntities.Contains(entity));
            }
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

        private void SearchBox_TextChanged(object sender, System.EventArgs e)
        {
            if (this.searchBox.Text.Length == 0 || this.searchBox.Text.Length > 1)
            {
                UpdateEntityList();
            }
        }

    }
}
