using System;

namespace Scorpia.Engine.Network.Protocol;

public class NetworkedVar<T>
{
    public event EventHandler<VarChangedEventArgs<T>> OnChange;

    public NetworkedVar(T value = default)
    {
        _value = value;
    }

    public T Value
    {
        get => _value;
        set
        {
            _proposedValue = value;
            IsDirty = true;
        }
    }

    internal T GetProposedVal()
    {
        IsDirty = false;
        return _proposedValue;
    }

    internal void Accept(T val)
    {
        OnChange?.Invoke(this, new VarChangedEventArgs<T> {OldValue = _value, NewValue = val});
        _value = val;
    }

    internal void Accept(object val)
    {
        OnChange?.Invoke(this, new VarChangedEventArgs<T> {OldValue = _value, NewValue = (T) val});
        _value = (T) val;
    }

    private T _value;
    private T _proposedValue;

    internal bool IsDirty { get; private set; }
}