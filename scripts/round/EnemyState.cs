using Godot;
using System;

public partial class RoundEnemyState : Resource
{
    ReactiveArray<Unit> Units = new([]);
    ReactiveObject <Unit> SelectedUnit = new(null);
}
