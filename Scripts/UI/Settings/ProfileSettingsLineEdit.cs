using System.Linq;
using CoreLauncher.Scripts.Menus.Main;
using CoreLauncher.Scripts.Systems;
using Godot;

namespace CoreLauncher.Scripts.UI.Settings; 

public partial class ProfileSettingsLineEdit : LineEdit {
    private ProfileSettingsOption _settingsOption;

    public override void _EnterTree() {
        MainMenuManager.MainMenuManagerLoadedEvent += OnUIManagerLoaded;
    }

    public override void _ExitTree() {
        MainMenuManager.MainMenuManagerLoadedEvent -= OnUIManagerLoaded;
    }

    public override void _Ready() {
        _settingsOption = GodotUtil.GetChildrenWithType<ProfileSettingsOption>(this).FirstOrDefault();

        UpdateSetting();
    }

    public void OnUIManagerLoaded() {
        MainMenuManager.Instance.ProfileList.ItemSelectedEvent += OnProfileSelected;
    }

    public void OnProfileSelected() {
        ReleaseFocus();
        
        bool success = UpdateSetting();

        if (!success) {
            GD.PrintErr($"Could not get option text input {_settingsOption.SettingName}.");
        }
    }
    
    public void OnInputChanged(string newText) {
        _settingsOption.SetSetting(newText);
    }

    private bool UpdateSetting() {
        string textTemp = Text;
        bool success = _settingsOption.GetSetting(ref textTemp);
        Text = textTemp;

        return success;
    }
}