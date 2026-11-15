using Godot;
using System;

public partial class RoundManager : Node
{
    public StringName GroupName = "round_manager";
	private RoundState _roundState;

    public override void _EnterTree()
    {
        AddToGroup(GroupName);
    }
}
