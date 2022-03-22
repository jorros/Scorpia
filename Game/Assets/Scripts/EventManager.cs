using System;
using System.Collections.Generic;

namespace Scorpia.Assets.Scripts
{
    public class EventManager
    {
        private Dictionary<string, Action<IReadOnlyList<object>>> eventDictionary;

        private static EventManager eventManager;

        public static string PanCamera => "PanCamera";
        public static string ZoomOutCamera => "ZoomOutCamera";
        public static string ZoomInCamera => "ZoomInCamera";
        public static string SelectTile => "SelectTile";
        public static string DeselectTile => "DeselectTile";
        public static string ReceiveNotification => "ReceiveNotification";
        public static string RemoveNotification => "RemoveNotification";
        public static string MapRendered => "MapRendered";

        private static EventManager instance
        {
            get
            {
                if (eventManager == null)
                {
                    eventManager = new EventManager();
                }

                return eventManager;
            }
        }

        public EventManager()
        {
            eventDictionary = new Dictionary<string, Action<IReadOnlyList<object>>>();
        }

        public static void Register(string eventName, Action<IReadOnlyList<object>> listener)
        {
            Action<IReadOnlyList<object>> thisEvent;

            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;
                instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void Remove(string eventName, Action<IReadOnlyList<object>> listener)
        {
            if (eventManager == null) return;
            Action<IReadOnlyList<object>> thisEvent;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;
                instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void Trigger(string eventName, params object[] message)
        {
            Action<IReadOnlyList<object>> thisEvent = null;
            if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent?.Invoke(message);
            }
        }
    }
}