using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public delegate void OnItemSelected();

public partial class ItemList : VBoxContainer {
	public event OnItemSelected ItemSelectedEvent;
	
	[Export] public PackedScene EntryScene;
	
	public List<ItemListEntry> Entries = new List<ItemListEntry>();
	public int SelectedEntry = -1;
	
	public virtual ItemListEntry AddEntry(bool select = true) {
		Node entryNode = EntryScene.Instantiate();
        
		if (entryNode is ItemListEntry entry) {
			AddChild(entry);
			Entries.Add(entry);

			entry.ItemList = this;
			entry.Id = Entries.Count - 1;

			if (select && SelectedEntry < 0) {
				entry.Select();
			}
			
			return entry;
		}
		else {
			GD.PrintErr("ItemList entry node did not derive from ItemListEntry.");
            
			entryNode.QueueFree();
			return null;
		}
	}

	public ItemListEntry GetSelectedEntry() {
		return SelectedEntry >= 0 ? Entries[SelectedEntry] : null;
	}

	public void SetSelectedEntry(int selectedEntry) {
		SelectedEntry = selectedEntry;
		ItemSelectedEvent.Invoke();
	}
}
