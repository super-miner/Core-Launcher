using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus;

public partial class CoreLauncherOptions : PanelContainer
{
	[Export] private FileLineEdit _steamPath;
	[Export] private FileLineEdit _gamePath;
	[Export] private FileLineEdit _serverPath;
	[Export] private FileLineEdit _appDataPath;

	public override void _Ready()
	{
		_steamPath.SetText(GameManager.SteamExePath);
		_gamePath.SetText(GameManager.SteamGamePath);
		_serverPath.SetText(GameManager.SteamGameServerPath);
		_appDataPath.SetText(GameManager.AppDataPath);
		InstanceManager.GetInstance<MainMenuManager>().ProfileList.ItemSelectedEvent += OnItemSelected;
	}

	public void _on_save_button_down()
	{
		GameManager.SteamExePath = _steamPath.Text;
		GameManager.SteamGamePath = _gamePath.Text;
		GameManager.SteamGameServerPath = _serverPath.Text;
		GameManager.AppDataPath = _appDataPath.Text;
		StoredDataManager.Serialize();
	}

	public void _on_button_button_down(string info)
	{
		switch (info)
		{
			case "Steam":
			{
				OS.ShellOpen(GameManager.SteamExePath);
				return;
			}
			case "Game":
			{
				OS.ShellOpen(GameManager.SteamGamePath);
				return;
			}
			case "Server":
			{
				OS.ShellOpen(_serverPath.Text);
				return;
			}
			case "AppData":
			{
				OS.ShellOpen(_appDataPath.Text);
				return;
			}
		}
	}

	private void OnItemSelected()
	{
		Visible = false;
	}
}