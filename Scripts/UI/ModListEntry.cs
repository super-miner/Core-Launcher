using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ModListEntry : ItemListEntry {
	[Export] private Label _nameLabel;

	public override void Init() {
		_nameLabel.Text = ModManager.ModsList.Mods[Id].Name;
	}
}
