using Godot;
using System;

public partial class ClassNamedNode : Node
{
    public override void _EnterTree()
    {
        base._EnterTree();
        Name = this.GetType().Name;
    }
}