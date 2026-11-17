using Godot;
using System;

public partial class MoveTool : Tool
{
    public override StringName GetTitle()
    {
        return "Move";
    }

    public override bool CheckConditions()
    {
        return true;
    }

    protected override void OnUnitChange(Reactive selectedUnit)
    {
    }

    protected override void OnSelectedChange(Reactive selectedTool)
    {
    }

    protected override void OnActiveChange(Reactive activeTool)
    {
    }
}