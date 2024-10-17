using CoreLauncher.Scripts.Systems;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.Menus; 

public partial class MenuManager : Control {
    [Export] private int _activeMenuIndex;
    [Export] private Array<PackedScene> _menuScenes = new Array<PackedScene>();
    private Node _activeMenu;

    public override void _EnterTree() {
        InstanceManager.AddInstance(this);
    }

    public override void _ExitTree() {
        InstanceManager.RemoveInstance(this);
    }

    public override void _Ready() {
        SetActiveMenu(_activeMenuIndex);
    }

    public void SetActiveMenu(int index) {
        if (_activeMenu != null) {
            _activeMenu.QueueFree();
        }
        
        _activeMenu = _menuScenes[index].Instantiate();
        AddChild(_activeMenu);
        
        GD.Print($"Menu Manager: Changed active menu to {_activeMenu.Name}.");
    }
}