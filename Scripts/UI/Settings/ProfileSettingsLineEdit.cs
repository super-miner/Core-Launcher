using Godot;

namespace CoreLauncher.Scripts.UI.Settings; 

public partial class ProfileSettingsLineEdit : LineEdit {
    [Export] private ProfileSettingsOption _settingsOption = null;
    
    public override void _Ready() {
        UIManager.UIManagerLoadedEvent += OnUIManagerLoaded;
    }

    public void OnUIManagerLoaded() {
        UIManager.Instance.ProfileList.ItemSelectedEvent += OnProfileSelected;
    }

    public void OnProfileSelected() {
        ReleaseFocus();
        
        string textTemp = Text;
        bool success = _settingsOption.GetSetting(ref textTemp);
        Text = textTemp;
        
        GD.Print("Profile selected " + ((ProfileListEntry) UIManager.Instance.ProfileList.GetSelectedEntry()).GetName());

        if (!success) {
            GD.PrintErr($"Could not set option text input {_settingsOption.SettingName}.");
        }
    }
    
    public void OnInputChanged(string newText) {
        _settingsOption.SetSetting(newText);
    }
}