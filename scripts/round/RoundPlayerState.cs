using Godot;
using System;

public partial class RoundPlayerState : Node
{
	public override string ToString()
	{
		string str = "";

		str += $"Is Player Turn: {IsPlayerTurn.Value}\n";
		str += $"Units: {Units.Value}\n";
		str += $"Selected Unit: {SelectedUnit.Value}\n";
		str += $"Toolbox: {ToolBox.Value}\n";
		str += $"Selected Tool: {SelectedTool.Value}\n";
		str += $"Active Tool: {ActiveTool.Value}";

		return str;
	}

	private static int _DbgCount = 0;
	public void DebugPrint(Value r)
	{
		GD.Print("==================");
		GD.Print($"State Log #{_DbgCount++}");
		GD.Print(ToString());
		GD.Print("==================\n");
	}

	public Value Value { get; set; }

	public BoolReadOnlyValue IsPlayerTurn { get; private set; }
	public ArrayReadOnlyValue<Unit> Units { get; private set; }
	public ObjectReadOnlyValue<Unit> SelectedUnit { get; private set; }
	public ArrayReadOnlyValue<Tool> ToolBox { get; private set; }
	public ObjectReadOnlyValue<Tool> SelectedTool { get; private set; }
	public LockReadOnlyValue<Tool> ActiveTool { get; private set; }

	private BoolValue _isPlayerTurn;

	private ArrayValue<Unit> _units;
	private ObjectValue<Unit> _selectedUnit;

	private ArrayValue<Tool> _toolBox;
	private ObjectValue<Tool> _selectedTool;
	private LockValue<Tool> _activeTool;

	public override void _EnterTree()
	{
		base._EnterTree();

		Value = new();

		_isPlayerTurn = new(true);

		_units = new([]);
		_selectedUnit = new(null);

		_toolBox = new([]);
		_selectedTool = new(null);
		_activeTool = new();

		IsPlayerTurn = new(_isPlayerTurn, Value);
		Units = new(_units, Value);
		SelectedUnit = new(_selectedUnit, Value);
		ToolBox = new(_toolBox, Value);
		SelectedTool = new(_selectedTool, Value);
		ActiveTool = new(_activeTool, Value);
	}

	public override void _Ready()
	{
		base._Ready();
		Value.ReactiveChanged += DebugPrint;
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
			_toolBox.Value = unit.Tools;
		}
		else
		{
			_toolBox.Clear();
		}

		SelectFirstAvailableTool();
	}

	public bool CanSelectUnit(Unit unit)
	{
		return unit.CanAct();
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
		return tool.CanAct(_selectedUnit.Value);
	}

	public void SelectFirstAvailableTool()
	{
		Tool selectedTool = null;
		foreach (Tool tool in _toolBox.Value)
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
