using Godot;

namespace CoreLauncher.Scripts.Menus.Setup; 

public partial class SetupPage : Control {
    [Export] public int NextPageDelay = 500;
    
    public SetupPagesManager SetupPagesManager = null;
    
    public void Finish() {
        SetupPagesManager.MoveForward();
    }
}