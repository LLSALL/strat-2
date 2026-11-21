using Godot;
using System;

public partial class RoundPlayerState : Reactive
{
	public ReactiveBool IsPlayerTurn;
	public ReactiveArray<Unit> Units;
	public ReactiveObject<Unit> SelectedUnit;

	public ReactiveArray<Tool> Tools;
	public ReactiveObject<Tool> SelectedTool;
	public ReactiveLock<Tool> ActiveTool;

	public RoundPlayerState()
	{
		IsPlayerTurn = new(true, this);

		Units = new([]);
		SelectedUnit = new(null, this);

		Tools = new([], this);
		SelectedTool = new(null, this);
		ActiveTool = new(this);
	}
}
