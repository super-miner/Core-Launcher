using Godot;
using System;
using Microsoft.Win32;

namespace CoreLauncher.Scripts;

public static class RegistryManager {
	public static object GetValue(string keyPath, string value) {
		RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath);
		
		return key?.GetValue(value);
	}
}
