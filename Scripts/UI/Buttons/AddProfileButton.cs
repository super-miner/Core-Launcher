using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Systems;
using Godot;
using CoreLauncher.Scripts.UI.Generic;

namespace CoreLauncher.Scripts.UI.Buttons;

public partial class AddProfileButton : Button {
	public void OnPressed() {
		if (InstanceManager.GetInstance<MainMenuManager>().ProfileList != null) {
			InstanceManager.GetInstance<MainMenuManager>().ProfileList.AddEntry();
		}
		else {
			GD.PrintErr("Add Profile Button: Could not add profile because the item list was not found.");
		}
	}
}
