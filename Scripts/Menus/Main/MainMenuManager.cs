using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main;

public delegate void OnMainMenuManagerLoaded();

public partial class MainMenuManager : Node {
    public static event OnMainMenuManagerLoaded MainMenuManagerLoadedEvent;
    
    [Export] public ProfileList ProfileList;
    [Export] public NameProfilePopup NameProfilePopup;
    [Export] public LoadingBar PlayProgressBar;
    [Export] public TabContainer OptionsTabs;

    public override void _EnterTree() {
        InstanceManager.AddInstance(this);
        
        ProfileList.ItemSelectedEvent += OnItemSelected;
    }
    
    public override void _Ready() {
        MainMenuManagerLoadedEvent?.Invoke();
    }

    public override void _ExitTree() {
        ProfileList.ItemSelectedEvent -= OnItemSelected;
        
        InstanceManager.RemoveInstance(this);
    }

    private void OnItemSelected() {
        OptionsTabs.CurrentTab = 0;
    }
}