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
}
