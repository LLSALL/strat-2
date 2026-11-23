using Godot;
using System;

public partial class RoundManager : Node
{
	public StringName GroupName = "round_manager";
	[Export] public NodePath RoundStatePath { get; set; }

	private RoundState _roundState;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);
	}

	public void EndPlayerTurn()
	{
		_roundState.PlayerState.EndPlayerTurn();
		_roundState.EnemyState.IsEnemyTurn.Value = true;
	}

	private void EndEnemyTurn()
	{
		_roundState.EnemyState.IsEnemyTurn.Value = false;
		_roundState.PlayerState.BeginPlayerTurn();
	}
}
