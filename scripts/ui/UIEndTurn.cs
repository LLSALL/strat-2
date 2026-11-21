using Godot;
using System;

public partial class UIEndTurn : Node
{
    public StringName GroupName = "ui";
    [Export] public NodePath RoundStatePath { get; set; }
    [Export] public NodePath ToolListPath { get; set; }
    [Export] public NodePath RoundManagerPath { get; set; }
    [Export] public Button EndTurnButton { get; set; }

    private RoundState _roundState;
    private RoundManager _roundManager;

    public override void _EnterTree()
    {

    }

    public override void _Ready()
    {
        _roundState = GetNode<RoundState>(RoundStatePath);
        _roundManager = GetNode<RoundManager>(RoundManagerPath);
        EndTurnButton.Pressed += OnButtonPress;
    }


    public void OnButtonPress()
    {
        _roundManager.EndPlayerTurn();
        
    }
}
