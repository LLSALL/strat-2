using Godot;
using System;

public partial class Value : Resource
{
    private Value _owner;
    public Value Owner
    {
        get => _owner;
        set
        {
            if (_owner is not null)
            {
                ReactiveChanged -= _owner.Propagate;
            }

            _owner = value;

            if (_owner is not null)
            {
                ReactiveChanged += _owner.Propagate;
            }
        }
    }

    [Signal] public delegate void ReactiveChangedEventHandler(Value reactive);

    public Value(Value initialOwner = null)
    {
        Owner = initialOwner;
    }

    public void Propagate(Value reactive = null)
    {
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void ManuallyEmit()
    {
        EmitSignal(SignalName.ReactiveChanged, this);
    }
}