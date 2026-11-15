using Godot;
using System;

public partial class RoundState : Node
{
    public StringName GroupName = "round_state";
	public RoundPlayerState PlayerState = new();
	public RoundEnemyState EnemyState = new();
	public RoundMapState MapState = new();

    public override void _EnterTree()
    {
        AddToGroup(GroupName);
    }
}