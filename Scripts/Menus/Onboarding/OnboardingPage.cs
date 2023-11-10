using Godot;

namespace CoreLauncher.Scripts.Menus.Onboarding; 

public partial class OnboardingPage : Control {
    [Export] public int nextPageDelay = 500;
    
    public OnboardingManager OnboardingManager = null;
    
    public void Finish() {
        OnboardingManager.MoveForward();
    }
}