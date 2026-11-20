using Godot;
using System;
using System.Numerics;

public partial class ReactiveBool : Reactive
{
    private bool _value;
    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            EmitSignal(SignalName.ReactiveChanged, this);
        }
    }

    public ReactiveBool(bool intialValue, Reactive initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}