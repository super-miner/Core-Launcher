using CoreLauncher.Scripts.Systems;
using Godot;
using Godot.Collections;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class FileLineEdit : LineEdit {
    [Export] public Array<string> FolderMustContain = new Array<string>();
    [Export] public string FileErrorText = "The selected folder must contain {folderMustContain}.";
    [Export] public string FolderErrorText = "The path specified does not exist.";
    [Export] public string SuccessText = "Found path.";
    
    [Export] private Label _errorLabel = null;
    [Export] private Label _successLabel = null;

    public override void _Ready() {
        UpdateFeedback();
    }

    public void OnTextChanged(string newText) {
        UpdateFeedback();
    }

    public void SetText(string text) {
        ReleaseFocus();
        
        Text = text;
        UpdateFeedback();
    }

    public void UpdateFeedback() {
        if (PathIsValid(out string outMsg)) {
            _errorLabel.Visible = false;
            _successLabel.Visible = true;

            _successLabel.Text = outMsg;
        }
        else {
            _successLabel.Visible = false;
            _errorLabel.Visible = true;

            _errorLabel.Text = outMsg;
        }
    }

    public virtual bool PathIsValid(out string outMsg) {
        if (!FileUtil.DirectoryExists(Text)) {
            outMsg = FolderErrorText;
            return false;
        }
        
        foreach (string contains in FolderMustContain) {
            if (!FileUtil.DirectoryContains(Text, contains)) {
                outMsg = FileErrorText.Replace("{folderMustContain}", contains);
                return false;
            }
        }
        
        outMsg = SuccessText;
        return true;
    }
}