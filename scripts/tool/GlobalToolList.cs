using Godot;
using System;
using System.Linq;

public partial class GlobalToolList : Node
{
    [Export] public NodePath RoundStatePath {get; set;}
    public Godot.Collections.Array<Tool> ToolList {get; private set;} = [];

    public override void _Ready()
    {
       ToolList = [];
       foreach (Node node in GetChildren())
        {
            if (node is Tool) ToolList.Add((Tool)node);
        }
    }
}