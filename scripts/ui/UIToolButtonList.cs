using Godot;
using System;
using System.Collections.Generic;

public partial class UIToolButtonList : BoxContainer
{
	[Export] public NodePath PlayerStatePath { get; set; }
	[Export] public NodePath ToolListPath { get; set; }

	private RoundPlayerState _playerState;

	private ArrayReadOnlyValue<Tool> _toolBox;
	private ObjectReadOnlyValue<Tool> _selectedTool;
	private LockReadOnlyValue<Tool> _activeTool;
	private Dictionary<Tool, Button> _toolButtonMap;

	public override void _Ready()
	{
		base._Ready();

		_playerState = GetNode<RoundPlayerState>(PlayerStatePath);
		_toolBox = _playerState.ToolBox;
		_selectedTool = _playerState.SelectedTool;
		_activeTool = _playerState.ActiveTool;

		var globalToolList = GetNode<GlobalToolList>(ToolListPath);
		var globalTools = globalToolList.ToolList;

		_toolButtonMap = [];
		foreach (Tool tool in globalTools)
		{
			Button button = new();

			button.Text = tool.Title;
			button.Pressed += () => { _playerState.SetSelectedTool(tool); };
			button.Hide();
			button.Disabled = true;

			_toolButtonMap.Add(tool, button);
			AddChild(button);
		}

		_toolBox.ReactiveChanged += SetButtonStates;
		_selectedTool.ReactiveChanged += SetButtonStates;
		_activeTool.ReactiveChanged += SetButtonStates;

		SetButtonStates(null);
	}
	private void SetButtonStates(Value r)
	{
		foreach (var (tool, button) in _toolButtonMap)
		{
			if (!_playerState.ToolBox.Value.Contains(tool)) // This is a linear check every time
			{
				button.Hide();
				button.Disabled = true;
			}
			else if (_selectedTool.Value == tool)
			{
				button.Show();
				button.Disabled = true;
			}
			else
			{
				button.Show();
				button.Disabled = false;
			}
		}
	}
}
