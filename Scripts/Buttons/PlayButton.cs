using Godot;
using System;

public partial class PlayButton : Button
{
	private void OnPressed() {
		GD.Print(SteamManager.FindSteamPath());
	}
}
