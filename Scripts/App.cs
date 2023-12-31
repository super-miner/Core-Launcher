using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts; 

public partial class App : Node {
    public override void _Notification(int what) {
        if (what == NotificationWMCloseRequest) {
            OnQuit();
            
            GetTree().Quit();
        }
    }

    public override void _Ready() {
        SetupManager.Init();
        GameManager.Init();
        ModManager.Init();
        
        StoredDataManager.Deserialize();
    }

    private void OnQuit() {
        StoredDataManager.Serialize();
    }
}