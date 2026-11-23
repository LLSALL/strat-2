using Godot;
using System;
using System.Security.Principal;

public abstract partial class Tool : Node3D
{
	[Export] public String Description { get; set; }
	[Export] public NodePath RoundStatePath { get; set; }

	protected RoundState _roundState;
	protected RoundPlayerState _playerState;

	protected BoolReadOnlyValue _isPlayerTurn;

	protected ArrayReadOnlyValue<Unit> _units;
	protected ObjectReadOnlyValue<Unit> _selectedUnit;

	protected ArrayReadOnlyValue<Tool> _tools;
	protected ObjectReadOnlyValue<Tool> _selectedTool;
	protected LockReadOnlyValue<Tool> _activeTool;

	protected static (GodotObject, StringName) NoContinuation = (null, null);

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);
		_playerState = _roundState.PlayerState;

		_isPlayerTurn = _playerState.IsPlayerTurn;

		_units = _playerState.Units;
		_selectedUnit = _playerState.SelectedUnit;

		_tools = _roundState.PlayerState.Tools;
		_selectedTool = _roundState.PlayerState.SelectedTool;
		_activeTool = _roundState.PlayerState.ActiveTool;

		_playerState.ReactiveChanged += HandleIdleState;
		_playerState.ReactiveChanged += HandleActiveStart;
	}

	public abstract StringName GetTitle();
	public abstract String GetDescription();


	public abstract bool CanAct(Unit unit);

	protected abstract void EnableIdle();
	protected abstract void DisableIdle();
	protected abstract (GodotObject, StringName) BeginActive();
	protected abstract void CompleteActive();

	public void HandleIdleState(Value r)
	{
		bool isPlayerTurn = !_isPlayerTurn.Value;
		bool isSelectedTool = _selectedTool.Value != this;
		bool activeToolDNE = _activeTool.Value is not null;

		if (isPlayerTurn && isSelectedTool && activeToolDNE)
		{
			EnableIdle();
		}

		else
		{
			DisableIdle();
		}
	}

	public void HandleActiveStart(Value r)
	{
		if (_activeTool.Value != this) return;

		var continuation = BeginActive();
		var (obj, signalName) = continuation;
		if (continuation != NoContinuation)
		{
			obj.Connect(signalName, Callable.From(OnActiveCompleted), (uint)GodotObject.ConnectFlags.OneShot);
		}
		else
		{
			_playerState.LockActiveTool(this);
		}
	}

	public void OnActiveCompleted()
	{
		CompleteActive();
		_playerState.UnlockActiveTool(this);
	}
}
