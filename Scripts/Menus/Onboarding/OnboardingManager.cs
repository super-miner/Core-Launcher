using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.Menus.Onboarding; 

public partial class OnboardingManager : Node {
    public bool OnboardingComplete = false;
    
    [Export] private Array<PackedScene> _pages = new Array<PackedScene>();
    [Export] private PackedScene _mainMenuScene;

    private Node _currentPage = null;
    private int _currentPageNum = 0;

    public override void _Ready() {
        SetPageNum(_currentPageNum);
        
        StoredDataManager.DeserializeStoredDataEvent += OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent += OnSerializeStoredData;
        
        if (StoredDataManager.HasDeserialized) {
            OnDeserializeStoredData();
        }
    }

    public override void _ExitTree() {
        StoredDataManager.DeserializeStoredDataEvent -= OnDeserializeStoredData;
        StoredDataManager.SerializeStoredDataEvent -= OnSerializeStoredData;
        
        OnSerializeStoredData();
    }

    public void MoveForward(int amount = 1) {
        SetPageNum(_currentPageNum + amount);
    }

    public void SetPageNum(int pageNum) {
        if (_currentPage != null) {
            _currentPage.QueueFree();
        }
        
        if (pageNum >= _pages.Count) {
            OnboardingComplete = true;
            StoredDataManager.Serialize();
            
            MenuManager.Instance.SetActiveMenu(2);
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
    
    private void OnDeserializeStoredData() {
        OnboardingComplete = StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete;
    }

    private void OnSerializeStoredData() {
        StoredDataManager.GetStoredDataGroup<PersistentDataGroup>().OnboardingComplete = OnboardingComplete;
    }
}