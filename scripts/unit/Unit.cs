using Godot;
using System;

public partial class Unit : Node3D
{
	[Export] public NodePath PlayerStatePath { get; set; }
	[Export] public NodePath EnemyStatePath { get; set; }
	[Export] public NodePath MapStatePath { get; set; }



	[Export] public Godot.Collections.Array<NodePath> ToolPaths { get; set; } = [];
	[Export] public UnitPathMover PathMover { get; set; }
	public Godot.Collections.Array<Tool> Tools { get; private set; }

	private RoundPlayerState _playerState;
	private RoundEnemyState _enemyState;
	private RoundMapState _mapState;

	private BoolReadOnlyValue _isPlayerTurn;
	private ArrayReadOnlyValue<Unit> _units;
	private ObjectReadOnlyValue<Unit> _selectedUnit;
	private LockReadOnlyValue<Tool> _activeTool;

	public override void _Ready()
	{
		_playerState = GetNode<RoundPlayerState>(PlayerStatePath);
		_enemyState = GetNode<RoundEnemyState>(EnemyStatePath);
		_mapState = GetNode<RoundMapState>(MapStatePath);

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
	}

	public override void _ExitTree()
	{
		_playerState.UnregisterUnit(this);
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
