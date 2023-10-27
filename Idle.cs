using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState
{
    public override void Enter()
    {
        GD.Print("SOY IDLE!!");
        GD.PrintRich(PreviousStates);

    }

    public override void Exit()
    {
        GD.Print("EXIT IDLE!!");
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (inputDirection.Equals(Vector2.Right))
        {
            EmitSignal(SignalName.StateFinished, "Walk", new());
        }
    }
}
