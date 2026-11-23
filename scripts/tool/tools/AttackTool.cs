using Godot;
using System;

public partial class AttackTool : Tool
{
    [Export] public Camera3D cam { get; set; }

    public override StringName GetTitle()
    {
        return "Attack";
    }

    public override string GetDescription()
    {
        return "Attack desc";
    }

    public override bool CanAct(Unit unit)
    {
        return true;
    }

    protected override void EnableIdle()
    {
        SetProcessInput(true);
    }

    protected override void DisableIdle()
    {
        SetProcessInput(false);
    }

    protected override (GodotObject, StringName) BeginActive()
    {
        return NoContinuation;
    }

    protected override void CompleteActive() {}
    
    private const float RayLength = 1000.0f;

    // Current: When RMB pressed, check if there is a unit is under the cursor. 
    // If there is, call that unit's set health function. Tool does not become active.

    // TODO: Use LMB to select a unit if it is under the cursor. 
    // Have the player press a button to initiate the attack.
    // The attack should be animated, and the tool should become active for that duration.

    // Also have a GUI to manually swap between potential targets.
    // Guard against attacking self
    // Disallow targeting of friendlies.
    // Current unit should determine the amount of damage dealt.
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Right)
        {
            var spaceState = GetWorld3D().DirectSpaceState;
            var mousePos = GetViewport().GetMousePosition();

            var origin = cam.ProjectRayOrigin(mousePos);
            var end = origin + cam.ProjectRayNormal(mousePos) * RayLength;
            var query = PhysicsRayQueryParameters3D.Create(origin, end);
            query.CollideWithAreas = true;

            var result = spaceState.IntersectRay(query);
            Node collider = (Node)result["collider"]; // TODO figure out how to properly check this
            GD.Print(collider);
            GD.Print(collider.GetParent());
            Unit unit = (Unit) collider;
            if (unit is not null)
            {
                unit.Health -= 100;
            }
        }
    }
}