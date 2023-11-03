using System;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot;
using ItemList = CoreLauncher.Scripts.UI.Generic.ItemList;

namespace CoreLauncher.Scripts.UI;

public partial class ProfileList : ItemList {
    public override void _Ready() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
    }

    public void OnStoredDataDeserialized() {
        foreach (StoredProfile profile in StoredDataManager.Data.Profiles) {
            AddEntryFromProfile(profile);
        }
    }

    public ProfileListEntry AddEntryFromProfile(StoredProfile profile) {
        ProfileListEntry entry = AddEntry(false, false);
        
        entry.FromProfile(profile);
        
        if (SelectedEntry < 0) {
            entry.Select();
        }

        return entry;
    }
    
    public new ProfileListEntry AddEntry(bool serialize = true, bool select = true) {
        ItemListEntry entry = base.AddEntry(select);

        if (entry is ProfileListEntry profileEntry) {
            if (serialize) {
                profileEntry.Serialize();
            }
            
            return profileEntry;
        }
        else {
            GD.PrintErr("ProfileList entry node did not derive from ProfileListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }
}