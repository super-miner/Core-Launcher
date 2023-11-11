using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene EntryScene;
	
	public List<ItemListEntry> Entries = new List<ItemListEntry>();
	
	public virtual ItemListEntry AddEntry(bool select = true) {
		Node entryNode = EntryScene.Instantiate();
        
		if (entryNode is ItemListEntry entry) {
			AddChild(entry);
			Entries.Add(entry);

			entry.ItemList = this;
			entry.Id = Entries.Count - 1;
			
			entry.Init();
			
			return entry;
		}
		else {
			GD.PrintErr("ItemList entry node did not derive from ItemListEntry.");
            
			entryNode.QueueFree();
			return null;
		}
	}
}
