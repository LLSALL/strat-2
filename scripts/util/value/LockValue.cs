using Godot;
using System;

public partial class LockValue<T> : Value where T : class
{
    private T _value;
    public T Value
    {
        get => _value;
    }

    public LockValue(Value initialOwner = null) : base(initialOwner)
    {
        _value = null;
    }
    
    public bool TryLock(T key)
    {
        if (_value is not null) return false;
        if (_value == key) return false;

        _value = key;
        EmitSignal(SignalName.ReactiveChanged, this);
        return true;
    }

    public bool TryUnlock(T key) {
        if (_value is null) return false;
        if (_value != key) return false;

        _value = null;
        EmitSignal(SignalName.ReactiveChanged, this);
        return true;
    }
}