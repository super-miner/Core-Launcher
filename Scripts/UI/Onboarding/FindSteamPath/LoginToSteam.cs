using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class LoginToSteam : OnboardingPage {
    [Export] private LineEdit _usernameLineEdit = null;
    [Export] private LineEdit _passwordLineEdit = null;
    
    public async void Continue() {
        Task<CredentialsState> validateCredentials = SteamManager.ValidateCredentials(_usernameLineEdit.Text, _passwordLineEdit.Text);
        
        await Task.WhenAll(validateCredentials);

        CredentialsState credentialsState = validateCredentials.Result;

        if (credentialsState == CredentialsState.Invalid) {
            GD.PrintErr("Invalid credentials.");
            return;
        }

        if (credentialsState == CredentialsState.NeedsSteamGuardCode) {
            GD.Print("Needs steam guard code.");
            CallDeferred("Finish");
            return;
        }
        
        GD.Print("Debug.");
    }

    public void SkipSteamGuard() {
        OnboardingManager.MoveForward(2);
    }
}