using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class LoadingManager : Control {
    public bool OnboardingComplete = false;
    
    [Export] private PackedScene _onboardingScene;
    [Export] private int _totalSteps;
    private int _stepsComplete;
    
    public override void _Ready() {
        ModManager.ModInfoLoadedEvent += OnModInfoLoaded;
        
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;

        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }

    public override void _ExitTree() {
        ModManager.ModInfoLoadedEvent -= OnModInfoLoaded;
        
        StoredDataManager.StoredDataDeserializedEvent -= OnStoredDataDeserialized;
        StoredDataManager.DeserializeStoredDataEvent -= OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent -= OnSerializeStoredData;
        
        OnSerializeStoredData();
    }

    public void StepComplete() {
        SetStepsComplete(_stepsComplete + 1);
    }

    public void SetStepsComplete(int amount) {
        _stepsComplete = amount;

        if (_stepsComplete >= _totalSteps) {
            if (OnboardingComplete) {
                MenuManager.Instance.SetActiveMenu(2);
            }
            else {
                MenuManager.Instance.SetActiveMenu(1);
            }
        }
    }

    private void OnModInfoLoaded() {
        GD.Print("Loading Manager: Fetched mod data.");
        
        StepComplete();
    }

    private void OnStoredDataDeserialized() {
        GD.Print("Loading Manager: Deserialized app data.");
        
        StepComplete();
    }
    
    private void OnDeserializeStoredData() {
        OnboardingComplete = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete;
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete = OnboardingComplete;
    }
}