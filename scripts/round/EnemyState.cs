using Godot;
using System;

public partial class RoundEnemyState : Reactive
{
	public ReactiveBool IsEnemyTurn = new(false, null);

	public ReactiveArray<Unit> Units = new([]);
	public ReactiveObject <Unit> SelectedUnit = new(null);

	public RoundEnemyState()
	{
		IsEnemyTurn = new(false, this);
		
		Units = new([], this);
		SelectedUnit = new(null, this);
	}
}
