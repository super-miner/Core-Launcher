using CoreLauncher.Scripts.UI.Generic;

namespace CoreLauncher.Scripts.UI; 

public partial class SerializableItemList : ItemList {
    public void AddEntry(bool serialize) {
        SerializableItemListEntry entry = new SerializableItemListEntry(entryScene, this, serialize);
			
        entries.Add(entry);
    }
}