using Godot;
using System;
using System.Numerics;

public partial class NumberValue<T> : Value where T : INumber<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            EmitSignal(SignalName.ReactiveChanged, this);
        }
    }

    public NumberValue(T intialValue, Value initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}