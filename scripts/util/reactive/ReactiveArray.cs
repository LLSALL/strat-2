using Godot;
using System;

public partial class ArrayValue<[MustBeVariant] T> : Value
{
    private Godot.Collections.Array<T> _value;
    public Godot.Collections.Array<T> Value
    {
        get => _value;
        set
        {
            _value = value;
            EmitSignal(SignalName.ReactiveChanged, this);
        }
    }

    public ArrayValue(Godot.Collections.Array<T> initialValue, Value initialOwner = null) : base(initialOwner)
    {
        _value = initialValue;
    }

    public ArrayValue(Value initialOwner = null) : base(initialOwner)
    {
        _value = new Godot.Collections.Array<T>();
    }

    public T this[int i]
    {
        get => Value[i];
        set
        {
            _value[i] = value;
            EmitSignal(SignalName.ReactiveChanged, this);
        }
    }

    public void Append(T item)
    {
        _value.Add(item);
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void AppendArray(Godot.Collections.Array<T> array)
    {
        _value.AddRange(array);
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Assign(Godot.Collections.Array<T> value)
    {
        _value = value;
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Clear()
    {
        _value.Clear();
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Remove(T item)
    {
        _value.Remove(item);
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Insert(int index, T item)
    {
        _value.Insert(index, item);
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Shuffle()
    {
        _value.Shuffle();
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void Sort()
    {
        _value.Sort();
        EmitSignal(SignalName.ReactiveChanged, this);
    }
}