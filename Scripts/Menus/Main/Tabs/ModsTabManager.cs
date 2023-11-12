using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.ModIO.JsonStructures;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main.Tabs; 

public partial class ModsTabManager : Control {
    [Export] public ModList ModList;
    
    [Export] private Label _modsLoadedLabel;
    [Export] private CheckBox _showLibraryModsCheckBox;

    public override void _Ready() {
        _modsLoadedLabel.Text = $"Showing {ModList.ModsShowing} out of {ModManager.ModsList.Mods.Count} mods.";
        _showLibraryModsCheckBox.ButtonPressed = ModList.ShowLibraryMods;
    }

    public void OnShowLibraryModsCheckboxPressed() {
        ModList.SetShowLibraryMods(_showLibraryModsCheckBox.ButtonPressed);
        
        _modsLoadedLabel.Text = $"Showing {ModList.ModsShowing} out of {ModManager.ModsList.Mods.Count} mods.";
    }
}