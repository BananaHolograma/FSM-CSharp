using Godot;
using System;

public partial class GodotParadiseState : Node
{
	[Signal]
	public delegate void StateEnteredEventHandler


	[Signal]
			public delegate void StateFinishedEventHandler(nextState, Dictionary params)
}
