using Godot;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemListEntry : Button {
    public ItemList ItemList = null;
    public int Id = -1;
    
    public void Select() {
        GD.Print("Selected entry");
        
        ItemList.GetSelectedEntry()?.Deselect();
        
        ItemList.SetSelectedEntry(Id);

        Disabled = true;
    }

    public void Deselect() {
        Disabled = false;
    }
}