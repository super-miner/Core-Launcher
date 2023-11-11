using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.Menus; 

public partial class MenuManager : Control {
    public static MenuManager Instance = null;
    
    [Export] private int _activeMenuIndex = 0;
    [Export] private Array<PackedScene> _menuScenes = new Array<PackedScene>();
    private Node _activeMenu = null;

    public override void _EnterTree() {
        
    }
    
    public override void _Ready() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            QueueFree();
            return;
        }
        
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