using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class VisibilityToggleButton : Button {
    [Export] private Control _toggle;

    public void OnPressed() {
        _toggle.Visible = !_toggle.Visible;
    }
}