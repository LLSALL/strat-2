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
	private RoundMapState _mapState;

	public override StringName GetTitle()
	{
		return "Move";
	}

    public override string GetDescription()
    {
        return "Move Desc";
    }

	public override void _Ready()
	{
		base._Ready();
		_mapState = _roundState.MapState;
		_floorArea.InputEvent += OnFloorChange;
	}

	public override bool CanUse(Unit unit)
	{
		if (unit is null)
		{

		}

		return true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("start")) return;
		_activeTool.TryLock(this);
								   // TODO prevent this from running without a valid path
	}

	protected override void EnableIdle()
	{
		SetProcessInput(true);
		_processFloor = true; // TODO Change?
	}

	protected override void DisableIdle()
	{
		SetProcessInput(false);
		_processFloor = false; // TODO Change?

	}

	protected override (GodotObject, StringName) BeginActive()
	{
		_activeUnit = _selectedUnit.Value;
		_endCoordinates = _cursorCoordinates;

		GD.Print($"{_mapState.GetCoordinates(_activeUnit)} => {_endCoordinates}");

		var path = _mapState.PlotPath(
			_mapState.GetCoordinates(_activeUnit),
			_endCoordinates
			);
		_activeUnit.PathMover.BeginPath(path);

		return (_activeUnit, "PathFinished");
	}

	protected override void CompleteActive()
	{
		_mapState.SetCoordinates(_activeUnit, _endCoordinates);
		_activeUnit = null;
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
