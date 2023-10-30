using Godot;
using System.Collections.Generic;
using Godot.Collections;

public partial class ItemList : VBoxContainer {
	[Export] public PackedScene entryScene;
	[Export] public Array<Control> entries = new Array<Control>();

	public override void _Ready() {
		
	}

	public void AddExistingEntries() {
		foreach (Control entry in entries) {
			Node entryNode = entryScene.Instantiate();
			
			entry.Reparent(this);
		}
	}
	
	public void AddItem() {
		Node entryNode = entryScene.Instantiate();

		if (entryNode is Control entry) {
			entry.Reparent(this);
			
			entries.Add(entry);
		}
		else {
			entryNode.QueueFree();
		}
	}
}
