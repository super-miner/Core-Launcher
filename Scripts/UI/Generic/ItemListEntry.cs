using Godot;

public class ItemListEntry {
    public Control node = null;

    public ItemListEntry(PackedScene nodeScene, Node parent) {
        CreateNode(nodeScene, parent);
    }

    public void CreateNode(PackedScene nodeScene, Node parent) {
        Node nodeNode = nodeScene.Instantiate();
        
        if (nodeNode is Control) {
            node = nodeNode as Control;
            
            node.Reparent(parent);
        }
        else {
            GD.PrintErr("ItemList node did not derive from Control.");
            
            nodeNode.QueueFree();
        }
    }
}