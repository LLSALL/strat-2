using Godot;
using System;

public partial class Unit : Node3D
{
	[Export] public NodePath RoundStatePath { get; set; }
	[Export] public Godot.Collections.Array<NodePath> ToolPaths { get; set; } = [];
	[Export] public UnitPathMover PathMover { get; set; }
	public Godot.Collections.Array<Tool> Tools { get; private set; }

	private RoundState _roundState;
	private RoundPlayerState _playerState;
	private RoundMapState _mapState;

	private ReactiveBool _isPlayerTurn;
	private ReactiveArray<Unit> _units;
	private ReactiveObject<Unit> _selectedUnit;
	private ReactiveLock<Tool> _activeTool;

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);

		_playerState = _roundState.PlayerState;
		_mapState = _roundState.MapState;

		_units = _playerState.Units;
		_selectedUnit = _playerState.SelectedUnit;

		Vector2I pos = new Vector2I((int) Position.X, (int) Position.Z);
		_mapState.RegisterEntity(this, pos);

		Tools = [];
		foreach (NodePath path in ToolPaths)
		{
			Tools.Add(GetNode<Tool>(path));
		}

		_playerState.RegisterUnit(this);
		_playerState.SetSelectedUnit(this);
	}

	public override void _ExitTree()
	{
		_units.Remove(this);
		if (_selectedUnit.Value == this)
		{
			_selectedUnit.Value = null;
		}
		
		_mapState.UnregisterEntity(this);
	}
	public bool CanAct()
	{
		bool canAct = false;
		foreach (Tool tool in Tools)
		{
			if (tool.CanAct(this))
			{
				canAct = true;
				break;
			}
		}

		return canAct;
	}
}
