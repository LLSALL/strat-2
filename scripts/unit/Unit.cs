using Godot;
using System;

public partial class Unit : Node
{
	public StringName GroupName = "unit";
	[Export] public NodePath RoundStatePath {get; set;} 
	[Export] public Godot.Collections.Array<NodePath> ToolPaths {get; set;} = [];
	public Godot.Collections.Array<Tool> Tools {get; private set;}

	private RoundState _roundState;

    public override void _EnterTree()
    {
        base._EnterTree();
		AddToGroup(GroupName);
    }

    public override void _Ready()
    {
		_roundState = GetNode<RoundState>(RoundStatePath);

		RoundMapState mapState = _roundState.MapState;
		mapState.RegisterEntity(this, new Vector2I(0,0));

		Tools = [];
		foreach (NodePath path in ToolPaths)
		{
			Tools.Add(GetNode<Tool>(path));
		}	
    }
}