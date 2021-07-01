using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StepType
{
    Play,
    Stop,
    ChangeSwitch,
    ChangeParameter,
}

[System.Serializable]
public class EventStep
{
    public StepType stepType;
    
    //Play Parameters
    public ScriptableObject soundToPlay;
    public float playInitialDelay;
    public float playProbability = 1;
    public bool fadeIn;
    public float fadeInDuration;

    //Stop parameters
    public ScriptableObject soundToStop;
    public float stopInitialDelay;
    public float stopProbability = 1;
    public bool fadeOut;
    public float fadeOutDuration;

    //Switch parameters
    public SwitchGroupSO switchGroup;
    public string switchToSet;

    //RTPC parameters
    public ParameterSO parameter;
    public float valueToSet;
}
