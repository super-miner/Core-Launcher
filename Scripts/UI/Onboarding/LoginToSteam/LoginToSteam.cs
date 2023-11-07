using System.Threading.Tasks;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class LoginToSteam : OnboardingPage {
    [Export] private LoadingBar _progressBar = null;
    [Export] private LineEdit _usernameLineEdit = null;
    [Export] private LineEdit _passwordLineEdit = null;
    
    public async void Continue() {
        SteamManager.Login(_usernameLineEdit.Text, _passwordLineEdit.Text, _progressBar);
    }

    public void SkipSteamGuard() {
        OnboardingManager.MoveForward(2);
    }
}