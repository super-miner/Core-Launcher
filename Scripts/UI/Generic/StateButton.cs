using System.Linq;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class StateButton : Control {
    private Control _currentStateNode;
    
    public void SetState(string state) {
        if (_currentStateNode != null) {
            _currentStateNode.Visible = false;
        }

        _currentStateNode = (Control) GodotUtil.GetChildrenWithName(this, state).FirstOrDefault(node => node is Control);

        if (_currentStateNode != null) {
            _currentStateNode.Visible = true;
        }
    }
}