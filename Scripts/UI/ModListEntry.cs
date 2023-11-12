using System.Collections.Generic;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class ModListEntry : ItemListEntry {
	public int ModListId;
	
	[Export] private StateButton _addButton;
	[Export] private Label _nameLabel;
	[Export] private Label _authorLabel;
	[Export] private TextureRect _logoTexture;

	public override void Init() {
		ModInfo modInfo = GetModInfo();
		
		_nameLabel.Text = modInfo.Name;
		_authorLabel.Text = $"By: {modInfo.Author.Username}";
		_logoTexture.Texture = ImageTexture.CreateFromImage(modInfo.Logo.LogoImage);
		
		UpdateButtonState();
	}

	public async void UpdateButtonState() {
		ModInfo modInfo = GetModInfo();
		
		SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			if (profileEntry.Mods.Contains(modInfo.Id)) {
				_addButton.SetState("AddedButton");
			}
			else {
				List<int> dependencies = await ModManager.GetDependencies(profileEntry.Mods);
				
				if (dependencies.Contains(modInfo.Id)) {
					_addButton.SetState("AddedAsDependencyButton");
				}
				else {
					_addButton.SetState("NotAddedButton");
				}
			}
		}
	}

	public ModInfo GetModInfo() {
		return ModManager.ModsList.Mods[ModListId];
	}
}
