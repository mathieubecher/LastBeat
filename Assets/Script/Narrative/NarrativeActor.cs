using System.Collections;
using System.Collections.Generic;
using NarrativeSystem;
using UnityEngine;

public class NarrativeActor : MonoBehaviour
{
    public string id;

    public void Goto(float duration)
    {
        
    }

    public float PlaySource(string source, PlayingType type)
    {
        Debug.LogFormat("Play " + source + " FMod");
        return 3;
    }

    public void SetParameter(string parameter, float value, Target target, string eventFMod = "")
    {
        Debug.LogFormat("Play " + parameter + " FMod");
        
    }

}
