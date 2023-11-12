using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class AddModButton : StateButton {
    [Export] private ModListEntry _modEntry;
    
    public void OnAddPressed() {
        SelectableItemListEntry selectableEntry = MainMenuManager.Instance.ProfileList.GetSelectedEntry();
        if (selectableEntry is ProfileListEntry profileEntry) {
			profileEntry.Mods.Add(_modEntry.GetModInfo().Id);
        }
        
        SetState("AddedButton");
    }

    public void OnAddedPressed() {
        SelectableItemListEntry selectableEntry = MainMenuManager.Instance.ProfileList.GetSelectedEntry();
        if (selectableEntry is ProfileListEntry profileEntry) {
            profileEntry.Mods.Remove(_modEntry.GetModInfo().Id);
        }
        
        SetState("NotAddedButton");
    }
}