using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene EntryScene;
	
	public List<ItemListEntry> Entries = new List<ItemListEntry>();
	public int SelectedEntry = -1;
	
	public virtual ItemListEntry AddEntry() {
		Node entryNode = EntryScene.Instantiate();
        
		if (entryNode is ItemListEntry entry) {
			AddChild(entry);
			Entries.Add(entry);

			entry.ItemList = this;
			entry.Id = Entries.Count - 1;

			if (SelectedEntry < 0) {
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
}
