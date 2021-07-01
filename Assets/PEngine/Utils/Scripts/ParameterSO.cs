using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Parameter", menuName = "PampleEngine/New Parameter")]
public class ParameterSO : ScriptableObject
{
    public float minimumValue;
    public float maximumValue;
    public float defaultValue;

    [Space]
    public bool interpolation;
    public float changePerSec = 100;

    [Space]
    public float currentValue = 0;
    [HideInInspector]
    public float targetValue;
    [HideInInspector]
    public bool valueChanging;

    public EventHandler<float> OnValueChange;

    private void OnEnable()
    {
        currentValue = defaultValue;
        valueChanging = false;
        if (changePerSec == 0)
            changePerSec = 0.1f;
    }
}
