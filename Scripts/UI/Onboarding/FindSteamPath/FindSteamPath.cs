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
        
        _progressBar.SetValue(0.0, "Saving path...");
        
        SteamManager.SetPath(_steamPathLineEdit.Text);
        
        _progressBar.SetValue(1.0, "Saved path.");

        await Task.Delay(nextPageDelay);
        
        CallDeferred("Finish");
    }
}