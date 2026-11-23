using Godot;
using System;

public partial class RoundPlayerState : Value
{

	public BoolReadOnlyValue IsPlayerTurn;
	public ArrayReadOnlyValue<Unit> Units;
	public ObjectReadOnlyValue<Unit> SelectedUnit;
	public ArrayReadOnlyValue<Tool> Tools;
	public ObjectReadOnlyValue<Tool> SelectedTool;
	public LockReadOnlyValue<Tool> ActiveTool;

	private BoolValue _isPlayerTurn;

	private ArrayValue<Unit> _units;
	private ObjectValue<Unit> _selectedUnit;

	private ArrayValue<Tool> _tools;
	private ObjectValue<Tool> _selectedTool;
	private LockValue<Tool> _activeTool;

	public RoundPlayerState()
	{
		_isPlayerTurn = new(true, this);

		_units = new([]);
		_selectedUnit = new(null, this);

		_tools = new([], this);
		_selectedTool = new(null, this);
		_activeTool = new(this);
	}

	public void BeginPlayerTurn()
	{
		SelectFirstAvailableUnit();
		_isPlayerTurn.Value = true;
	}

	public void EndPlayerTurn()
	{
		_isPlayerTurn.Value = false;
	}

	public void RegisterUnit(Unit unit)
	{
		_units.Append(unit);	
	}

	public void UnregisterUnit(Unit unit)
	{
		_units.Remove(unit);	
		if (_selectedUnit.Value == unit)
		{
			SelectFirstAvailableUnit();
		}
	}

	public void SetSelectedUnit(Unit unit)
	{
		_selectedUnit.Value = unit;

		if (unit is not null)
		{
			_tools.Value = unit.Tools;
		}
		else
		{
			_tools.Clear();
		}

		SelectFirstAvailableTool();
	}
	
	public bool CanSelectUnit(Unit unit)
	{
		bool isPlayerTurn = _isPlayerTurn.Value;
		bool notSelectedUnit = _selectedUnit.Value != unit;
		bool noActiveToolExists = _activeTool.Value is null;
		bool canAct = unit.CanAct();

		return isPlayerTurn && notSelectedUnit && noActiveToolExists && canAct;
	}

	public void SelectFirstAvailableUnit()
	{
		Unit selectedUnit = null;
		foreach (Unit unit in _units.Value)
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
		_selectedTool.Value = tool;
	}

	public bool CanSelectTool(Tool tool)
	{
		bool isPlayerTurn = _isPlayerTurn.Value;
		bool notSelectedTool = _selectedTool.Value != tool;
		bool noActiveToolExists = _activeTool.Value is null;
		bool canAct = tool.CanAct(_selectedUnit.Value);

		return isPlayerTurn && notSelectedTool && noActiveToolExists && canAct;
	}

	public void SelectFirstAvailableTool()
	{
		Tool selectedTool = null;
		foreach (Tool tool in _tools.Value)
		{
			if (CanSelectTool(tool))
			{
				selectedTool = tool;
				break;
			}
		}	

		_selectedTool.Value = selectedTool;
	}

	public bool LockActiveTool(Tool tool)
	{
		return _activeTool.TryLock(tool);
	}

	public bool UnlockActiveTool(Tool tool)
	{
		if (!_activeTool.TryUnlock(tool)) return false;

		if (!CanSelectTool(_selectedTool.Value))
		{
			SelectFirstAvailableTool();
		}

		return true;
	}
}
