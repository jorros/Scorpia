using System;

namespace Scorpia.Engine.SceneManagement;

[AttributeUsage(AttributeTargets.Method)]
public class EventAttribute : Attribute
{
    public string Name { get; }

    public EventAttribute(string name)
    {
        Name = name;
    }
}