using Godot;
using System.Collections.Generic;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene entryScene;
	
	public List<ItemListEntry> entries = new List<ItemListEntry>();
	
	public virtual void AddEntry() {
		ItemListEntry entry = new ItemListEntry(entryScene, this);
			
		entries.Add(entry);
	}
}
