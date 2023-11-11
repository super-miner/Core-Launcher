using CoreLauncher.Scripts.UI.Generic;

namespace CoreLauncher.Scripts.UI; 

public partial class AddModButton : StateButton {
    public void OnAddPressed() {
        SetState("AddedButton");
    }

    public void OnAddedPressed() {
        SetState("NotAddedButton");
    }
}