using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI;

public partial class OptionsWindow : Window
{
	[Export] private FileLineEdit _steamPathLineEdit;
	[Export] private FileLineEdit _gamePathLineEdit;
	[Export] private FileLineEdit _serverPathLineEdit;
	[Export] private FileLineEdit _appDataPathLineEdit;

	public void Toggle() {
		Visible = !Visible;

		if (Visible) {
			_steamPathLineEdit.SetText(GameManager.SteamExePath);
			_gamePathLineEdit.SetText(GameManager.SteamGamePath);
			_serverPathLineEdit.SetText(GameManager.SteamGameServerPath);
			_appDataPathLineEdit.SetText(GameManager.AppDataPath);
		}
	}

	public void OnCloseRequested() {
		Save();
		
		Visible = false;
	}

	public void Save() {
		if (_steamPathLineEdit.PathIsValid(out string _)) {
			GD.Print("Settings Menu: Saving Steam path.");
			GameManager.SteamExePath = _steamPathLineEdit.Text;
		}

		if (_gamePathLineEdit.PathIsValid(out string _)) {
			GD.Print("Settings Menu: Saving Client path.");
			GameManager.SteamGamePath = _gamePathLineEdit.Text;
		}

		if (_serverPathLineEdit.PathIsValid(out string _)) {
			GD.Print("Settings Menu: Saving Server path.");
			GameManager.SteamGameServerPath = _serverPathLineEdit.Text;
		}

		if (_appDataPathLineEdit.PathIsValid(out string _)) {
			GD.Print("Settings Menu: Saving App Data path.");
			GameManager.AppDataPath = _appDataPathLineEdit.Text;
		}

		StoredDataManager.Serialize();
	}
}
