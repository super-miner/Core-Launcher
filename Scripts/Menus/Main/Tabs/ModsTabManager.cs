using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main.Tabs; 

public partial class ModsTabManager : Control {
    [Export] public ModList ModList;
    
    [Export] private Label _modsLoadedLabel;
    [Export] private CheckBox _showLibraryModsCheckBox;

    public override void _EnterTree() {
        InstanceManager.AddInstance(this);
    }
    
    public override void _Ready() {
        UpdateModsShowingText();
        _showLibraryModsCheckBox.ButtonPressed = ModList.ShowLibraryMods;
    }
    
    public override void _ExitTree() {
        InstanceManager.RemoveInstance(this);
    }

    public void OnShowLibraryModsCheckboxPressed() {
        ModList.SetShowLibraryMods(_showLibraryModsCheckBox.ButtonPressed);

        UpdateModsShowingText();
    }

    public void UpdateModsShowingText() {
        _modsLoadedLabel.Text = $"Showing {ModList.ModsShowing} out of {ModManager.ModsList.Mods.Count} mods.";
    }
}