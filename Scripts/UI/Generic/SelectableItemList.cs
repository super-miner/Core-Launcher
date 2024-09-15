using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public delegate void OnItemSelected();

public partial class SelectableItemList : ItemList {
    public event OnItemSelected ItemSelectedEvent;
    
    public int SelectedEntry = -1;
    
    public new SelectableItemListEntry AddEntry(string section, bool select = true, bool init = true) {
        ItemListEntry entry = base.AddEntry(section, select, false);

        if (entry is SelectableItemListEntry selectableEntry) {
            if (select) {
                SetSelectedEntry(selectableEntry);
            }

            selectableEntry.ItemList = this;

            if (init) {
                selectableEntry.Init();
            }
            
            return selectableEntry;
        }
        else {
            GD.PrintErr("SelectableItemList entry node did not derive from SelectableItemListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }

    public override void RemoveEntry(ItemListEntry entry) {
        base.RemoveEntry(entry);

        if (SelectedEntry >= Entries.Count) {
            SetSelectedEntry(Entries.Count - 1);
        }
        else {
            SetSelectedEntry(SelectedEntry);
        }
    }
    
    public SelectableItemListEntry GetSelectedEntry() {
        if (SelectedEntry >= 0) {
            ItemListEntry entry = SelectedEntry < Entries.Count ? Entries[SelectedEntry] : null;

            if (entry is SelectableItemListEntry selectableEntry) {
                return selectableEntry;
            }
        }
        return null;
    }
    
    public void SetSelectedEntry(SelectableItemListEntry selectedEntry) {
        SetSelectedEntry(Entries.FindIndex(entry => entry == selectedEntry));
    }

    public void SetSelectedEntry(int selectedEntry) {
        GetSelectedEntry()?.Deselect();
		
        SelectedEntry = SelectedEntry >= Entries.Count ? Entries.Count - 1 : selectedEntry;
		
        GetSelectedEntry()?.Select();
		
        ItemSelectedEvent?.Invoke();
    }
}