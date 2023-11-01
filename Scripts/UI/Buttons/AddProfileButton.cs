using Godot;
using CoreLauncher.Scripts.UI.Generic;

namespace CoreLauncher.Scripts.UI.Buttons;

public partial class AddProfileButton : Button {
	[Export] private ProfileList itemList = null;
	
	public void OnPressed() {
		if (itemList != null) {
			itemList.AddEntry();
		}
		else {
			GD.Print("Could not add profile because the itemList was not found.");
		}
	}
}
