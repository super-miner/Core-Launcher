using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts;

public delegate void OnUIManagerLoaded();

public partial class UIManager : Node {
    public static UIManager Instance = null;

    public static event OnUIManagerLoaded UIManagerLoadedEvent;

    [Export] public ProfileList ProfileList = null;

    public override void _Ready() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            QueueFree();
        }
        
        UIManagerLoadedEvent.Invoke();
    }
}