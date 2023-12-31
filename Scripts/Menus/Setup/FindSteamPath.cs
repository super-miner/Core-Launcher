using System.Threading.Tasks;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Setup; 

public partial class FindSteamPath : SetupPage {
    [Export] private FileLineEdit _steamPathLineEdit = null;
    [Export] private LoadingBar _progressBar = null;
    
    public override void _Ready() {
        _steamPathLineEdit.SetText(FileUtil.GetPath(PathType.Steam));
    }

    public async void Continue() {
        if (!_steamPathLineEdit.PathIsValid(out string outMsg)) {
            _progressBar.SetValue("SteamPath", 0.0, "Invalid Steam path...");
            return;
        }
        
        _progressBar.SetValue("SteamPath", 0.0, "Setting path...");
        
        GameManager.SteamPath = _steamPathLineEdit.Text;
        
        _progressBar.SetValue("SteamPath", 1.0, "Set path.");

        await Task.Delay(NextPageDelay);
        
        CallDeferred("Finish");
    }
}