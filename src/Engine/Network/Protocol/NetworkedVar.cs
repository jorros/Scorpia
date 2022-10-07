using System;

namespace Scorpia.Engine.Network.Protocol;

public class NetworkedVar<T>
{
    public event EventHandler<VarChangedEventArgs<T>> OnChange;

    public NetworkedVar(Func<ushort, bool> shouldReceive = null, T value = default)
    {
        _value = value;
        this.shouldReceive = shouldReceive;
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
        var oldVal = _value;
        _value = val;
        OnChange?.Invoke(this, new VarChangedEventArgs<T> {OldValue = oldVal, NewValue = _value});
    }

    internal void Accept(object val)
    {
        var oldVal = _value;
        _value = (T) val;
        OnChange?.Invoke(this, new VarChangedEventArgs<T> {OldValue = oldVal, NewValue = (T) val});
    }

    private T _value;
    internal readonly Func<ushort, bool> shouldReceive;
    private T _proposedValue;

    internal bool IsDirty { get; private set; }
}