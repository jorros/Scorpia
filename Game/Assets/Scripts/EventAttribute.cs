using System;

public class EventAttribute : Attribute
{
    public string Name { get; }

    public EventAttribute(string name)
    {
        Name = name;
    }
}