using Godot;
using System;
using System.Collections.Generic;

public partial class UIUnitList : Node
{
	public StringName GroupName = "ui";
	[Export] public NodePath RoundStatePath { get; set; }
	[Export] public CanvasItem Panel {get; set;}
	[Export] public BoxContainer ListContainer { get; set; }

	private RoundState _roundState;

	private ReactiveArray<Unit> _units;

	private ReactiveBool _isPlayerTurn;

	private ReactiveObject<Unit> _selectedUnit;
	private ReactiveLock<Tool> _activeTool;

	private Dictionary<Unit, Button> _unitButtonMap;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);

		_units = _roundState.PlayerState.Units;
		_selectedUnit = _roundState.PlayerState.SelectedUnit;
		_activeTool = _roundState.PlayerState.ActiveTool;
		_isPlayerTurn = _roundState.PlayerState.IsPlayerTurn;

		_isPlayerTurn.ReactiveChanged += OnActiveChange;
		_units.ReactiveChanged += OnUnitsChange;
		_selectedUnit.ReactiveChanged += OnSelectedChange;
		_activeTool.ReactiveChanged += OnActiveChange;

		_unitButtonMap = new();
	}

	private void OnUnitsChange(Reactive r)
	{
		ClearList();
		BuildList();
		UpdateButtonStates();
	}

	private void OnSelectedChange(Reactive r)
	{
		UpdateButtonStates();
	}

	private void OnActiveChange(Reactive r)
	{
		UpdateButtonStates();
	}

	private void ClearList()
	{
		_unitButtonMap = null;
		foreach (Node node in ListContainer.GetChildren())
		{
			ListContainer.RemoveChild(node);
			node.QueueFree();
		}
	}

	private void BuildList()
	{
		_unitButtonMap = new();
		foreach (Unit unit in _units.Value)
		{
			Button button = new();
			button.Text = unit.Name;
			button.Pressed += () => { _selectedUnit.Value = unit; };
			ListContainer.AddChild(button);
			_unitButtonMap.Add(unit, button);
		}
	}

	private void UpdateButtonStates()
	{
		if (_isPlayerTurn.Value == false)
		{
			Panel.Hide();
		}
		else
		{
			Panel.Show();
		}

		foreach ((Unit unit, Button button) in _unitButtonMap)
		{
			if (_activeTool.Value != null)
			{
				button.Show();
				button.Disabled = true;   
			}
			else if (_selectedUnit.Value == unit)
			{
				button.Show();
				button.Disabled = true;
			}
			else
			{
				button.Show();
				button.Disabled = false;
			}
		}
	}
}
