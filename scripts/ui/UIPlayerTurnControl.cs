using Godot;
using System;

public partial class UIPlayerTurnControl : Control
{
    [Export] public NodePath PlayerStatePath { get; set; }

    private RoundPlayerState _playerState;
    private BoolReadOnlyValue _isPlayerTurn;

    public override void _Ready()
    {
        _playerState = GetNode<RoundPlayerState>(PlayerStatePath);
        _isPlayerTurn = _playerState.IsPlayerTurn;

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