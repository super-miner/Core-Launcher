using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class WindowToggleButton : Button {
    [Export] private Window _toggle;

    public void OnPressed() {
        _toggle.Visible = !_toggle.Visible;
    }
}