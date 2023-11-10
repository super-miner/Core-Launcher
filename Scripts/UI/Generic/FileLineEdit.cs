using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

public partial class FileLineEdit : LineEdit {
    [Export] public string FolderMustContain = "";
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
        if (FileManager.DirectoryExists(Text)) {
            if (FolderMustContain != "") {
                if (FileManager.DirectoryContains(Text, FolderMustContain)) {
                    outMsg = SuccessText;
                    return true;
                }
                else {
                    outMsg = FileErrorText.Replace("{folderMustContain}", FolderMustContain);
                    return false;
                }
            }
            else {
                outMsg = SuccessText;
                return true;
            }
        }
        else {
            outMsg = FolderErrorText;
            return false;
        }
    }
}