using Godot;
using System;
using Microsoft.Win32;

namespace CoreLauncher.Scripts.Systems;

public static class RegistryManager {
	public static object GetValue(string keyPath, string value) {
		RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath); // TODO: Implement to Linux
		
		return key?.GetValue(value);
	}
}
