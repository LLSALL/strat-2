using Godot;
using System;

public partial class Reactive : Resource
{
    private Reactive _owner;
    public Reactive Owner
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

    [Signal] public delegate void ReactiveChangedEventHandler(Reactive reactive);

    public Reactive(Reactive initialOwner = null)
    {
        Owner = initialOwner;
    }

    public void Propagate(Reactive reactive = null)
    {
        EmitSignal(SignalName.ReactiveChanged, this);
    }

    public void ManuallyEmit()
    {
        EmitSignal(SignalName.ReactiveChanged, this);
    }
}