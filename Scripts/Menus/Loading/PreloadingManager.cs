using System.Threading.Tasks;
using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class PreloadingManager : Control {
    [Export] public LoadingBar ProgressBar;
    
    [Export] private int _nextPageDelay = 500;

    public override async void _Ready() {
        ProfileManager.Init();
        
        if (!ProfileManager.ProfileTemplatesExist()) {
            ProfileManager.CreateProfileTemplates();
        }
        
        await Task.Delay(_nextPageDelay);
			
        CallDeferred("Finish");
    }
    
    public void Finish() {
        InstanceManager.GetInstance<MenuManager>().SetActiveMenu(0);
    }
}