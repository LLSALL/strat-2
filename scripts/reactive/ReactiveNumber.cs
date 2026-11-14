using Godot;
using System;
using System.Numerics;

public partial class ReactiveNumber<T> : Reactive where T : INumber<T>
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

    public ReactiveNumber(T intialValue, Reactive initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}