using Godot;
using System;
using System.Collections.Generic;

public partial class ToolButtonList : Node
{
    public StringName GroupName = "ui";
    [Export] public NodePath RoundStatePath { get; set; }
    [Export] public NodePath ToolListPath { get; set; }

    private Godot.Collections.Array<Tool> _globalToolList;

    private RoundState _roundState;

    private ReactiveArray<Tool> _playerTools;
    private ReactiveObject<Tool> _selectedTool;
    private ReactiveLock<Tool> _activeTool;

    private VBoxContainer _container;
    private Dictionary<Tool, Button> _toolButtonMap;

    public override void _EnterTree()
    {
        AddToGroup(GroupName);
    }

    public override void _Ready()
    {
        _roundState = GetNode<RoundState>(RoundStatePath);

        _playerTools = _roundState.PlayerState.Tools;
        _selectedTool = _roundState.PlayerState.SelectedTool;
        _activeTool = _roundState.PlayerState.ActiveTool;

        _playerTools.ReactiveChanged += OnChange;
        _selectedTool.ReactiveChanged += OnChange;
        _activeTool.ReactiveChanged += OnChange;

        _globalToolList = GetNode<GlobalToolList>(ToolListPath).ToolList;
        _toolButtonMap = new();
        foreach (Tool tool in _globalToolList)
        {
            Button button = new();
            button.Text = tool.GetTitle();
            button.Pressed += () =>
            {
                if (_activeTool.Value is not null) return;
                _selectedTool.Value = tool;
            };

            _container.AddChild(button);
            _toolButtonMap.Add(tool, button);

            button.Hide();
        }
    }

    private void OnChange(Reactive tools)
    {
        ClearList();
        if (_playerTools.Value is null) return;
        BuildList();
    }

    private void ClearList()
    {
        foreach ((var tool, var button) in _toolButtonMap)
        {
            button.Hide();
        }
    }

    private void BuildList()
    {
        if (_playerTools.Value is null) return;

        foreach (Tool tool in _playerTools.Value)
        {
            Button button = _toolButtonMap[tool];
            switch (_activeTool.Value is not null, tool == _selectedTool.Value, tool.CheckConditions())
            {
                case (true, _, _):
                case (_, true, _):
                case (_, _, false):
                    button.Show();
                    button.Disabled = true;
                    break;

                case (_, _, true):
                    button.Show();
                    button.Disabled = false;
                    break;
            }
        }
    }
}