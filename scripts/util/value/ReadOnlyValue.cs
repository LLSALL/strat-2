using Godot;
using System;
using System.Numerics;

public partial class ArrayReadOnlyValue<[MustBeVariant] T> : Value
{
    private ArrayValue<T> _backingValue;

    public Godot.Collections.Array<T> Value
    {
        get => _backingValue.Value;
    }

    public ArrayReadOnlyValue(ArrayValue<T> backingValue, Value initialOwner = null) : base(initialOwner)
    {
        _backingValue = backingValue;
        if (_backingValue is null) return;
        _backingValue.Owner = this;
    }
}

public partial class BoolReadOnlyValue : Value
{
    private BoolValue _backingValue;

    public bool Value
    {
        get => _backingValue.Value;
    }

    public BoolReadOnlyValue(BoolValue backingValue, Value initialOwner = null) : base(initialOwner)
    {
        _backingValue = backingValue;
        if (_backingValue is null) return;
        _backingValue.Owner = this;
    }
}

public partial class LockReadOnlyValue<T> : Value where T : class
{
    private LockValue<T> _backingValue;

    public T Value
    {
        get => _backingValue.Value;
    }

    public LockReadOnlyValue(LockValue<T> backingValue, Value initialOwner = null) : base(initialOwner)
    {
        _backingValue = backingValue;
        if (_backingValue is null) return;
        _backingValue.Owner = this;
    }
}

public partial class NumberReadOnlyValue<T> : Value where T : INumber<T>
{
    private NumberValue<T> _backingValue;

    public T Value
    {
        get => _backingValue.Value;
    }

    public NumberReadOnlyValue(NumberValue<T> backingValue, Value initialOwner = null) : base(initialOwner)
    {
        _backingValue = backingValue;
        if (_backingValue is null) return;
        _backingValue.Owner = this;
    }
}

public partial class ObjectReadOnlyValue<T> : Value
{
    private ObjectValue<T> _backingValue;

    public T Value
    {
        get => _backingValue.Value;
    }

    public ObjectReadOnlyValue(ObjectValue<T> backingValue, Value initialOwner = null) : base(initialOwner)
    {
        _backingValue = backingValue;
        if (_backingValue is null) return;
        _backingValue.Owner = this;
    }
}