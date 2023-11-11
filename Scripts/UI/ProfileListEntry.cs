using System.Collections.Generic;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : SelectableItemListEntry {
    public new string Name = "";
    public List<int> Mods;
    
    [Export] private Label _nameText;

    public string GetName() {
        return Name;
    }

    public void SetName(string name) {
        Name = name;

        if (Name != "") {
            _nameText.Text = Name;
        }
        else {
            _nameText.Text = "Name...";
        }
    }

    public void OnPressed() {
        ItemList.SetSelectedEntry(Id);
    }
}