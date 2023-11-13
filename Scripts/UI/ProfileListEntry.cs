using System.Collections.Generic;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : SelectableItemListEntry {
    public new string Name = "";
    public List<int> Mods = new List<int>();
    
    [Export] private Label _nameText;

    public string GetName() {
        return Name;
    }

    public void SetName(string name) {
        Name = name;

        if (!string.IsNullOrEmpty(Name)) {
            _nameText.Text = Name;
        }
        else {
            _nameText.Text = "Name...";
        }
    }
}