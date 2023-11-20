using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class CustomLinkButton : Button {
    [Export] public string Link;
    
    public void OnPressed() {
        GD.Print($"Link Text: Opening \"{Link}\" in the users web browser.");
        
        OS.ShellOpen(Link);
    }
}