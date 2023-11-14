using System.Collections.Generic;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.StoredData.StoredDataTypes;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

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

        if (Entries.Count == 1) {
            InstanceManager.GetInstance<MainMenuManager>().OptionsTabs.Visible = true;
        }

        if (entry is ProfileListEntry profileEntry) {
            return profileEntry;
        }
        else {
            GD.PrintErr("ProfileList entry node did not derive from ProfileListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }

    public override void RemoveEntry(ItemListEntry entry) {
        base.RemoveEntry(entry);
        
        if (Entries.Count == 0) {
            InstanceManager.GetInstance<MainMenuManager>().OptionsTabs.Visible = false;
        }
    }
    
    private void OnDeserializeStoredData() {
        List<StoredProfileListEntry> storedProfiles = StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().Profiles;

        if (storedProfiles.Count == 0) {
            storedProfiles.Add(new StoredProfileListEntry());
        }
        
        foreach (StoredProfileListEntry storedProfileEntry in storedProfiles) {
            ProfileListEntry entry = AddEntry(false);

            entry.SetName(storedProfileEntry.Name);
            entry.Mods = storedProfileEntry.Mods ?? new List<int>();
        }

        int selectedEntry = StoredDataManager.GetStoredDataGroup<ProfileDataGroup>().SelectedEntry;
        SetSelectedEntry(selectedEntry >= 0 ? selectedEntry : 0);
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