using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class OnboardingPage : Control {
    public OnboardingManager OnboardingManager = null;
    
    public void Finish() {
        OnboardingManager.MoveForward(1);
    }
}