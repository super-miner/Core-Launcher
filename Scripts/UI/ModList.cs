using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Menus.Main.Tabs;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;
using ItemList = CoreLauncher.Scripts.UI.Generic.ItemList;

namespace CoreLauncher.Scripts.UI;

public partial class ModList : ItemList {
    [Export] public bool ShowLibraryMods;
    public int ModsShowing = 0;
    
    public override void _EnterTree() {
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
        
        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }
    
    public override void _Ready() {
        InstanceManager.GetInstance<MainMenuManager>().ProfileList.ItemSelectedEvent += OnItemSelected;
        
        RefreshModsList();
    }

    public override void _ExitTree() {
        InstanceManager.GetInstance<MainMenuManager>().ProfileList.ItemSelectedEvent -= OnItemSelected;
        
        StoredDataManager.DeserializeStoredDataEvent -= OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent -= OnSerializeStoredData;
        
        OnSerializeStoredData();
    }

    public void CreateEntries() {
        GD.Print("Creating entries.");
        
        ModsShowing = 0;

        ItemListEntry entry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();

        if (entry == null) {
            return;
        }

        if (entry is ProfileListEntry profileEntry) {
            for (int i = 0; i < ModManager.ModsList.Mods.Count; i++) {
                ModInfo modInfo = ModManager.ModsList.Mods[i];
            
                if ((!ShowLibraryMods && modInfo.GetLibrary()) || !(modInfo.GetServerSide() == profileEntry.Server || modInfo.GetClientSide() == !profileEntry.Server)) {
                    continue;
                }
            
                ModListEntry modEntry = AddEntry("", true, false);
                modEntry.ModListId = i;
                modEntry.Init();

                ModsShowing++;
            }
        }
    }
    
    public void RefreshModsList() {
        ClearEntries();
        CreateEntries();
        
        InstanceManager.GetInstance<ModsTabManager>().UpdateModsShowingText();
    }

    public void SetShowLibraryMods(bool showLibraryMods) {
        ShowLibraryMods = showLibraryMods;
        
        RefreshModsList();
    }
    
    public new ModListEntry AddEntry(string section, bool select = true, bool init = true) {
        ItemListEntry entry = base.AddEntry(section, select, init);

        if (entry is ModListEntry modEntry) {
            return modEntry;
        }
        else {
            GD.PrintErr("ModList entry node did not derive from ModListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }

    public void UpdateButtonStates() {
        foreach (ItemListEntry entry in Entries) {
            if (entry is ModListEntry modEntry) {
                modEntry.UpdateButtonState();
            }
        }
    }

    private void OnItemSelected() {
        RefreshModsList();
    }
    
    private void OnDeserializeStoredData() {
        ShowLibraryMods = StoredDataManager.GetStoredDataGroup<ConfigDataGroup>().ShowLibraryMods;
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<ConfigDataGroup>().ShowLibraryMods = ShowLibraryMods;
    }
}