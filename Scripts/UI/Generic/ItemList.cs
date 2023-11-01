using Godot;
using System.Collections.Generic;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene entryScene;
	
	public List<ItemListEntry> entries = new List<ItemListEntry>();
	
	public void AddEntry() {
		ItemListEntry entry = new ItemListEntry(entryScene, this);
			
		entries.Add(entry);
	}
}
