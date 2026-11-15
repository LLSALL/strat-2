using Godot;
using System;

public abstract partial class Tool : Node
{
    public StringName GroupName = "tool";
    [Export] public NodePath RoundStatePath {get; set;}

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

    protected abstract void OnUnitChange (Reactive selectedUnit);
	protected abstract void OnSelectedChange(Reactive selectedTool);
	protected abstract void OnActiveChange(Reactive activeTool);
}
