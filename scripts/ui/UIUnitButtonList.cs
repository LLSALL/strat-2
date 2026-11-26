using Godot;
using System;

public partial class UIUnitButtonList : Control
{
	[Export] public NodePath PlayerStatePath { get; set; }

	private RoundPlayerState _playerState;
	private ObjectReadOnlyValue<Unit> _selectedUnit;
	private ArrayReadOnlyValue<Unit> _units;
	private LockReadOnlyValue<Tool> _activeTool;

	public override void _Ready()
	{
		_playerState = GetNode<RoundPlayerState>(PlayerStatePath);
		_units = _playerState.Units;
		_selectedUnit = _playerState.SelectedUnit;
		_activeTool = _playerState.ActiveTool;

		_units.ReactiveChanged += CreateButtons;
		_selectedUnit.ReactiveChanged += CreateButtons;
		_activeTool.ReactiveChanged += CreateButtons;

		CreateButtons(null);
	}

	public bool CheckUnitUIState(Unit unit)
	{
		bool noActiveToolExists = _activeTool.Value is null;
		bool canAct = unit.CanAct();
		bool isNotSelectedUnit = _selectedUnit.Value != unit;

		return noActiveToolExists && canAct && isNotSelectedUnit;
	}

	private void CreateButtons(Value r) // TODO: Avoid rebuilding the list on every update 
	{
		foreach (var child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}

		foreach (var unit in _units.Value)
		{
			Button button = new();
			button.Text = unit.Name;
			button.Pressed += () => { _playerState.SetSelectedUnit(unit); };

			if (CheckUnitUIState(unit))
			{
				button.Disabled = false;
			} else
			{
				button.Disabled = true;
			}

			AddChild(button);
		}
	}
}
