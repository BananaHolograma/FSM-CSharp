#if TOOLS
using Godot;

[Tool]
public partial class FiniteStateMachinePlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		AddCustomType("FiniteStateMachine",
			"Node",
			GD.Load<Script>("res://addons/finite-state-machine/FiniteStateMachine.cs"),
			GD.Load<Texture2D>("res://addons/finite-state-machine/icons/icon.png")
		);

		AddCustomType("State",
		"Node",
		GD.Load<Script>("res://addons/finite-state-machine/State.cs"),
		GD.Load<Texture2D>("res://addons/finite-state-machine/icons/state_icon.png")
	);
	}

	public override void _ExitTree() => RemoveCustomType("FiniteStateMachine");
}
#endif