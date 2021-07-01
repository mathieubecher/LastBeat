using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEngineListener : MonoBehaviour
{
    [Header("Put -1 if you don't want to register this listener")]
    public int listenerID = -1;
    [Range(-2,2)]
    public float panningOffset;

    private void Awake()
    {
        if (listenerID == -1) return;
        else PEngine.AddListener(this, listenerID);
    }
}
