/*
## Created by https://github.com/bananaholograma with LICENSE MIT
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

public partial class FiniteStateMachine : Node
{
    [Signal]
    public delegate void StateChangedEventHandler(State fromState, State state);
    [Signal]
    public delegate void StackPushedEventHandler(State newState, Array<State> stack);
    [Signal]
    public delegate void StackFlushedEventHandler(Array<State> stack);

    [Export]
    public CharacterBody2D Actor;
    [Export]
    public State CurrentState;
    [Export]
    public int StackCapacity = 3;
    [Export]
    public bool FlushStackWhenReachCapacity = false;
    [Export]
    public bool EnableStack { get; set; } = true;

    public Dictionary States = new();
    public Array<State> StatesStack = new();
    public bool Locked = false;

    public override void _Ready()
    {
        InitializeStateNodes();

        foreach (State state in States.Values)
        {
            state.StateFinished += OnFinishedState;
            state.FSM = this;
            state.Ready();
        }

        if (CurrentState is not null)
        {
            ChangeState(CurrentState, new(), true);
        }

        UnlockStateMachine();

        StackPushed += OnStackPushed;
    }


    public override void _UnhandledInput(InputEvent @event)
    {
        CurrentState.HandleInput(@event);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicsUpdate(delta);
    }

    public override void _Process(double delta)
    {
        CurrentState.Update(delta);
    }

    public void ChangeState(State newState, Dictionary parameters, bool force = false)
    {
        if (!force && CurrentStateIs(newState))
        {
            return;
        }

        if (CurrentState is not null)
        {
            ExitState(CurrentState);
        }

        PushStateToStack(CurrentState);
        EmitSignal(SignalName.StateChanged, CurrentState, newState);

        CurrentState = newState;
        CurrentState.parameters = parameters;

        EnterState(newState);
    }

    public void ChangeStateByName(string name, Dictionary parameters, bool force = false)
    {
        State state = GetStateByName(name);

        if (state is not null)
        {
            ChangeState(state, parameters, force);
        }

        GD.PushError($"FSMPlugin: The state {name} does not exists on this FiniteStateMachine");
    }

    public void EnterState(State state)
    {
        state.Enter();
        state.EmitSignal(State.SignalName.StateEntered);
    }


    public void ExitState(State state)
    {
        state.Exit();
    }


    public bool CurrentStateIs(State state)
    {
        return state.Name.ToString().ToLower().Equals(CurrentState.Name.ToString().ToLower());
    }

    public bool CurrentStateNameIs(string name)
    {
        State state = GetStateByName(name);

        if (state is not null)
        {
            return CurrentStateIs(state);
        }

        return false;
    }

    public State GetStateByName(string name)
    {
        if (States.ContainsKey(name))
        {
            return (State)States[name];
        }

        return null;
    }

    public void PushStateToStack(State state)
    {
        if (EnableStack && StackCapacity > 0)
        {
            if (StatesStack.Count >= StackCapacity)
            {
                if (FlushStackWhenReachCapacity)
                {
                    EmitSignal(SignalName.StackFlushed, StatesStack);
                    StatesStack.Clear();
                }
                else
                {
                    StatesStack.RemoveAt(0);
                }
            }

            StatesStack.Add(state);
            EmitSignal(SignalName.StackPushed, state, StatesStack);
        }
    }

    public void LockStateMachine()
    {
        SetProcess(false);
        SetPhysicsProcess(false);
        SetProcessInput(false);
        SetProcessUnhandledInput(false);
    }

    public void UnlockStateMachine()
    {
        SetProcess(true);
        SetPhysicsProcess(true);
        SetProcessInput(true);
        SetProcessUnhandledInput(true);
    }

    private void AddStateToDictionary(State state)
    {
        if (state.IsInsideTree())
        {
            States.Add(state.Name, GetNode(state.GetPath()));
        }
    }


    private void InitializeStateNodes(Node node = null)
    {
        Array<Node> childrens = node != null ? node.GetChildren(true) : GetChildren(true);

        foreach (Node child in childrens)
        {
            if (child is State state)
            {
                AddStateToDictionary(state);
            }
            else
            {
                InitializeStateNodes(child);
            }
        }

    }

    private void OnFinishedState(string nextState, Dictionary parameters)
    {
        State state = GetStateByName(nextState);

        if (state is not null)
        {
            ChangeState(state, parameters);
        }
    }


    private void OnStackPushed(State newState, Array<State> stack)
    {
        foreach (State state in States.Values)
        {
            state.PreviousStates = stack;
        }
    }


}