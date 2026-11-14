using Godot;
using System;
using System.Collections.Generic;

public partial class MapState : Resource
{
    [Export] public uint SizeX {get; set;}
    [Export] public uint SizeY {get; set;}
    [Signal] public delegate void MapChangedEventHandler(MapState mapState, Node entity);

    private AStarGrid2D _grid;
    private Dictionary<Node, Vector2I> _coordinateMap;

    public MapState()
    {
        _grid = new();
        _coordinateMap = new();

		_grid.Region = new Rect2I(0, 0, (int) SizeX, (int) SizeY);
		_grid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		_grid.JumpingEnabled = true;
		_grid.Update();
    }
 
	public bool RegisterEntity(Node entity, Vector2I point)
	{
		if (!IsVacant(point)) return false;

		_coordinateMap.Add(entity, point);
		EmitSignal(SignalName.MapChanged, this, entity);
		return true;
	}

	public bool UnregisterEntity(Node entity)
	{
		if (!_coordinateMap.ContainsKey(entity)) return false;
		_grid.SetPointSolid(_coordinateMap[entity], false);
		EmitSignal(SignalName.MapChanged, this, entity);
		return _coordinateMap.Remove(entity);
	}

	public bool IsRegistered(Node entity)
	{
		return _coordinateMap.ContainsKey(entity);   
	}

	public bool IsVacant(Vector2I point)
	{
		if (!IsInBounds(point)) return false;
		return !_grid.IsPointSolid(point);
	}

	public bool IsInBounds(Vector2I point)
	{
		return (point.X >= 0 && point.X < SizeX && point.Y >= 0 && point.Y < SizeY);
	}

	public Vector2I GetCoordinates(Node entity)
	{
		if (!_coordinateMap.ContainsKey(entity)) return new Vector2I(-1, -1);

		return _coordinateMap[entity];
	}
	public bool SetCoordinates(Node entity, Vector2I point)
	{
		if (!IsInBounds(point) || !IsVacant(point) || !_coordinateMap.ContainsKey(entity)) return false;

		_coordinateMap[entity] = point;
		EmitSignal(SignalName.MapChanged, this, entity);
		return true;
	}

	public Godot.Collections.Array<Vector2I> PlotPath(Vector2I start, Vector2I end)
	{
		return _grid.GetIdPath(start, end, false);
	}

}
