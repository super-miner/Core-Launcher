using System.Threading.Tasks;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class FindSteamPath : OnboardingPage {
    [Export] private FileLineEdit _steamPathLineEdit = null;
    [Export] private LoadingBar _progressBar = null;
    
    public override void _Ready() {
        _steamPathLineEdit.SetText(SteamManager.GetPath());
    }

    public async void Continue() {
        if (!_steamPathLineEdit.PathIsValid(out string outMsg)) {
            return;
        }
        
        SteamManager.SetPath(_steamPathLineEdit.Text);
        
        _progressBar.SetValue(0.0, "Looking for SteamCMD in steam directory...");

        if (FileManager.PathContains(SteamManager.GetPath(), "steamcmd.exe")) {
            _progressBar.SetValue(1.0, "Found SteamCMD in steam directory...");
        }
        else {
            _progressBar.SetValue(0.2, "Installing SteamCMD in steam directory...");
            
            FileManager.CopyFile(FileManager.GetPath(PathType.Project) + "/SteamCMD/steamcmd.exe", SteamManager.GetPath() + "/steamcmd.exe");
            
            _progressBar.SetValue(1.0, "Installed SteamCMD in the steam directory...");
        }

        await Task.Delay(500);
        
        GD.Print("Continued");
        
        CallDeferred("Finish");
    }
}