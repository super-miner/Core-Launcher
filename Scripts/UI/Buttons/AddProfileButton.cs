using Godot;

public partial class AddProfileButton : Button {
	[Export] private ItemList itemList = null;
	
	public void OnPressed() {
		if (itemList != null) {
			itemList.AddEntry();
		}
		else {
			GD.Print("Could not add profile because the itemList was not found.");
		}
	}
}
