using Godot;
using System;

public partial class ObjectValue<T> : Value
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

    public ObjectValue(T intialValue, Value initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}