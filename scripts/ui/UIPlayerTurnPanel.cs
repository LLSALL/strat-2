using Godot;
using System;

public partial class UIPlayerTurnPanel : Control
{
    [Export] public NodePath RoundStatePath { get; set; }

    private RoundState _roundState;
    private BoolValue _isPlayerTurn;

    public override void _Ready()
    {
        base._Ready();
        _roundState = GetNode<RoundState>(RoundStatePath);
        _isPlayerTurn = _roundState.PlayerState.IsPlayerTurn;

        _isPlayerTurn.ReactiveChanged += OnPlayerTurnChange;
    }

    private void OnPlayerTurnChange(Value r)
    {
        if (_isPlayerTurn.Value == true)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}