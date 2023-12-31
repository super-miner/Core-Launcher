using System.Threading.Tasks;
using CoreLauncher.Scripts.ModIO;
using CoreLauncher.Scripts.StoredData;
using CoreLauncher.Scripts.StoredData.StoredDataGroups;
using CoreLauncher.Scripts.Systems;
using CoreLauncher.Scripts.UI.Generic;
using Godot;

namespace CoreLauncher.Scripts.Menus.Loading; 

public partial class LoadingManager : Control {
	[Export] public LoadingBar ProgressBar;
	
	[Export] private int _totalSteps;
	[Export] private int _startDelay = 500;
	[Export] private int _nextPageDelay = 500;
	private int _stepsComplete;
	
	public override void _EnterTree() {
		InstanceManager.AddInstance(this);
		
		ModManager.ModInfoLoadedEvent += OnModInfoLoaded;
		
		if (ModManager.HasLoaded) {
			OnModInfoLoaded();
		}
		
		StoredDataManager.StoredDataDeserializedEvent += OnStoredDataDeserialized;
		
		if (StoredDataManager.HasDeserialized) {
			OnStoredDataDeserialized();
		}
	}

	public override void _Ready() {
		if (SetupManager.SetupComplete) {
			ModManager.FetchModsList();
		}
	}

	public override void _ExitTree() {
		ModManager.ModInfoLoadedEvent -= OnModInfoLoaded;
		
		StoredDataManager.StoredDataDeserializedEvent -= OnStoredDataDeserialized;
		
		InstanceManager.RemoveInstance(this);
	}

	public void StepComplete() {
		SetStepsComplete(_stepsComplete + 1);
	}

	public async void SetStepsComplete(int amount) {
		GD.Print($"Loading Manager: Completed {amount} steps.");
		
		_stepsComplete = amount;

		if (_stepsComplete >= _totalSteps) {
			await Task.Delay(_nextPageDelay);
			
			CallDeferred("Finish");
		}
	}

	public void Finish() {
		InstanceManager.GetInstance<MenuManager>().SetActiveMenu(2);
	}

	private void OnModInfoLoaded() {
		GD.Print("Loading Manager: Fetched mod data.");
		
		ProgressBar.SetValue("Mods", 1.0, "Retrieved info from Mod.io...");
		
		StepComplete();
	}

	private void OnStoredDataDeserialized() {
		GD.Print("Loading Manager: Deserialized app data.");
		
		ProgressBar.SetValue("AppData", 1.0, "Loaded configs...");
		
		if (SetupManager.SetupComplete) {
			StepComplete();
		}
		else {
			InstanceManager.GetInstance<MenuManager>().SetActiveMenu(1);
		}
	}
}
