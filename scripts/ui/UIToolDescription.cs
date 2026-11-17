using Godot;
using System;

public partial class UIToolDescription : Node
{
    public StringName GroupName = "ui";
    [Export] public NodePath RoundStatePath { get; set; }
    [Export] public Panel DescriptionPanel { get; set; }
    [Export] public Label DescriptionLabel { get; set; }
    [Export] public Label UnitLabel { get; set; }

    private RoundState _roundState;

    private ReactiveObject<Tool> _selectedTool;
    private ReactiveObject<Unit> _selectedUnit;
    private ReactiveLock<Tool> _activeTool;

    public override void _EnterTree()
    {
        AddToGroup(GroupName);
    }

    public override void _Ready()
    {
        _roundState = GetNode<RoundState>(RoundStatePath);

        _selectedTool = _roundState.PlayerState.SelectedTool;
        _activeTool = _roundState.PlayerState.ActiveTool;
        _selectedUnit = _roundState.PlayerState.SelectedUnit;

        _selectedTool.ReactiveChanged += OnChange;
        _activeTool.ReactiveChanged += OnChange;
        _selectedUnit.ReactiveChanged += OnChange;
    }

    private void OnChange(Reactive r)
    {
        if (_activeTool.Value is not null)
        {
            DescriptionPanel.Hide();
        }

        else
        {
            DescriptionPanel.Show();   
        }

        Tool tool = _selectedTool.Value;
        if (tool is not null)
        {
            DescriptionLabel.Text = tool.Description;
        }
        else
        {
            DescriptionLabel.Text = "";
        }

        Unit unit = _selectedUnit.Value;
        if (unit is not null)
        {
            UnitLabel.Text = unit.Name;
        }
        else
        {
            UnitLabel.Text = "";
        }
    }
}