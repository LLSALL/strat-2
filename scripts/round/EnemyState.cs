using Godot;
using System;

public partial class EnemyState : Resource
{
    ReactiveArray<Unit> Units;
    ReactiveObject <Unit> SelectedUnit;
}
