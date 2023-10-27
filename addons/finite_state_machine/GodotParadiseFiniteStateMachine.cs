/*
## Created by https://github.com/GodotParadise organization with LICENSE MIT
# There are no restrictions on modifying, sharing, or using this component commercially
# We greatly appreciate your support in the form of stars, as they motivate us to continue our journey of enhancing the Godot community
# ***************************************************************************************
# A finite state machine designed to cover 95% of use cases, providing essential functionality and a basic state node that can be extended now in C#.

# There is nothing wrong using the same process on the CharacterBody2D to handle all the movement but when the things start to grow and the player can perform a wide number of moves it is better to start thinking about a modular way to build this movement. 
# In this case, a state machine is a design pattern widely employed in the video game industry to manage this complexity
##

*/

using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class GodotParadiseFiniteStateMachine : Node
{
    [Signal]
    public delegate void StateChangedEventHandler(GodotParadiseState fromState, GodotParadiseState state);
    [Signal]
    public delegate void StackPushedEventHandler(GodotParadiseState newState, Array<GodotParadiseState> stack);
    [Signal]
    public delegate void StackFlushedEventHandler(Array<GodotParadiseState> stack);

    [Export]
    public GodotParadiseState CurrentState;
    [Export]
    public int StackCapacity = 3;
    [Export]
    public bool FlushStackWhenReachCapacity = false;
    [Export]
    public bool EnableStack = false;

    public Dictionary States = new();
    public Array<GodotParadiseState> StatesStack = new();
    public bool Locked = false;



    public void LockStateMachine()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
        SetProcessUnhandledInput(false);
        EnableStack = false;
    }

    public void UnlockStateMachine()
    {
        SetProcess(true);
        SetPhysicsProcess(true);
        SetProcessInput(true);
        SetProcessUnhandledInput(true);
        EnableStack = true;
    }
}