using System.Collections.Generic;
using Godot;

namespace CoreLauncher.Scripts; 

public static class GodotUtil {
    public static IEnumerable<T> GetChildrenWithType<T>(Node parent) where T : Node {
        foreach (Node node in parent.GetChildren()) {
            if (node is T castedNode) {
                yield return castedNode;
            }
        }
    }
    
    public static IEnumerable<Node> GetChildrenWithName(Node parent, string name) {
        foreach (Node node in parent.GetChildren()) {
            if (node.Name == name) {
                yield return node;
            }
        }
    }
}