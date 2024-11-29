using System;
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
	[Export] private Label _elevatedAccessLabel;
	[Export] private Label _authorLabel;
	[Export] private TextureRect _logoTexture;
	[Export] private CustomLinkButton _donationButton;

	public override void Init()
	{
		ModInfo modInfo = GetModInfo();
		try
		{
			_nameLabel.Text = modInfo.Name;
			_elevatedAccessLabel.Visible = modInfo.IsElevatedAccess();
			_authorLabel.Text = $"By: {modInfo.Author.Username}";
			_logoTexture.Texture = ImageTexture.CreateFromImage(modInfo.Logo.LogoImage);

			string donationLink = modInfo.GetDonationLink();
			if (!string.IsNullOrEmpty(donationLink))
			{
				_donationButton.Link = donationLink;
			}
			else
			{
				_donationButton.Visible = false;
			}

			UpdateButtonState();
		}
		catch (Exception e)
		{
			GD.PrintErr($"Mod: [{modInfo.Name}] is causing {e.Message}\r\n{modInfo.GetDonationLink()}\r\n {e.StackTrace}");
		}
	}

	public async void UpdateButtonState() {
		ModInfo modInfo = GetModInfo();
		
		SelectableItemListEntry selectableEntry = InstanceManager.GetInstance<MainMenuManager>().ProfileList.GetSelectedEntry();
		if (selectableEntry is ProfileListEntry profileEntry) {
			List<int> profileEntryMods = profileEntry.Profile.GetAddedMods();
			if (profileEntryMods.Contains(modInfo.Id)) {
				_addButton.SetState("AddedButton");
			}
			else {
				List<int> dependencies = await ModManager.GetDependencies(profileEntryMods);
				if (!modInfo.HasDependencies) return;
				_addButton.SetState(dependencies.Contains(modInfo.Id) ? "AddedAsDependencyButton" : "NotAddedButton");
			}
		}
	}

	public ModInfo GetModInfo() {
		return ModManager.ModsList.Mods[ModListId];
	}
}
