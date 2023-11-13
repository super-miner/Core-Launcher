using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.Systems;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.Menus.Setup; 

public partial class SetupPagesManager : Node {
    [Export] private Array<PackedScene> _pages = new Array<PackedScene>();
    [Export] private PackedScene _mainMenuScene;

    private Node _currentPage = null;
    private int _currentPageNum = 0;

    public override void _EnterTree() {
        InstanceManager.AddInstance(this);
    }

    public override void _Ready() {
        SetPageNum(_currentPageNum);
    }

    public override void _ExitTree() {
        InstanceManager.RemoveInstance(this);
    }

    public void MoveForward(int amount = 1) {
        SetPageNum(_currentPageNum + amount);
    }

    public void SetPageNum(int pageNum) {
        if (_currentPage != null) {
            _currentPage.QueueFree();
        }
        
        if (pageNum >= _pages.Count) {
            SetupManager.SetupComplete = true;
            StoredDataManager.Serialize();
            
            InstanceManager.GetInstance<MenuManager>().SetActiveMenu(0);
        }
        else {
            _currentPageNum = pageNum;

            _currentPage = _pages[_currentPageNum].Instantiate();
            AddChild(_currentPage);

            if (_currentPage is SetupPage currentSetupPage) {
                currentSetupPage.SetupPagesManager = this;
            }
        }
    }
}