using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot;
using ItemList = CoreLauncher.Scripts.UI.Generic.ItemList;

namespace CoreLauncher.Scripts.UI; 

public partial class ProfileList : ItemList {
    public override void _Ready() {
        StoredDataManager.AddDeserializedCallback((data) => {
            foreach (StoredProfile profile in data.Profiles) {
                AddEntryFromProfile(profile);
            }
        });
    }

    public ProfileListEntry AddEntryFromProfile(StoredProfile profile) {
        ProfileListEntry entry = AddEntry(false);
        
        entry.FromProfile(profile);

        return entry;
    }
    
    public new ProfileListEntry AddEntry(bool serialize = true) {
        ItemListEntry entry = base.AddEntry();

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