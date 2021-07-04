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

    public void PlaySource(string source, PlayingType type)
    {
        Debug.LogFormat("Play " + source + " FMod");
    }

    public void PlaySourcePEngine(EventSO source, PlayingType type)
    {
        Debug.LogFormat("Play " + source + " PEngine");
    
    }

    public void SetParameter(string parameter, float value, Target target, string eventFMod = "")
    {
        Debug.LogFormat("Play " + parameter + " FMod");
        
    }

    public void SetParameterPEngine(EventSO parameter, float value, Target target)
    {
        Debug.LogFormat("Play " + parameter + " PEngine");
    
    }
}
