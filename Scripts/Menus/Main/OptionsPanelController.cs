using Godot;

namespace CoreLauncher.Scripts.Menus.Main;

public partial class OptionsPanelController : MarginContainer
{
	[Export] private TabContainer _optionsTabContainer;
	[Export] private PanelContainer _optionsPanelContainer;

	public void TogglePanel()
	{
		if (_optionsTabContainer.Visible)
		{
			_optionsTabContainer.Visible = false;
		}
		_optionsPanelContainer.Visible = !_optionsPanelContainer.Visible;
	}
}