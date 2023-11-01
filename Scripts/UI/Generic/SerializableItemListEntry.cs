using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.UI; 

public class SerializableItemListEntry : ItemListEntry {
    public SerializableItemListEntry(PackedScene nodeScene, Node parent, bool serialize) : base(nodeScene, parent) {
        if (serialize) {
            StoredDataManager.data.profiles.Add(new StoredProfile("Test"));
        }
    }
}