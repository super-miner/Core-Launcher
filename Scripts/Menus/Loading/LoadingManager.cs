using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class LoadingManager : Control {
    public bool OnboardingComplete = false;
    
    [Export] private PackedScene _onboardingScene;
    
    public override void _Ready() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
        
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;

        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }

    public override void _ExitTree() {
        StoredDataManager.StoredDataDeserializedEvent -= OnStoredDataDeserialized;
        
        StoredDataManager.DeserializeStoredDataEvent -= OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent -= OnSerializeStoredData;
        
        OnSerializeStoredData();
    }

    private void OnStoredDataDeserialized() {
        if (OnboardingComplete) {
            MenuManager.Instance.SetActiveMenu(2);
        }
        else {
            MenuManager.Instance.SetActiveMenu(1);
        }
    }
    
    private void OnDeserializeStoredData() {
        OnboardingComplete = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete;
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete = OnboardingComplete;
    }
}