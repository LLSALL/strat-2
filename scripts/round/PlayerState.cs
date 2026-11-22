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

	public void RegisterUnit(Unit unit)
	{
		Units.Append(unit);	
	}

	public void UnregisterUnit(Unit unit)
	{
		Units.Remove(unit);	
	}
	
	public void SelectFirstAvailableUnit()
	{
		Unit selectedUnit = null;
		foreach (Unit unit in Units.Value)
		{
			if (unit.CanAct())
			{
				selectedUnit = unit;
				break;
			}
		}	

		SelectedUnit.Value = selectedUnit;
	}

	public void SelectFirstAvailableTool()
	{
		Tool selectedTool = null;
		foreach (Tool tool in Tools.Value)
		{
			if (tool.CanUse(SelectedUnit.Value))
			{
				selectedTool = tool;
				break;
			}
		}	

		SelectedTool.Value = selectedTool;
	}
}
