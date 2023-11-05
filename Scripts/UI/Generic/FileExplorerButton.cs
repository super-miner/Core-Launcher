using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class FileExplorerButton : Button {
    [Export] private FileDialog _fileExplorer;
    [Export] private FileLineEdit _fileLineEdit;

    public void OnPressed() {
        _fileExplorer.Visible = true;
    }

    public void OnDirectorySelected(string directory) {
        _fileLineEdit.SetText(directory);
    }
}