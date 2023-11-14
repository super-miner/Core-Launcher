using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main.Tabs; 

public partial class TabsManager : TabContainer {
    public override void _EnterTree() {
        InstanceManager.GetInstance<MainMenuManager>().ProfileList.ItemSelectedEvent += OnItemSelected;
    }
    
    public override void _ExitTree() {
        InstanceManager.GetInstance<MainMenuManager>().ProfileList.ItemSelectedEvent -= OnItemSelected;
    }

    public void OnTogglePressed() {
        Visible = !Visible;
    }

    private void OnItemSelected() {
        Visible = true;
    }
}