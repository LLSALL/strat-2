using Godot;
using System;
using System.Data;

// TODO MAKE THIS SKIPPABLE
public partial class UnitPathMover : Node
{

	[Export] Node3D Transform;
	[Export] public float MoveSpeed { get => _moveSpeed; set { _moveSpeed = float.Max(0, value); } }
	public bool IsActive {get; private set;} = false;
	[Signal] public delegate void PathFinishedEventHandler();

	private const float EPSIL = 0.01f;
	private Godot.Collections.Array<Vector2I> _path;
	private int _pathIdx = 0;
	private float _moveSpeed;

	public override void _Ready()
	{
		SetPhysicsProcess(false);
	}

	public override void _PhysicsProcess(double delta)
	{
		// Translate the current waypoint to 3D space
		Vector2I waypoint = _path[_pathIdx];
		Vector3 waypoint3D = new Vector3(waypoint.X, Transform.Position.Y, waypoint.Y);

		// Move to waypoint
		Transform.Position = Transform.Position.MoveToward(waypoint3D, (float) delta * _moveSpeed);

		// Get next waypoint if reached after moving
		if (Transform.Position.DistanceTo(waypoint3D) < EPSIL)
		{
			_pathIdx++;
			if (_pathIdx == _path.Count)
			{
				// Path completed
				_path = null;
				_pathIdx = 0;
				SetPhysicsProcess(false);
				IsActive = false;
				EmitSignal(SignalName.PathFinished);
				return;
			}
		}
	}

	public void BeginPath(Godot.Collections.Array<Vector2I> path)
	{
		_path = path;
		_pathIdx = 0;
		IsActive = true;
		SetPhysicsProcess(true);
		return;
	}
}
