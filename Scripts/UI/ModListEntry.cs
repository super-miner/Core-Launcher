using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ModListEntry : ItemListEntry {
	[Export] private Label _nameLabel;
	[Export] private Label _authorLabel;
	[Export] private TextureRect _logoTexture;

	public override void Init() {
		_nameLabel.Text = ModManager.ModsList.Mods[Id].Name;
		_authorLabel.Text = $"By: {ModManager.ModsList.Mods[Id].Author.Username}";
		_logoTexture.Texture = ImageTexture.CreateFromImage(ModManager.ModsList.Mods[Id].Logo.LogoImage);
	}
}
