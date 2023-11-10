using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts;

public delegate void OnUIManagerLoaded();

public partial class UIManager : Node {
    public static UIManager Instance = null;

    public static event OnUIManagerLoaded UIManagerLoadedEvent;

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
        UIManagerLoadedEvent?.Invoke();
    }
}