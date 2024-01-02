using System.Threading.Tasks;
using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class PreloadingManager : Control {
    [Export] public LoadingBar ProgressBar;
    
    [Export] private int _nextPageDelay = 500;

    public override void _EnterTree() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;

        if (StoredDataManager.HasDeserialized) {
            OnStoredDataDeserialized();
        }
    }

    public override void _Ready() {
        ProfileManager.Init();
        
        StoredDataManager.Deserialize();
    }
    
    public override void _ExitTree() {
        StoredDataManager.StoredDataDeserializedEvent -= OnStoredDataDeserialized;
    }
    
    public void Finish() {
        InstanceManager.GetInstance<MenuManager>().SetActiveMenu(0);
    }

    private async void OnStoredDataDeserialized() {
        if (!ProfileManager.ProfileTemplatesExist()) {
            ProfileManager.CreateProfileTemplates();
        }
        
        await Task.Delay(_nextPageDelay);
			
        CallDeferred("Finish");
    }
}