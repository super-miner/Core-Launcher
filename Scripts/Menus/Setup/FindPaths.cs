using System.Threading.Tasks;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Setup; 

public partial class FindPaths : SetupPage {
    [Export] private FileLineEdit _steamExePathLineEdit;
    [Export] private FileLineEdit _steamGamePathLineEdit;
    [Export] private FileLineEdit _steamServerPathLineEdit;
    [Export] private FileLineEdit _appDataPathLineEdit;
    [Export] private LoadingBar _progressBar;
    
    public override void _Ready() {
        _steamExePathLineEdit.SetText(FileUtil.GetPath(PathType.SteamExe));
        _steamGamePathLineEdit.SetText(FileUtil.GetPath(PathType.GameApp));
        _steamServerPathLineEdit.SetText(FileUtil.GetPath(PathType.ServerApp));
        _appDataPathLineEdit.SetText(FileUtil.GetPath(PathType.CoreKeeperAppData));
    }

    public async void Continue() {
        if (!_steamExePathLineEdit.PathIsValid(out string outMsg)) {
            _progressBar.SetValue("SteamPath", 0.0, "Invalid Steam Exe path...");
            return;
        }
        
        if (!_steamGamePathLineEdit.PathIsValid(out outMsg)) {
            _progressBar.SetValue("SteamPath", 0.0, "Invalid Steam Games path...");
            return;
        }
        
        if (!_steamServerPathLineEdit.PathIsValid(out outMsg)) {
            _progressBar.SetValue("SteamPath", 0.0, "Invalid Steam Games path...");
            return;
        }
        
        if (!_appDataPathLineEdit.PathIsValid(out outMsg)) {
            _progressBar.SetValue("SteamPath", 0.0, "Invalid Core Keeper App Data path...");
            return;
        }
        
        _progressBar.SetValue("SteamPath", 0.0, "Setting path...");
        
        GameManager.SteamExePath = _steamExePathLineEdit.Text;
        GameManager.SteamGamePath = _steamGamePathLineEdit.Text;
        GameManager.SteamGameServerPath = _steamServerPathLineEdit.Text;
        GameManager.AppDataPath = _appDataPathLineEdit.Text;
        
        _progressBar.SetValue("SteamPath", 1.0, "Set path.");

        await Task.Delay(NextPageDelay);
        
        CallDeferred("Finish");
    }
}