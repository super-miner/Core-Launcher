using Godot;
using CoreLauncher.Scripts.Systems;

namespace CoreLauncher.Scripts.UI.Buttons;

public partial class PlayButton : Button {
	private void OnPressed() {
		GameManager.RunGame();
	}
}
