using CoreLauncher.Scripts.ModIO;
using Godot;

namespace CoreLauncher.Scripts.Menus.Main.Tabs; 

public partial class ModsTabManager : Control {
    [Export] private Label _modsLoadedLabel;

    public override void _Ready() {
        _modsLoadedLabel.Text = $"Showing {ModManager.ModsList.Mods.Count} out of {ModManager.ModsList.Mods.Count} mods.";
    }
}