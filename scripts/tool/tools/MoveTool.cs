using Godot;
using System;

public partial class MoveTool : Tool
{
	private Unit _unit;
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

	public override void _Ready()
	{
		base._Ready();
		_mapState = _roundState.MapState;
		_floorArea.InputEvent += OnFloorChange;
	}

	public override bool CheckConditions()
	{
		return true;
	}

	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("start")) return;
		_activeTool.TryLock(this); // Move start action code here?
								   // TODO prevent this from running without a valid path
	}

	protected override void OnUnitChange(Reactive selectedUnit)
	{
		_unit = _selectedUnit.Value;
	}

	protected override void OnSelectedChange(Reactive selectedTool)
	{
		if (_selectedTool.Value == this)
		{
			SetProcessInput(true);
			_processFloor = true;
		}
		else
		{
			SetProcessInput(false);
			_processFloor = false;
		}
	}

	protected override void OnActiveChange(Reactive activeTool)
	{
		if (_activeTool.Value == this)
		{
			_endCoordinates = _cursorCoordinates;
			GD.Print($"{_mapState.GetCoordinates(_unit)} => {_endCoordinates}");
			var path = _mapState.PlotPath(
				_mapState.GetCoordinates(_unit),
				_endCoordinates
				);
			_unit.PathMover.BeginPath(path);
			_unit.PathMover.Connect("PathFinished", Callable.From(ActionCompleted), (uint)GodotObject.ConnectFlags.OneShot);
		}
		else
		{

		}
	}

	private void ActionCompleted()
	{
		_mapState.SetCoordinates(_unit, _endCoordinates);
		_activeTool.TryUnlock(this);
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
