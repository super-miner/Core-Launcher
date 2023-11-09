using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : ItemListEntry {
    [Export] private Label _nameText = null;
    
    public new string Name = "";

    private StoredProfile _profile;

    public void FromProfile(StoredProfile profile) {
        _profile = profile;
        
        SetName(_profile.Name, false);
    }

    public void Serialize() {
        GD.Print($"Serializing profile {Name}.");
        
        _profile = new StoredProfile(Name);
        StoredDataManager.Data.Profiles.Add(_profile);
        
        GD.Print($"Serialized profile {Name}.");
    }

    public string GetName() {
        return Name;
    }

    public void SetName(string name, bool updateProfile = true) {
        Name = name;

        if (updateProfile) {
            _profile.Name = Name;
        }

        if (Name != "") {
            _nameText.Text = Name;
        }
        else {
            _nameText.Text = "Name...";
        }
    }
}