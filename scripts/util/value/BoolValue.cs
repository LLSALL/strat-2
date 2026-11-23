using Godot;
using System;
using System.Numerics;

public partial class BoolValue : Value
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

    public BoolValue(bool intialValue, Value initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}