using Godot;
using System;
using Microsoft.Win32;

namespace CoreLauncher.Scripts.Systems;

public static class RegistryUtil {
    public static object GetValue(string keyPath, string value) {
        try {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath); // TODO: Implement to Linux

            return key?.GetValue(value);
        }
        catch (Exception err) {
            GD.PrintErr("Registry: Error reading from the registry, user might not be using Windows.");
        }

        return null;
    }
}