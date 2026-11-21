using Godot;
using System;

public partial class RoundEnemyState : Resource
{
	public ReactiveBool IsEnemyTurn = new(false, null);
	public ReactiveArray<Unit> Units = new([]);
	public ReactiveObject <Unit> SelectedUnit = new(null);
}
