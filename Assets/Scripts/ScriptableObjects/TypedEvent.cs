using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypedEvent<T> : ScriptableObject
{
    private readonly List<TypedEventListener<T>> eventListeners = new List<TypedEventListener<T>>();

    public void Raise(T data)
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(data);
    }

    public void RegisterListener(TypedEventListener<T> listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(TypedEventListener<T> listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}
