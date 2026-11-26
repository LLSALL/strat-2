using Godot;
using System;

public partial class UICancelActionButton : Button
{

	[Export] public NodePath PlayerStatePath { get; set; }

	private RoundPlayerState _playerState;
	private LockReadOnlyValue<Tool> _activeTool;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerState = GetNode<RoundPlayerState>(PlayerStatePath);
		_activeTool = _playerState.ActiveTool;

		Pressed += OnButtonPress;
		_activeTool.ReactiveChanged += OnActiveChange;

		OnActiveChange(null);
	}

	private void OnButtonPress()
	{
		var activeTool = _playerState.ActiveTool.Value;
		if (activeTool is null) return;
		activeTool.SkipActive();
	}

	private void OnActiveChange(Value r)
	{
		if (_activeTool.Value is not null)
		{
			Disabled = false;
		}
		else
		{
			Disabled = true;
		}
	}
}
