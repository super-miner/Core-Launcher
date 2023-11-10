using System.Threading.Tasks;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class GetModIOAPIKey : OnboardingPage {
    public string ModIOUserID = "";
    public string ModIOApiKey = "";
    
    [Export] private LoadingBar _progressBar = null;
    [Export] private LineEdit _userIDLineEdit = null;
    [Export] private LineEdit _apiKeyLineEdit = null;
    
    public override void _Ready() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
    }
    
    public async void Continue() {
        _progressBar.SetValue(0.0, "Saving data...");

        ModIOUserID = _userIDLineEdit.Text;
        ModIOApiKey = _apiKeyLineEdit.Text;
        StoredDataManager.Serialize();
        
        _progressBar.SetValue(1.0, "Saved data.");
        
        await Task.Delay(nextPageDelay);
        
        CallDeferred("Finish");
    }
    
    private void OnStoredDataDeserialized() {
        ModIOUserID = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOUserID;
        ModIOApiKey = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey;
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOUserID = ModIOUserID;
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().ModIOApiKey = ModIOApiKey;
    }
}