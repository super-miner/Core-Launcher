using CoreLauncher.Scripts.StoredData;
using Godot;

namespace CoreLauncher.Scripts; 

public partial class App : Node {
    public override void _Notification(int what) {
        if (what == NotificationWMCloseRequest) {
            OnQuit();
            
            GetTree().Quit();
        }
    }

    private void OnQuit() {
        StoredDataManager.Serialize();
    }
}