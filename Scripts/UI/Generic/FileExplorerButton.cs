using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class FileExplorerButton : Button {
    [Export] private FileExplorer _fileExplorer;
    [Export] private FileLineEdit _fileLineEdit;

    public void OnPressed() {
        _fileExplorer.CurrentDir = FileUtil.DirectoryExists(_fileLineEdit.Text) ? _fileLineEdit.Text : "";
        _fileExplorer.Visible = true;
    }

    public void OnDirectorySelected(string directory) {
        _fileLineEdit.SetText(directory);
    }
}