using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ModListEntry : ItemListEntry {
	[Export] private Label _nameLabel;
	[Export] private Label _authorLabel;
	[Export] private TextureRect _logoTexture;

	public override void Init() {
		ModInfo modInfo = GetModInfo();
		
		_nameLabel.Text = modInfo.Name;
		_authorLabel.Text = $"By: {modInfo.Author.Username}";
		_logoTexture.Texture = ImageTexture.CreateFromImage(modInfo.Logo.LogoImage);
	}

	public ModInfo GetModInfo() {
		return ModManager.ModsList.Mods[Id];
	}
}
