using Godot;

namespace CoreLauncher.Scripts.UI.Generic;

public partial class ItemListEntry : Control {
    public ItemList ItemList = null;
    public int Id = -1;

    public virtual void Init() {
        
    }
}