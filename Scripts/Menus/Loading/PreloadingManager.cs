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

    public override async void _Ready() {
        StoredDataManager.Deserialize();
        
        SetupManager.Init();
        GameManager.Init();
        
        if (!SetupManager.SetupComplete) {
            ProfileManager.Init();
            
            InstanceManager.GetInstance<MenuManager>().SetActiveMenu(1);
            return;
        }
        
        ProfileManager.Init();
        
        await Task.Delay(_nextPageDelay);
			
        CallDeferred("Finish");
    }
    
    public void Finish() {
        InstanceManager.GetInstance<MenuManager>().SetActiveMenu(0);
    }
}