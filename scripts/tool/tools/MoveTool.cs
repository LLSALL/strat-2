using Godot;
using System;

public partial class MoveTool : Tool
{
	private Unit _activeUnit;
	[Export] public Area3D _floorArea { get; set; }
	private Vector2I _cursorCoordinates;
	private Vector2I _endCoordinates;
	private bool _cursorInFloor = false;
	private bool _processFloor = false;

	public override void _Ready()
	{
		base._Ready();
		_floorArea.InputEvent += OnFloorChange;
	}

	public override bool CanAct(Unit unit)
	{
		if (unit is null)
		{
			return false;
		}

		return true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("start")) return;

		var path = _mapState.PlotPath(
			_mapState.GetCoordinates(_selectedUnit.Value),
			_cursorCoordinates
			);
		if (path.Count == 0) return;

		_playerState.LockActiveTool(this);
	}

	protected override void EnableIdle()
	{
		SetProcessInput(true);
		_processFloor = true;
	}

	protected override void DisableIdle()
	{
		SetProcessInput(false);
		if (_processFloor)
		_processFloor = false;

	}

	protected override (GodotObject, StringName) BeginAction()
	{
		_activeUnit = _selectedUnit.Value;
		_endCoordinates = _cursorCoordinates;

		var path = _mapState.PlotPath(
			_mapState.GetCoordinates(_activeUnit),
			_endCoordinates
			);
		_activeUnit.PathMover.BeginPath(path);

		return (_activeUnit.PathMover, "PathFinished");
	}

	protected override void CompleteAction()
	{
		_mapState.SetCoordinates(_activeUnit, _endCoordinates);
		_activeUnit = null;
	}

	protected override void CancelAction()
	{
		_activeUnit.PathMover.SkipPath();
	}

	private void OnFloorChange(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, long shapeIdx)
	{
		if (!_processFloor) return;

		var position2I = new Vector2I((int)position.X, (int)position.Z);
		if (!_mapState.IsInBounds(position2I))
		{
			_cursorInFloor = false;
			return;
		}
		_cursorInFloor = true;
		_cursorCoordinates = position2I;
	}
}
