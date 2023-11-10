using CoreLauncher.Scripts.StoredData;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class LoadingManager : Control {
    [Export] private PackedScene _onboardingScene;
    
    public override void _Ready() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
    }

    private void OnStoredDataDeserialized() {
        MenuManager.instance.SetActiveMenu(1);
    }
}