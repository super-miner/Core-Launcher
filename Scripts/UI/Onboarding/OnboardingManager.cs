using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class OnboardingManager : Node {
    [Export] private Array<PackedScene> _pages = new Array<PackedScene>();

    private Node _currentPage = null;
    private int _currentPageNum = 0;

    public override void _Ready() {
        SetPageNum(_currentPageNum);
    }

    public void MoveForward(int amount) {
        int newPageNum = _currentPageNum + amount;
        if (newPageNum < _pages.Count) {
            SetPageNum(newPageNum);
        }
    }

    public void SetPageNum(int pageNum) {
        if (pageNum >= _pages.Count) {
            GD.PrintErr("The page index was out of bounds.");
        }
        
        if (_currentPage != null) {
            _currentPage.QueueFree();
        }
        
        _currentPageNum = pageNum;

        _currentPage = _pages[_currentPageNum].Instantiate();
        AddChild(_currentPage);

        if (_currentPage is OnboardingPage currentOnboardingPage) {
            currentOnboardingPage.OnboardingManager = this;
        }
    }
}