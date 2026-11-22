using Godot;
using System;
using System.Collections.Generic;

public partial class UIToolList : Node
{
	public StringName GroupName = "ui";
	[Export] public NodePath RoundStatePath { get; set; }
	[Export] public NodePath ToolListPath { get; set; }
	[Export] public CanvasItem Panel {get; set;}
	[Export] public BoxContainer ListContainer { get; set; }

	private Godot.Collections.Array<Tool> _globalToolList;

	private RoundState _roundState;

	private ReactiveBool _isPlayerTurn;
	private ReactiveObject<Unit> _selectedUnit;
	private ReactiveArray<Tool> _playerTools;
	private ReactiveObject<Tool> _selectedTool;
	private ReactiveLock<Tool> _activeTool;

	private Dictionary<Tool, Button> _toolButtonMap;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}

	public override void _Ready()
	{
		_roundState = GetNode<RoundState>(RoundStatePath);

		_playerTools = _roundState.PlayerState.Tools;
		_selectedUnit = _roundState.PlayerState.SelectedUnit;
		_selectedTool = _roundState.PlayerState.SelectedTool;
		_activeTool = _roundState.PlayerState.ActiveTool;
		_isPlayerTurn = _roundState.PlayerState.IsPlayerTurn;

		_isPlayerTurn.ReactiveChanged += OnChange;
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

			ListContainer.AddChild(button);
			_toolButtonMap.Add(tool, button);

			button.Hide();
		}
	}

	private void OnChange(Reactive r)
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
		if (_isPlayerTurn.Value == false)
		{
			Panel.Hide();
			return;
		} else
		{
			Panel.Show();
		}
		foreach (Tool tool in _playerTools.Value)
		{
			Button button = _toolButtonMap[tool];
			switch (_activeTool.Value is not null, tool == _selectedTool.Value, tool.CanUse(_selectedUnit.Value))
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
