using CoreLauncher.Scripts.Profiles;
using CoreLauncher.Scripts.StoredData;
using Godot;

namespace CoreLauncher.Scripts.UI.Buttons; 

public partial class OnlyCopyModsCheckBox : CheckBox {
    public override void _EnterTree() {
        StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
        
        if (StoredDataManager.HasDeserialized) {
            OnStoredDataDeserialized();
        }
    }

    public override void _ExitTree() {
        StoredDataManager.StoredDataDeserializedEvent -= OnStoredDataDeserialized;
    }

    public void OnPressed() {
        ProfileManager.OnlyCopyMods = ButtonPressed;
    }

    private void OnStoredDataDeserialized() {
        ButtonPressed = ProfileManager.OnlyCopyMods;
    }
}