using Godot;
using System;

public partial class ReactiveObject<T> : Reactive
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

    public ReactiveObject(T intialValue, Reactive initialOwner = null) : base(initialOwner)
    {
        _value = intialValue;
    }
}