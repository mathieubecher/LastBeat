using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PostMethods
{
    OnStart,
    OnTriggerEnter,
    OnTriggerExit,
    OnCollision,
    OnScript,
}

[RequireComponent(typeof(PEngineObject))]
public class PEnginePostEvent : MonoBehaviour
{
    public EventSO eventToPost;
    public PostMethods postMethod;
    public bool overrideOutputBus;
    
    public PEngineListener[] listeners;
    public int[] listenersID;
    
    private List<PEngineListener> totalListeners = new List<PEngineListener>();

    private void Awake()
    {
        foreach (PEngineListener listener in listeners)
            totalListeners.Add(listener);

        foreach (int listenerID in listenersID)
            totalListeners.Add(PEngine.GetListener(listenerID));
    }

    void Start()
    {
        if (postMethod == PostMethods.OnStart)
            PEngine.PostEvent(eventToPost.name, gameObject, totalListeners);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (postMethod == PostMethods.OnTriggerEnter)
            PEngine.PostEvent(eventToPost.name, gameObject, totalListeners);
    }

    private void OnTriggerExit(Collider other)
    {
        if(postMethod == PostMethods.OnTriggerExit)
            PEngine.PostEvent(eventToPost.name, gameObject, totalListeners);
    }
}
