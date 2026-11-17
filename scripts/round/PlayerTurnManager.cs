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
    }

    private void OnUnitChange(Reactive r)
    {
        _roundState.PlayerState.Tools.Value = _roundState.PlayerState.SelectedUnit.Value.Tools;
        _roundState.PlayerState.SelectedTool.Value = null;
    }
}