using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class DropdownMenu : Button {
    [Signal] public delegate void OnItemSelectedEventHandler(string itemName);
    
    private Control _child;

    public override void _Ready() {
        _child = GetChild<Control>(0);
    }

    public virtual void OnPressed() {
        _child.Visible = !_child.Visible;
    }

    public void OnChildItemSelected(string itemName) {
        _child.Visible = false;
        
        EmitSignal("OnItemSelected", itemName);
    }
}