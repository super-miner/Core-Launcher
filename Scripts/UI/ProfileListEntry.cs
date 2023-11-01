using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : ItemListEntry {
    [Export] private Label _nameText = null;
    
    public new string Name = "";

    public void FromProfile(StoredProfile profile) {
        SetName(profile.Name);
    }

    public void Serialize() {
        GD.Print($"Serialized profile {Name}.");
        StoredDataManager.Data.Profiles.Add(new StoredProfile(Name));
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