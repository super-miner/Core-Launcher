using System.Threading.Tasks;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public partial class NameProfilePopup : Window {
    [Export] private LineEdit _nameLineEdit;
    private TaskCompletionSource<string> _task = null;
    
    public void Open(TaskCompletionSource<string> task) {
        if (InUse()) {
            Close(null);
        }

        _task = task;
        
        Visible = true;
    }

    public void Close(string result) {
        _nameLineEdit.Text = "";
        
        _task.SetResult(result);
        _task = null;

        Visible = false;
    }

    public bool InUse() {
        return _task != null;
    }
    
    public void OnCloseRequested() {
        Close(null);
    }

    public void OnCancelButtonPressed() {
        Close(null);
    }

    public void OnOkButtonPressed() {
        Close(_nameLineEdit.Text);
    }
}