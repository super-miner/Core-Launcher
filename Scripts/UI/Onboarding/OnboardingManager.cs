using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class OnboardingManager : Node {
    [Export] private Array<PackedScene> _pages = new Array<PackedScene>();
    [Export] private PackedScene _mainMenuScene;

    private Node _currentPage = null;
    private int _currentPageNum = 0;

    public override void _Ready() {
        SetPageNum(_currentPageNum);
    }

    public void MoveForward(int amount = 1) {
        int newPageNum = _currentPageNum + amount;
        if (newPageNum < _pages.Count) {
            SetPageNum(newPageNum);
        }
    }

    public void SetPageNum(int pageNum) {
        if (_currentPage != null) {
            _currentPage.QueueFree();
        }
        
        if (pageNum >= _pages.Count) {
            GetTree().ChangeSceneToPacked(_mainMenuScene);
        }
        else {
            _currentPageNum = pageNum;

            _currentPage = _pages[_currentPageNum].Instantiate();
            AddChild(_currentPage);

            if (_currentPage is OnboardingPage currentOnboardingPage) {
                currentOnboardingPage.OnboardingManager = this;
            }
        }
    }
}