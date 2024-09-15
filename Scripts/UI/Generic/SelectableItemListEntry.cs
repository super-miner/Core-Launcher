using Godot;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class SelectableItemListEntry : ItemListEntry {
    public new SelectableItemList ItemList = null;

    [Export] private Button _button;
    
    public void Select() {
        _button.Disabled = true;
    }

    public void Deselect() {
        _button.Disabled = false;
    }
    
    public void OnPressed() {
        ItemList.SetSelectedEntry(this);
    }
}