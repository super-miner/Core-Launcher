using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene EntryScene;
	
	public List<ItemListEntry> Entries = new List<ItemListEntry>();

	public virtual ItemListEntry AddEntry(string section, bool select = true, bool init = true) {
		Node entryNode = EntryScene.Instantiate();
        
		if (entryNode is ItemListEntry entry) {
			AddChild(entry);

			int separatorIndex = GetSeparatorIndex(section);
			if (separatorIndex >= 0) {
				MoveChild(entry, separatorIndex + 1);
			}
			
			Entries.Add(entry);

			entry.ItemList = this;
			entry.Id = Entries.Count - 1;

			if (init) {
				entry.Init();
			}
			
			return entry;
		}
		else {
			GD.PrintErr("ItemList entry node did not derive from ItemListEntry.");
            
			entryNode.QueueFree();
			return null;
		}
	}

	public virtual void RemoveEntry(ItemListEntry entry) {
		entry.QueueFree();
		Entries.Remove(entry);
	}

	public virtual void ClearEntries() {
		foreach (ItemListEntry entry in Entries) {
			entry.QueueFree();
		}

		Entries = new List<ItemListEntry>();
	}

	public int GetSeparatorIndex(string separatorName) {
		for (int i = 0; i < GetChildCount(); i++) {
			Node child = GetChild(i);
			
			if (child is ItemListSeparator separator && separator.Label.Text == separatorName) {
				return i;
			}
		}

		return -1;
	}
}
