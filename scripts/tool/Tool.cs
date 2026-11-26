using Godot;
using System;
using System.Security.Principal;

public abstract partial class Tool : Node3D
{
	[Export] public String Title { get; set; }
	[Export] public String Description { get; set; }

	[Export] public NodePath PlayerStatePath { get; set; }
	[Export] public NodePath EnemyStatePath { get; set; }
	[Export] public NodePath MapStatePath { get; set; }

	protected RoundPlayerState _playerState;
	protected RoundEnemyState _enemyState;
	protected RoundMapState _mapState;

	protected BoolReadOnlyValue _isPlayerTurn;

	protected ArrayReadOnlyValue<Unit> _units;
	protected ObjectReadOnlyValue<Unit> _selectedUnit;

	protected ArrayReadOnlyValue<Tool> _tools;
	protected ObjectReadOnlyValue<Tool> _selectedTool;
	protected LockReadOnlyValue<Tool> _activeTool;


	public override void _Ready()
	{
		_playerState = GetNode<RoundPlayerState>(PlayerStatePath);
		_enemyState = GetNode<RoundEnemyState>(EnemyStatePath);
		_mapState = GetNode<RoundMapState>(MapStatePath);

		_isPlayerTurn = _playerState.IsPlayerTurn;

		_units = _playerState.Units;
		_selectedUnit = _playerState.SelectedUnit;

		_tools = _playerState.ToolBox;
		_selectedTool = _playerState.SelectedTool;
		_activeTool = _playerState.ActiveTool;

		_playerState.Value.ReactiveChanged += HandleIdleState;
		_playerState.Value.ReactiveChanged += HandleActiveStart;
	}

	public abstract bool CanAct(Unit unit);

	protected abstract void EnableIdle();
	protected abstract void DisableIdle();
	protected abstract (GodotObject, StringName) BeginAction();
	protected abstract void CompleteAction();
	protected abstract void CancelAction();

	private void HandleIdleState(Value r)
	{
		bool isPlayerTurn = _isPlayerTurn.Value;
		bool isSelectedTool = _selectedTool.Value == this;
		bool activeToolDNE = _activeTool.Value is null;

		if (isPlayerTurn && isSelectedTool && activeToolDNE)
		{
			EnableIdle();
		}

		else
		{
			DisableIdle();
		}
	}

	protected static (GodotObject, StringName) NoContinuation = (null, null);
	private (GodotObject, StringName) _continuation = NoContinuation;

	private void HandleActiveStart(Value r)
	{
		if (_activeTool.Value != this) return;

		var continuation = BeginAction();
		var (obj, signalName) = continuation;
		if (continuation != NoContinuation)
		{
			_continuation = continuation;
			obj.Connect(signalName, Callable.From(HandleActiveCompletion), (uint)GodotObject.ConnectFlags.OneShot);
		}
		else
		{
			_continuation = NoContinuation;
			HandleActiveCompletion();
		}
	}

	private void HandleActiveCompletion()
	{
		// Assumes that signal has already been disconnected prior to call
		_continuation = NoContinuation;

		CompleteAction();
		_playerState.UnlockActiveTool(this);
	}

	public void SkipActive()
	{
		if (_activeTool.Value != this) return;

		// Disconnect continuation signal connection if it exists
		if (_continuation != NoContinuation)
		{
			var (obj, signalName) = _continuation;
			if (obj.IsConnected(signalName, Callable.From(HandleActiveCompletion)))
			{
				obj.Disconnect(signalName, Callable.From(HandleActiveCompletion));
			}
		}
		_continuation = NoContinuation;

		CancelAction();
		HandleActiveCompletion();
	}
}
