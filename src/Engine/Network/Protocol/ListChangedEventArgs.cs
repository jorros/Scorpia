using System;

namespace Scorpia.Engine.Network.Protocol;

public class ListChangedEventArgs<T> : EventArgs
{
    public int? Index { get; set; }
    
    public T Value { get; set; }
    
    public NetworkedListAction Action { get; set; }
}