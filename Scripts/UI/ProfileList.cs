using System.Collections.Generic;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.UI.Generic;
using Godot;
using ItemList = CoreLauncher.Scripts.UI.Generic.ItemList;

namespace CoreLauncher.Scripts.UI;

public partial class ProfileList : SelectableItemList {
    public override void _Ready() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
        
        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }

    public override void _ExitTree() {
        StoredDataManager.DeserializeStoredDataEvent -= OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent -= OnSerializeStoredData;
        
        OnSerializeStoredData();
    }
    
    public new ProfileListEntry AddEntry(bool select = true) {
        ItemListEntry entry = base.AddEntry(select);

        if (entry is ProfileListEntry profileEntry) {
            return profileEntry;
        }
        else {
            GD.PrintErr("ProfileList entry node did not derive from ProfileListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }
    
    private void OnDeserializeStoredData() {
        foreach (StoredProfileListEntry storedProfileEntry in StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().Profiles) {
            ProfileListEntry entry = AddEntry(false);

            entry.SetName(storedProfileEntry.Name);
            entry.Mods = storedProfileEntry.Mods ?? new List<int>();
        }
        
        SetSelectedEntry(StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().SelectedEntry);
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().Profiles.Clear();
        foreach (ItemListEntry entry in Entries) {
            if (entry is ProfileListEntry profileEntry) {
                StoredProfileListEntry storedProfileEntry = new StoredProfileListEntry {
                    Name = profileEntry.Name,
                    Mods = profileEntry.Mods
                };
                
                StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().Profiles.Add(storedProfileEntry);
            }
        }

        StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().SelectedEntry = SelectedEntry;
    }
}