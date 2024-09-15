using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class AddModButton : StateButton {
	[Export] private ModListEntry _modEntry;
	
	public void OnAddPressed() {
		SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			profileEntry.Profile.QueueAddMod(_modEntry.GetModInfo().Id);
		}
		
		SetState("AddedButton");

		CoreLauncher.Scripts.UI.Generic.ItemList itemList = _modEntry.ItemList;
		if (itemList is ModList modList) {
			modList.UpdateButtonStates();
		}
	}

	public void OnAddedPressed() {
		SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			profileEntry.Profile.QueueRemoveMod(_modEntry.GetModInfo().Id);
		}
		
		SetState("NotAddedButton");
		
		CoreLauncher.Scripts.UI.Generic.ItemList itemList = _modEntry.ItemList;
		if (itemList is ModList modList) {
			modList.UpdateButtonStates();
		}
	}
}
