using System.Collections.Generic;
using Godot;

namespace CoreLauncher.Scripts; 

public static class GodotUtil {
    public static IEnumerable<T> GetChildrenWithType<T>(Node parent) where T : Node {
        foreach (Node node in parent.GetChildren()) {
            yield return (T)node;
        }
    }
}