using Godot;
using System;

[Tool]
public class SystemScript : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int editedField = -1;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary statsData = jsonDictionary["stats"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weaponsData = jsonDictionary["weapons"] as Godot.Collections.Dictionary;

        for (int i = 0; i < statsData.Count; i++)
        {
            ItemList item = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList");
            item.AddItem((statsData[i.ToString()]).ToString());
        }

        for (int i = 0; i < weaponsData.Count; i++)
        {
            ItemList item = GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList");
            item.AddItem((weaponsData[i.ToString()]).ToString());
        }
        database_editor.Close();
    }

    private void _save_Data()
    {
        _save_Stats();
        _save_Weapons();
    }
    private void _save_Stats()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary stats_data = new Godot.Collections.Dictionary();

        int statSize = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetItemCount();
        for (int i = 0; i < statSize; i++)
        {
            String text = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetItemText(i);
            stats_data.Add(i.ToString(), text);
        }
        jsonDictionary["stats"] = stats_data;
        database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _save_Weapons()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weapons_data = new Godot.Collections.Dictionary();

        int weaponSize = GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList").GetItemCount();
        for (int i = 0; i < weaponSize; i++)
        {
            String text = GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList").GetItemText(i);
            weapons_data.Add(i.ToString(), text);
        }
        jsonDictionary["weapons"] = weapons_data;
        database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_AddStat_pressed()
    {
        editedField = 0;
        GetNode<WindowDialog>("EditField").PopupCentered();
    }

    private void _on_RemoveStat_pressed()
    {
        int index = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetSelectedItems()[0];
        if (index > -1)
        {
            GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").RemoveItem(index);
            _save_Data();
        }
    }
    private void _on_OKButton_pressed()
    {
        string name = GetNode<LineEdit>("EditField/FieldName").Text;
        if (name != "") {
            if (editedField == 0) {
                GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").AddItem(name);
            }else if (editedField == 1) {
                GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList").AddItem(name);
            }
            _save_Data();
        }
        GetNode<WindowDialog>("EditField").Hide();
        editedField = -1;
    }

    private void _on_CancelButton_pressed()
    {
        editedField = -1;
    }
    private void _on_EditField_popup_hide()
    {
        editedField = -1;
    }

    private void _on_AddWeapon_pressed()
    {
        editedField = 1;
        GetNode<WindowDialog>("EditField").PopupCentered();
    }

    private void _on_RemoveWeapon_pressed()
    {
        int index = GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList").GetSelectedItems()[0];
        if (index > -1)
        {
            GetNode<ItemList>("WeaponTypesLabel/WeaponTypesContainer/WpBoxContainer/WeaponList").RemoveItem(index);
            _save_Data();
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
