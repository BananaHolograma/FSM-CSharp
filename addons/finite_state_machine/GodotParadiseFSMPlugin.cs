#if TOOLS
using Godot;
using System;

[Tool]
public partial class GodotParadiseFSMPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		AddCustomType("GodotParadiseFiniteStateMachine",
			"Node",
			GD.Load<Script>("res://addons/finite_state_machine/GodotParadiseFiniteStateMachine.cs"),
			GD.Load<Texture2D>("res://addons/finite_state_machine/icon.png")
		);
	}

	public override void _ExitTree() => RemoveCustomType("GodotParadiseFiniteStateMachine");
}
#endif