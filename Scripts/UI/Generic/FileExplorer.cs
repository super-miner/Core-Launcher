using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class FileExplorer : FileDialog {
    [Export] public string StartingDirectory = "";

    public override void _Ready() {
        CurrentDir = StartingDirectory;
    }
}