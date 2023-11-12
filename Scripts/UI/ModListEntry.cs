using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ModListEntry : ItemListEntry {
	[Export] private StateButton _addButton;
	[Export] private Label _nameLabel;
	[Export] private Label _authorLabel;
	[Export] private TextureRect _logoTexture;

	public override void Init() {
		ModInfo modInfo = GetModInfo();
		
		_nameLabel.Text = modInfo.Name;
		_authorLabel.Text = $"By: {modInfo.Author.Username}";
		_logoTexture.Texture = ImageTexture.CreateFromImage(modInfo.Logo.LogoImage);
		
		SelectableItemListEntry selectableEntry = MainMenuManager.Instance.ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			if (profileEntry.Mods.Contains(modInfo.Id)) {
				_addButton.SetState("AddedButton");
			}
		}
	}

	public ModInfo GetModInfo() {
		return ModManager.ModsList.Mods[Id];
	}
}
