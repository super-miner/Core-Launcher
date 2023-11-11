using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public delegate void OnItemSelected();

public partial class SelectableItemList : ItemList {
    public event OnItemSelected ItemSelectedEvent;
    
    public int SelectedEntry = -1;
    
    public new SelectableItemListEntry AddEntry(bool select = true) {
        ItemListEntry entry = base.AddEntry(select);

        if (entry is SelectableItemListEntry selectableEntry) {
            if (select && SelectedEntry < 0) {
                SetSelectedEntry(entry.Id);
            }

            selectableEntry.ItemList = this;
            
            return selectableEntry;
        }
        else {
            GD.PrintErr("SelectableItemList entry node did not derive from SelectableItemListEntry.");
            
            entry.QueueFree();
            return null;
        }
    }
    
    public SelectableItemListEntry GetSelectedEntry() {
        if (SelectedEntry >= 0) {
            ItemListEntry entry = Entries[SelectedEntry];

            if (entry is SelectableItemListEntry selectableEntry) {
                return selectableEntry;
            }
        }
        return null;
    }

    public void SetSelectedEntry(int selectedEntry) {
        GetSelectedEntry()?.Deselect();
		
        SelectedEntry = selectedEntry;
		
        GetSelectedEntry()?.Select();
		
        ItemSelectedEvent?.Invoke();
    }
}