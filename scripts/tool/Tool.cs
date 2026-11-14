using Godot;
using System;

public abstract partial class Tool : ClassNamedNode
{
	protected RoundState _roundState;
    protected ReactiveObject<Unit> _selectedUnit;
    protected ReactiveObject<Tool> _selectedTool;
    protected ReactiveLock<Tool> _activeTool;

    public override void _Ready()
    {
        base._Ready();
        Name = this.GetType().Name;
		_roundState = GetNode<RoundState>(nameof(RoundState));
        _selectedUnit = _roundState.PlayerState.SelectedUnit;
        _selectedTool = _roundState.PlayerState.SelectedTool;
        _activeTool = _roundState.PlayerState.ActiveTool;

        _selectedTool.ReactiveChanged += OnSelectedChange;
    }

	protected abstract void OnSelectedChange(Reactive selectedTool);
	protected abstract void OnActiveChange(Reactive activeTool);
}