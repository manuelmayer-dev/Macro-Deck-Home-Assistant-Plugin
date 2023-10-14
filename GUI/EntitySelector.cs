using Newtonsoft.Json.Linq;
using SuchByte.MacroDeck.GUI.CustomControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SuchByte.HomeAssistantPlugin.GUI;

public partial class EntitySelector : DialogForm
{
    public readonly List<string> SelectedEntities = new();

    private readonly List<string> _entities = new();

    public EntitySelector(string selectedEntitiesJson)
    {
        InitializeComponent();
        searchBox.TextChanged += SearchBox_TextChanged;
        entityList.ItemCheck += EntityList_ItemCheck;
        if (selectedEntitiesJson.Length == 0)
        {
            return;
        }
        
        try
        {
            SelectedEntities = JArray.Parse(selectedEntitiesJson).ToObject<List<string>>();
        }
        catch { }
    }

    private void EntityList_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        if (e.NewValue == CheckState.Checked)
        {
            var entity = entityList.GetItemText(entityList.Items[e.Index]);
            if (!SelectedEntities.Contains(entity))
            {
                SelectedEntities.Add(entity);
            }
                
        }
        else
        {
            var entity = entityList.GetItemText(entityList.Items[e.Index]);
            if (SelectedEntities.Contains(entity))
            {
                SelectedEntities.Remove(entity);
            }
        }
    }

    private async void EntitySelector_Shown(object sender, EventArgs e)
    {
        if (Main.HomeAssistant.IsConnected == false || Main.HomeAssistant.IsLoggedIn == false) return;

        var results = await Main.HomeAssistant.GetStates();
        if (results["result"] is not JArray resultsArray)
        {
            return;
        }
        foreach (var jToken in resultsArray)
        {
            var result = (JObject)jToken;
            _entities.Add(result["entity_id"]?.ToString());
        }
        Invoke(UpdateEntityList);
    }

    private void UpdateEntityList()
    {
        entityList.Items.Clear();
        foreach (var entity in _entities.FindAll(e => searchBox.Text.Length < 2 || MacroDeck.Utils.StringSearch.StringContains(e, searchBox.Text)))
        {
            entityList.Items.Add(entity, SelectedEntities.Contains(entity));
        }
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void SearchBox_TextChanged(object sender, EventArgs e)
    {
        if (searchBox.Text.Length == 0 || searchBox.Text.Length > 1)
        {
            UpdateEntityList();
        }
    }

}