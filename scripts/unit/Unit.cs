using Godot;
using System;

public partial class Unit : Node3D
{
	public StringName GroupName = "unit";
	[Export] public NodePath RoundStatePath { get; set; }
	[Export] public Godot.Collections.Array<NodePath> ToolPaths { get; set; } = [];
	[Export] public UnitPathMover PathMover { get; set; }
	public Godot.Collections.Array<Tool> Tools { get; private set; }

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
		Vector2I pos = new Vector2I((int) Position.X, (int) Position.Z);
		GD.Print(pos, mapState.IsInBounds(pos));
		GD.Print(mapState.RegisterEntity(this, pos));

		Tools = [];
		foreach (NodePath path in ToolPaths)
		{
			Tools.Add(GetNode<Tool>(path));
		}

		_roundState.PlayerState.Units.Append(this);
		_roundState.PlayerState.SelectedUnit.Value = this;
	}

	public override void _ExitTree()
	{
		_roundState.PlayerState.Units.Remove(this);
		if (_roundState.PlayerState.SelectedUnit.Value == this)
		{
			_roundState.PlayerState.SelectedUnit.Value = null;
		}
		
		RoundMapState mapState = _roundState.MapState;
		mapState.UnregisterEntity(this);
	}
}
