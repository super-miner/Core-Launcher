using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene entryScene;
	
	public List<ItemListEntry> entries = new List<ItemListEntry>();
	
	public virtual ItemListEntry AddEntry() {
		Node entryNode = entryScene.Instantiate();
        
		if (entryNode is ItemListEntry entry) {
			AddChild(entry);
			entries.Add(entry);
			return entry;
		}
		else {
			GD.PrintErr("ItemList entry node did not derive from ItemListEntry.");
            
			entryNode.QueueFree();
			return null;
		}
	}
}
