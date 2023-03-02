using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TypedEventListener<T> : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public TypedEvent<T> Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T data)
    {
        Response.Invoke(data);
    }
}
