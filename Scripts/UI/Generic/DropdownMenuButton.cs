using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class DropdownMenuButton : Button {
	[Export] private DropdownMenu _dropdown;

	public void OnPressed() {
		_dropdown.OnChildItemSelected(Name);
	}
}
