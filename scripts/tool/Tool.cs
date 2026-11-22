using Godot;
using System;
using System.Security.Principal;

public abstract partial class Tool : Node3D
{
	[Export] public String Description { get; set; }
	[Export] public NodePath RoundStatePath { get; set; }

	protected RoundState _roundState;
	protected RoundPlayerState _playerState;

	protected ReactiveBool _isPlayerTurn;

	protected ReactiveArray<Unit> _units;
	protected ReactiveObject<Unit> _selectedUnit;

	protected ReactiveArray<Tool> _tools;
	protected ReactiveObject<Tool> _selectedTool;
	protected ReactiveLock<Tool> _activeTool;

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

		_playerState.ReactiveChanged += HandleProcessingState;
	}

	public abstract StringName GetTitle();
	public abstract String GetDescription();

	public abstract bool CanUse(Unit unit);

	protected abstract void EnableIdle();
	protected abstract void DisableIdle();
	protected abstract (GodotObject, StringName) BeginActive();
	protected abstract void CompleteActive();

	public void HandleProcessingState(Reactive r)
	{
		bool notPlayerTurn = !_isPlayerTurn.Value;
		bool notSelectedTool = _selectedTool.Value != this;
		bool activeToolExists = _activeTool.Value is not null;

		if (notPlayerTurn || notSelectedTool || activeToolExists)
		{
			EnableIdle();
		}

		else
		{
			DisableIdle();
		}
	}

	public void HandleActiveStart(Reactive r)
	{
		if (_activeTool.Value == this)
		{
			var continuation = BeginActive();
			var (obj, signalName) = continuation;
			if (continuation != NoContinuation)
			{
				obj.Connect(signalName, Callable.From(OnActiveCompleted), (uint)GodotObject.ConnectFlags.OneShot);
			}
			else
			{
				_activeTool.TryUnlock(this);
			}
		}
	}

	public void OnActiveCompleted()
	{
		CompleteActive();
		_activeTool.TryUnlock(this);
	}
}
