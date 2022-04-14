using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class EventManager
{
    private readonly Dictionary<string, Dictionary<string, Action<object[]>>> eventDictionary;

    private static EventManager _eventManager;

    public const string PanCamera = "PanCamera";
    public const string ZoomOutCamera = "ZoomOutCamera";
    public const string ZoomInCamera = "ZoomInCamera";
    public const string SelectTile = "SelectTile";
    public const string DeselectTile = "DeselectTile";
    public const string ReceiveNotification = "ReceiveNotification";
    public const string RemoveNotification = "RemoveNotification";
    public const string MapRendered = "MapRendered";
    public const string PlayerInfo = "PlayerInfo";
    public const string LocationUpdated = "LocationUpdated";

    private static EventManager Instance
    {
        get { return _eventManager ??= new EventManager(); }
    }

    private EventManager()
    {
        eventDictionary = new Dictionary<string, Dictionary<string, Action<object[]>>>();
    }

    public static void RegisterAll(object instance)
    {
        var type = instance.GetType();
        foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                                               BindingFlags.NonPublic))
        {
            var attribute = method.GetCustomAttribute(typeof(EventAttribute));

            if (attribute is null)
            {
                continue;
            }

            var eventAttribute = (EventAttribute) attribute;

            void RunMethod(object[] x)
            {
                method.Invoke(instance, x);
            }

            if (Instance.eventDictionary.TryGetValue(eventAttribute.Name, out var events))
            {
                if (!events.TryAdd(instance.GetType().FullName, RunMethod))
                {
                    events[instance.GetType().FullName] = RunMethod;
                }
            }
            else
            {
                events = new Dictionary<string, Action<object[]>>
                {
                    [instance.GetType().FullName] = RunMethod
                };
                Instance.eventDictionary.Add(eventAttribute.Name, events);
            }
        }
    }

    public static void RemoveAll(object instance)
    {
        var type = instance.GetType();
        foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                                               BindingFlags.NonPublic))
        {
            var attribute = method.GetCustomAttribute(typeof(EventAttribute));

            if (attribute is null)
            {
                continue;
            }

            var eventAttribute = (EventAttribute) attribute;

            if (Instance.eventDictionary.TryGetValue(eventAttribute.Name, out var events))
            {
                events.Remove(instance.GetType().FullName);
            }
        }
    }

    public static void Trigger(string eventName, params object[] message)
    {
        if (!Instance.eventDictionary.TryGetValue(eventName, out var events))
        {
            return;
        }

        foreach (var evt in events)
        {
            evt.Value(message.Any() ? message : null);
        }
    }
}