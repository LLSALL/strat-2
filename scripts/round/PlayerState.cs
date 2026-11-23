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

	public void BeginPlayerTurn()
	{
		SelectFirstAvailableUnit();
		IsPlayerTurn.Value = true;
	}

	public void EndPlayerTurn()
	{
		IsPlayerTurn.Value = false;
	}

	public void RegisterUnit(Unit unit)
	{
		Units.Append(unit);	
	}

	public void UnregisterUnit(Unit unit)
	{
		Units.Remove(unit);	
		if (SelectedUnit.Value == unit)
		{
			SelectFirstAvailableUnit();
		}
	}

	public void SetSelectedUnit(Unit unit)
	{
		SelectedUnit.Value = unit;

		if (unit is not null)
		{
			Tools.Value = unit.Tools;
		}
		else
		{
			Tools.Clear();
		}

		SelectFirstAvailableTool();
	}
	
	public bool CanSelectUnit(Unit unit)
	{
		bool isPlayerTurn = IsPlayerTurn.Value;
		bool notSelectedUnit = SelectedUnit.Value != unit;
		bool noActiveToolExists = ActiveTool.Value is null;
		bool canAct = unit.CanAct();

		return isPlayerTurn && notSelectedUnit && noActiveToolExists && canAct;
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

		SetSelectedUnit(selectedUnit);
	}

	public void SetSelectedTool(Tool tool)
	{
		SelectedTool.Value = tool;
	}

	public bool CanSelectTool(Tool tool)
	{
		bool isPlayerTurn = IsPlayerTurn.Value;
		bool notSelectedTool = SelectedTool.Value != tool;
		bool noActiveToolExists = ActiveTool.Value is null;
		bool canAct = tool.CanAct(SelectedUnit.Value);

		return isPlayerTurn && notSelectedTool && noActiveToolExists && canAct;
	}

	public void SelectFirstAvailableTool()
	{
		Tool selectedTool = null;
		foreach (Tool tool in Tools.Value)
		{
			if (CanSelectTool(tool))
			{
				selectedTool = tool;
				break;
			}
		}	

		SelectedTool.Value = selectedTool;
	}

	public bool SetActiveTool(Tool tool)
	{
		return ActiveTool.TryLock(tool);
	}

	public bool UnsetActiveTool(Tool tool)
	{
		if (!ActiveTool.TryUnlock(tool)) return false;

		if (!CanSelectTool(SelectedTool.Value))
		{
			SelectFirstAvailableTool();
		}

		return true;
	}
}
