using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.Systems;
using Godot;
using CoreLauncher.Scripts.UI.Generic;

namespace CoreLauncher.Scripts.UI.Buttons;

[GlobalClass]
public partial class AddProfileButton : DropdownMenu {
	public void OnProfileTypeSelected(string name) {
		if (InstanceManager.GetInstance<MainMenuManager>().ProfileList != null) {
			Profile profile = ProfileManager.CreateAndAddProfile("", name == "ServerButton");
			InstanceManager.GetInstance<MainMenuManager>().ProfileList.AddEntry(profile);
		}
		else {
			GD.PrintErr("Add Profile Button: Could not add profile because the item list was not found.");
		}
	}
}
