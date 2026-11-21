using Godot;
using System;

public abstract partial class Tool : Node3D
{
	public StringName GroupName = "tool";
	[Export] public String Description { get; set; }
	[Export] public NodePath RoundStatePath { get; set; }

	protected RoundState _roundState;

	protected ReactiveObject<Unit> _selectedUnit;
	protected ReactiveObject<Tool> _selectedTool;
	protected ReactiveLock<Tool> _activeTool;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);

		_selectedUnit = _roundState.PlayerState.SelectedUnit;
		_selectedTool = _roundState.PlayerState.SelectedTool;
		_activeTool = _roundState.PlayerState.ActiveTool;

		_selectedUnit.ReactiveChanged += OnUnitChange;
		_selectedTool.ReactiveChanged += OnSelectedChange;
		_activeTool.ReactiveChanged += OnActiveChange;
	}

	public abstract StringName GetTitle();
	public abstract bool CheckConditions(Unit unit);
	protected abstract void OnUnitChange(Reactive selectedUnit);
	protected abstract void OnSelectedChange(Reactive selectedTool);
	protected abstract void OnActiveChange(Reactive activeTool);
}
