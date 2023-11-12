using System;
using System.Collections.Generic;
using Godot;

namespace CoreLauncher.Scripts.Systems; 

public static class InstanceManager {
    private static Dictionary<Type, Node> _instances = new Dictionary<Type, Node>();

    public static void AddInstance<T>(T instance) where T : Node {
        Node currentInstance = GetInstance(typeof(T));
        if (currentInstance != null) {
            currentInstance.QueueFree();
        }
        
        _instances.Add(typeof(T), instance);
    }

    public static void RemoveInstance<T>(T instance) where T : Node {
        RemoveInstance(typeof(T), instance);
    }

    public static void RemoveInstance(Type type, Node instance) {
        Node currentInstance = GetInstance(type);
        if (instance.Equals(currentInstance)) {
            _instances.Remove(type);
        }
    }

    public static T GetInstance<T>() where T : Node {
        return (T)GetInstance(typeof(T));
    }
    
    public static Node GetInstance(Type type) {
        return _instances.TryGetValue(type, out Node value) ? value : null;
    }
}