using Godot;
using System;

public partial class PlayerTurnManager : Node
{
	public StringName GroupName = "player_turn_manager";
	[Export] public NodePath RoundStatePath { get; set; }

	private RoundState _roundState;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);
		_roundState.PlayerState.SelectedUnit.ReactiveChanged += OnUnitChange;
		_roundState.PlayerState.IsPlayerTurn.ReactiveChanged += OnTurnChange;
	}

	private void OnUnitChange(Reactive r)
	{
		// TODO Check for null value
		if (_roundState.PlayerState.SelectedUnit.Value is null)
		{
		_roundState.PlayerState.Tools.Value = null;
		_roundState.PlayerState.SelectedTool.Value = null;
		}
		else
		{
		_roundState.PlayerState.Tools.Value = _roundState.PlayerState.SelectedUnit.Value.Tools;
		_roundState.PlayerState.SelectedTool.Value = null;
		}
	}

	private void CheckConditions()
	{
		bool canAct = true;
		foreach (Unit unit in _roundState.PlayerState.Units.Value)
		{
			foreach (Tool tool in unit.Tools)
			{
				if (canAct = tool.CheckConditions(unit) == true) break;
			}
			if (canAct) break;
		}
		if (!canAct)
		{
			//Transition to enemy turn
		}
	}

	private void OnTurnChange(Reactive r)
	{
		_roundState.PlayerState.SelectedTool.Value = null;
		_roundState.PlayerState.SelectedUnit.Value = null;
	}
}
