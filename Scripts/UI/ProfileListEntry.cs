using System.Collections.Generic;
using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileListEntry : SelectableItemListEntry {
    public Profile Profile;
    
    [Export] private Label _nameText;

    public override void Init() {
        SetName(GetName());
    }

    public string GetName() {
        return Profile.Name;
    }

    public void SetName(string name) {
        Profile.Name = name;

        if (!string.IsNullOrEmpty(Profile.Name)) {
            _nameText.Text = Profile.Name;
        }
        else {
            _nameText.Text = "Name...";
        }
    }
}