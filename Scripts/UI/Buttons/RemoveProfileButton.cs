using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Buttons; 

public partial class RemoveProfileButton : Button {
    public void OnPressed() {
        if (InstanceManager.GetInstance<MainMenuManager>().ProfileList != null) {
            SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();

            if (selectableEntry is ProfileListEntry profileEntry) {
                ProfileManager.DeleteProfile(profileEntry.Profile);
            }
            
            InstanceManager.GetInstance<MainMenuManager>().ProfileList.RemoveEntry(InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry());
        }
        else {
            GD.PrintErr("Remove Profile Button: Could not remove profile because the item list was not found.");
        }
    }
}