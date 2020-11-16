using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    //When you create a game event create a new empty list of things listening to it
    private List<GameEventListener> listeners = new List<GameEventListener>();

    //actually trigger the event and find every listener for the event and trigger it on that
    public void Raise()
    {
        for (int i =listeners.Count - 1; i >= 0;i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    //Tell the event "this is listening"
    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
