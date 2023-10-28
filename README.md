<p align="center">
	<img width="256px" src="https://github.com/GodotParadise/FSM-Csharp/blob/main/icon.jpg" alt="GodotParadiseFSM logo" />
	<h1 align="center">Godot Paradise FSM</h1>
	
[![LastCommit](https://img.shields.io/github/last-commit/GodotParadise/FSM?cacheSeconds=600)](https://github.com/GodotParadise/FSM-Csharp/commits)
[![Stars](https://img.shields.io/github/stars/godotparadise/FSM)](https://github.com/GodotParadise/FSM-Csharp/stargazers)
[![Total downloads](https://img.shields.io/github/downloads/GodotParadise/FSM-Csharp/total.svg?label=Downloads&logo=github&cacheSeconds=600)](https://github.com/GodotParadise/FSM-Csharp/releases)
[![License](https://img.shields.io/github/license/GodotParadise/FSM?cacheSeconds=2592000)](https://github.com/GodotParadise/FSM-Csharp/blob/main/LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat&logo=github)](https://github.com/godotparadise/FSM-Csharp/pulls)
[![](https://img.shields.io/discord/1167079890391138406.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/XqS7C34x)
</p>

[![es](https://img.shields.io/badge/lang-es-yellow.svg)](https://github.com/GodotParadise/FSM-Csharp/blob/main/locale/README.es-ES.md)
- - -

A finite state machine designed to cover 95% of use cases, providing essential functionality and a basic state node that can be extended now in C#.

- [Requirements](#requirements)
- [✨Installation](#installation)
	- [Automatic (Recommended)](#automatic-recommended)
	- [Manual](#manual)
	- [GDScript](#gdscript)
	- [CSharp GlobalClasses](#csharp-globalclasses)
- [Getting started](#getting-started)
- [Guide](#guide)
	- [GodotParadiseState](#godotparadisestate)
		- [Enter()](#enter)
		- [Exit()](#exit)
		- [HandleInput(InputEvent @event)](#handleinputinputevent-event)
		- [PhysicsUpdate(double delta)](#physicsupdatedouble-delta)
		- [Update(double delta)](#updatedouble-delta)
		- [OnAnimationPlayerFinished(string name)](#onanimationplayerfinishedstring-name)
		- [OnAnimationFinished()](#onanimationfinished)
	- [Signals](#signals)
- [The Finite State Machine *(FSM)*](#the-finite-state-machine-fsm)
	- [Exported parameters](#exported-parameters)
	- [Accessible parameters](#accessible-parameters)
	- [How to change the state](#how-to-change-the-state)
	- [Functions](#functions)
		- [ChangeState(GodotParadiseState newState, Dictionary parameters, bool force = false)](#changestategodotparadisestate-newstate-dictionary-parameters-bool-force--false)
		- [ChangeStateByName(string name, Dictionary parameters, bool force = false)](#changestatebynamestring-name-dictionary-parameters-bool-force--false)
		- [EnterState(GodotParadiseState state)](#enterstategodotparadisestate-state)
		- [ExitState(GodotParadiseState state)](#exitstategodotparadisestate-state)
		- [GetStateByName(string name)](#getstatebynamestring-name)
		- [bool CurrentStateIs(GodotParadiseState state)](#bool-currentstateisgodotparadisestate-state)
		- [bool CurrentStateNameIs(string name)](#bool-currentstatenameisstring-name)
		- [LockStateMachine()](#lockstatemachine)
		- [UnlockStateMachine()](#unlockstatemachine)
	- [Signals](#signals-1)
- [✌️You are welcome to](#️you-are-welcome-to)
- [🤝Contribution guidelines](#contribution-guidelines)
- [📇Contact us](#contact-us)


# Requirements
📢 We don't currently give support to Godot 3+ as we focus on future stable versions from version 4 onwards
* Godot 4+

# ✨Installation
## Automatic (Recommended)
You can download this plugin from the official [Godot asset library](https://godotengine.org/asset-library/asset/2039) using the AssetLib tab in your godot editor. Once installed, you're ready to get started
##  Manual 
To manually install the plugin, create an **"addons"** folder at the root of your Godot project and then download the contents from the **"addons"** folder of this repository.
## GDScript
This plugin has also been written in C# and you can find it on [FSM](https://github.com/GodotParadise/FSM)

## CSharp GlobalClasses
In order to make available in the Godot editor the custom classes you've created, that unlike gdscript, in C# you need use Global classes.

So for example if you create a `Idle` state that inherits from `GodotParadiseState` in this way, you cannot add this as child in the scene tree via editor until you use the decorator:
```csharp
using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState
{
    public override void Enter()
    {
       GD.Print("Idle start");
    }
}

```

# Getting started
The finite state machine can be added as any other node in the scene tree where you want to use it. 
![fsm-add-node](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_add_child.png)
![fsm-added-scene-tree](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_added_scene_tree.png)
![fsm-example](https://github.com/GodotParadise/FSM-Csharp/blob/main/images/fsm_example.png)

⚠️ The finite state machine **always need at least one default state** to start with, this default state can be set on the exported variable `CurrentState`. Once this is done, when executing the scene this will be the current state of the machine until the conditions that change the state occur. 
While nothing will break without it, having a defined initial state is good practice to start from.

There will always be only one `_PhysicProcess()` or `_Process_()` since it is the main machine that is in charge of calling the virtual methods of each state.If your state overrides `PhysicsUpdate()` will be executed as `_PhysicProcess()`

`Enter()` and `Exit()` are called when the new state becomes the current and when it will transition to another state. They are useful to clean up or get ready some sort of parameters inside the state to be used only in this state.
 
# Guide
## GodotParadiseState
All the functions here are virtual, which means they can be overridden with the desired functionality in each case.

In all states you have access to the `PreviousStates` and the extra `Parameters` you have exchanged between transition and transition.
The `PreviousStates` are available only if you enabled the stack in the FSM.

```csharp
using Godot;
using Godot.Collections;

public partial class GodotParadiseState : Node
{
	[Signal]
	public delegate void StateEnteredEventHandler();


	[Signal]
	public delegate void StateFinishedEventHandler(string nextState, Dictionary parameters);

	public Array<GodotParadiseState> PreviousStates = new();
	public Dictionary parameters = new();



	public virtual void Enter()
	{

	}

	public virtual void Exit()
	{

	}

	public virtual void HandleInput(InputEvent @event)
	{

	}

	public virtual void PhysicsUpdate(double delta)
	{

	}

	public virtual void Update(double delta)
	{

	}

	public virtual void OnAnimationPlayerFinished(string Name)
	{

	}

	public virtual void OnAnimationFinished()
	{

	}
}

```

### Enter()
This function executes when the state enters for the first time as the current state.
### Exit()
This function executes when the state exits from being the current state and transitions to the next one.
### HandleInput(InputEvent @event)
In case you want to customize how this state handle the inputs in your game this is the place to do that. The event type is InputEvent
### PhysicsUpdate(double delta)
This function executes on each frame of the finite state machine's physic process
### Update(double delta)
This function executes on each frame of the finite state machine's process
### OnAnimationPlayerFinished(string name)
You can use this function generically to execute custom logic when an AnimationPlayer finishes any animation. This receive the animation name as parameter to avoid errors and be consistent with the original signal.
### OnAnimationFinished()
You can use this function generically to execute custom logic when an AnimatedSprite(2/3)D finishes any animation. This does not receive any params to avoid errors and be consistent with the original signal.

## Signals
- *StateEntered*
- *StateFinished(GodotParadiseState next_state, Dictionary parameters)*

So for example if you want to implement a **Idle** state it's easy as:
```csharp
using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState {
	public override void Enter() {
		// play animations...
		// set velocity to zero...
	}
	public override void Exit() {
		// stop animations...
	}
	public override void PhysicsUpdate(double delta) {
		// detect the input direction to change to another state such as Walk or Crouch
	}
}

```

# The Finite State Machine *(FSM)*
## Exported parameters
- CurrentState: GodotParadiseState = null
- StackCapacity: int = 3
- FlushStackWhenReachCapacity: bool = false
- EnableStack: bool = true
## Accessible parameters
- States: Dictionary
- StatesStack: Array[GodotParadiseState]
- Locked: bool

When this node is ready in the scene tree, all the states detected as children **at any nesting level** are saved in a dictionary for easy access by their node names. 

The **finite state machine** connects to all the `StateFinished` signals from the nested existing states.
When a change state happens and the **stack is enabled**, the previous state is appended to the `StatesStack`. You can define a `StackCapacity` to define the number of previous states you want to save. This stack is accessible on each state to handle conditions in which we need to know which states have been previously transitioned.
The locked value enables the state machine to be locked or unlocked for state execution. It can be resumed by resetting it to false. When it's locked **the stack is also disabled.**

## How to change the state
This is an example of code that changes state from Idle to Run:
```csharp
using Godot;
using System;

[GlobalClass, Icon("res://addons/finite_state_machine/state_icon.png")]
public partial class Idle : GodotParadiseState {

	public override void PhysicsUpdate(double delta) {
		Vector2 InputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		 if (!InputDirection.IsZeroApprox()) {
			EmitSignal(SignalName.StateFinished, "Walk", new()); // Instead of empty Dictionary new() you can also pass parameters here
        }
	}
}
```
As you can see, within each individual state, you have the option to emit the `StateFinished` signal, which will be monitored by the parent state machine.
You can find a more complex example in the repository [FirstPersonController](https://github.com/GodotParadise/First-Person-Controller/tree/main/first_person_controller/state_machine)

## Functions
Usually **you don't really want to call this functions manually**, it is preferable to emit signals from the states themselves and let the finite state machine react to these signals in order to execute actions such as changing the state. By the way, nothing stops you yo do that and may be needed in your use case.

### ChangeState(GodotParadiseState newState, Dictionary parameters, bool force = false)
Changes the current state to the next state passed as parameter if they are not the same. This action can be forced with the third parameter force.
If the state can be transitioned, the `Exit()` function from the current state and the `Enter()` function of the next state will be executed.
In this transition the new state can receive external parameters. Emits the signal `StateChanged`

### ChangeStateByName(string name, Dictionary parameters, bool force = false)
Perform the same action as the `ChangeState` function but by receiving the state with the name it has in the states dictionary. For example, if we have a node state named **'Idle'** in the scene, it can be changed using `ChangeStateByName("Idle")`

### EnterState(GodotParadiseState state)
This function is called when a new state becomes the current state. During this process, the `state_entered` signal is emitted.

### ExitState(GodotParadiseState state)
Exit the state passed as parameter, execute the `_exit()` function on this state.
### GetStateByName(string name)
Returns the state node using the dictionary key from the states variable if it exists, or null if it does not.

### bool CurrentStateIs(GodotParadiseState state)
Check if the current state is the one passed as parameter

### bool CurrentStateNameIs(string name)
Same as above but using the dictionary key from states

### LockStateMachine()
Lock the state machine, all the process are set to false and the stack is disabled. This function is called automatically when locked changes to false

### UnlockStateMachine()
Unlock the machine, all the process are set to true and stack is enabled again (if it was enabled). This function is called automatically when locked changes to true

## Signals
- *StateChanged(GodotParadiseState fromState, GodotParadiseState state)*
- *StackPushed(GodotParadiseState newState, Array<GodotParadiseState> stack)*
- *StackFlushed(Array<GodotParadiseState> stack)*


# ✌️You are welcome to
- [Give feedback](https://github.com/GodotParadise/FSM-Csharp/pulls)
- [Suggest improvements](https://github.com/GodotParadise/FSM-Csharp/issues/new?assignees=BananaHolograma&labels=enhancement&template=feature_request.md&title=)
- [Bug report](https://github.com/GodotParadise/FSM-Csharp/issues/new?assignees=BananaHolograma&labels=bug%2C+task&template=bug_report.md&title=)

GodotParadise is available for free.

If you're grateful for what we're doing, please consider a donation. Developing GodotParadise requires massive amount of time and knowledge, especially when it comes to Godot. Even $1 is highly appreciated and shows that you care. Thank you!

- - -
# 🤝Contribution guidelines
**Thank you for your interest in Godot Paradise!**

To ensure a smooth and collaborative contribution process, please review our [contribution guidelines](https://github.com/GodotParadise/FSM-Csharp/blob/main/CONTRIBUTING.md) before getting started. These guidelines outline the standards and expectations we uphold in this project.

**📓Code of Conduct:** We strictly adhere to the [Godot code of conduct](https://godotengine.org/code-of-conduct/) in this project. As a contributor, it is important to respect and follow this code to maintain a positive and inclusive community.

- - -

# 📇Contact us
If you have built a project, demo, script or example with this plugin let us know and we can publish it here in the repository to help us to improve and to know that what we do is useful.
