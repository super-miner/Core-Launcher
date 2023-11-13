using System.Threading.Tasks;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Setup; 

public partial class GetModIOAPIKey : SetupPage {
    [Export] private LoadingBar _progressBar = null;
    [Export] private LineEdit _apiKeyLineEdit = null;
    
    public async void Continue() {
        _progressBar.SetValue("Validation", 0.0, "Validating API key...");

        bool apiKeyValid = await ModManager.ValidateApiKey(_apiKeyLineEdit.Text);
        if (!apiKeyValid) {
            _progressBar.SetValue("Validation", 0.0, "Invalid API key...");
            return;
        }
        
        _progressBar.SetValue("Validation", 1.0, "API key working properly.");
        _progressBar.SetValue("ApiKey", 0.0, "Setting API key...");
        
        ModManager.SetApiKey(_apiKeyLineEdit.Text);
        
        _progressBar.SetValue("ApiKey", 1.0, "Set API key.");
        
        await Task.Delay(NextPageDelay);
        
        CallDeferred("Finish");
    }
}