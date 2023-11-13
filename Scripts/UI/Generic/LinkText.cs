using Godot;

namespace CoreLauncher.Scripts.UI.Generic; 

[GlobalClass]
public partial class LinkText : RichTextLabel {
    public void OnMetaPressed(string meta) {
        GD.Print($"Link Text: Opening \"{meta}\" in the users web browser.");
        
        OS.ShellOpen(meta);
    }
}