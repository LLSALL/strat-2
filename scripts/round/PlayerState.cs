using Godot;
using System;

public partial class RoundPlayerState : Resource
{
	public ReactiveBool IsPlayerTurn = new(true, null);
	public ReactiveArray<Unit> Units = new([]);
	public ReactiveObject<Unit> SelectedUnit = new(null);

	public ReactiveArray<Tool> Tools = new(null);
	public ReactiveObject<Tool> SelectedTool = new(null);
	public ReactiveLock<Tool> ActiveTool = new(null);
}
