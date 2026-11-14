using Godot;
using System;

public partial class PlayerState : Resource
{
    ReactiveArray<Unit> Units;
    ReactiveObject<Unit> SelectedUnit;

    ReactiveArray<Tool> Tools;
    ReactiveObject<Tool> SelectedTool;
    ReactiveLock<Tool> ActiveTool;
}
