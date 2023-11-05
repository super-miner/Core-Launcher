using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI.Onboarding; 

public partial class FindSteamPath : Control {
    [Export] private FileLineEdit steamPathLineEdit = null;
    
    public override void _Ready() {
        steamPathLineEdit.SetText(SteamManager.GetPath());
    }
}