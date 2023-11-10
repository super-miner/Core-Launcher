using System.Threading.Tasks;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Onboarding; 

public partial class GetModIOAPIKey : OnboardingPage {
    [Export] private LoadingBar _progressBar = null;
    [Export] private LineEdit _userIDLineEdit = null;
    [Export] private LineEdit _apiKeyLineEdit = null;
    
    public async void Continue() {
        _progressBar.SetValue(0.0, "Setting User ID...");

        ModIOManager.UserID = _userIDLineEdit.Text;
        
        _progressBar.SetValue(0.5, "Set User ID.");
        _progressBar.SetValue(0.5, "Setting API Key...");
        
        ModIOManager.ApiKey = _apiKeyLineEdit.Text;
        
        _progressBar.SetValue(1.0, "Set API Key.");
        
        await Task.Delay(nextPageDelay);
        
        CallDeferred("Finish");
    }
}