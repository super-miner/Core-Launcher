using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : ItemListEntry {
    public new string Name = "";
    
    [Export] private Label _nameText = null;

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
}