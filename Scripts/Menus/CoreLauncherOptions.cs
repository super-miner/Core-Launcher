using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
// using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus;

public partial class CoreLauncherOptions : MarginContainer
{
	[Export] private FileLineEdit SteamPath;
	[Export] private FileLineEdit GamePath;
	[Export] private FileLineEdit ServerPath;
	[Export] private FileLineEdit AppDataPath;

	[Export] private Button SteamButton;
	[Export] private Button GameButton;
	[Export] private Button ServerButton;
	[Export] private Button AppDataButton;
	public override void _Ready()
	{
		SteamPath.SetText(GameManager.SteamExePath);
		GamePath.SetText(GameManager.SteamGamePath);
		ServerPath.SetText(GameManager.SteamGameServerPath);
		AppDataPath.SetText(GameManager.AppDataPath);
	}

	public void _on_save_button_down()
	{
		GameManager.SteamExePath = SteamPath.Text;
		GameManager.SteamGamePath = GamePath.Text;
		GameManager.SteamGameServerPath = ServerPath.Text;
		GameManager.AppDataPath = AppDataPath.Text;
		StoredDataManager.Serialize();
	}

	public void _on_button_button_down(string info)
	{
		GD.Print(info);
		switch (info)
		{
			case "Steam":
			{
				GD.Print(info);
				OS.ShellOpen(GameManager.SteamExePath);
				return;
			}
			case "Game":
			{
				GD.Print(info);
				OS.ShellOpen(GameManager.SteamGamePath);
				return;
			}
			case "Server":
			{
				GD.Print(info);
				OS.ShellOpen(ServerPath.Text);
				return;
			}
			case "AppData":
			{
				GD.Print(info);
				OS.ShellOpen(AppDataPath.Text);
				return;
			}
		}
	}
}