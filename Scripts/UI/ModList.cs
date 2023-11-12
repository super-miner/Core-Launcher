using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.UI.Generic;
using Godot;
using ItemList = CoreLauncher.Scripts.UI.Generic.ItemList;

namespace CoreLauncher.Scripts.UI;

public partial class ModList : ItemList {
    public override void _Ready() {
        CreateEntries();
    }

    public void CreateEntries() {
        for (int i = 0; i < ModManager.ModsList.Mods.Count; i++) {
            AddEntry();
        }
    }
    
    public new ModListEntry AddEntry(bool select = true) {
        ItemListEntry entry = base.AddEntry(select);

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
}