using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main;

public delegate void OnMainMenuManagerLoaded();

public partial class MainMenuManager : Node {
    public static MainMenuManager Instance = null;

    public static event OnMainMenuManagerLoaded MainMenuManagerLoadedEvent;

    [Export] public ProfileList ProfileList = null;

    public override void _EnterTree() {
        if (Instance != null) {
            Instance.QueueFree();
        }
        
        Instance = this;
    }

    public override void _ExitTree() {
        if (Instance == this) {
            Instance = null;
        }
    }

    public override void _Ready() {
        MainMenuManagerLoadedEvent?.Invoke();
    }
}