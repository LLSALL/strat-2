using Godot;
using System;

public partial class RoundState : Node
{
	public RoundPlayerState PlayerState = new();
	public RoundEnemyState EnemyState = new();
	public RoundMapState MapState = new();
}
