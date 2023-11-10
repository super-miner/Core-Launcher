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
        _progressBar.SetValue(0.0, "Saving data...");

        ModIOManager.UserID = _userIDLineEdit.Text;
        ModIOManager.ApiKey = _apiKeyLineEdit.Text;
        StoredDataManager.Serialize();
        
        _progressBar.SetValue(1.0, "Saved data.");
        
        await Task.Delay(nextPageDelay);
        
        CallDeferred("Finish");
    }
}