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
        throw new NotImplementedException();
    }

    protected override void OnUnitChange(Reactive selectedUnit)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnSelectedChange(Reactive selectedTool)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnActiveChange(Reactive activeTool)
    {
        throw new System.NotImplementedException();
    }
}