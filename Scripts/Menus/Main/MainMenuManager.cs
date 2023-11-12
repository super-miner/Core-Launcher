using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main;

public delegate void OnMainMenuManagerLoaded();

public partial class MainMenuManager : Node {
    public static event OnMainMenuManagerLoaded MainMenuManagerLoadedEvent;
    
    [Export] public ProfileList ProfileList = null;

    [Export] public LoadingBar PlayProgressBar;

    public override void _EnterTree() {
        InstanceManager.AddInstance(this);
    }

    public override void _ExitTree() {
        InstanceManager.RemoveInstance(this);
    }

    public override void _Ready() {
        MainMenuManagerLoadedEvent?.Invoke();
    }
}