using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParameter : MonoBehaviour
{
    public ParameterSO parameter;
    public float valueToSet;
    public PostMethods postMethod;
    
    void Start()
    {
        if (postMethod == PostMethods.OnStart)
            PEngine.instance.SetParameterValue(parameter, valueToSet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (postMethod == PostMethods.OnTriggerEnter)
            PEngine.instance.SetParameterValue(parameter, valueToSet);
    }

    private void OnTriggerExit(Collider other)
    {
        if (postMethod == PostMethods.OnTriggerExit)
            PEngine.instance.SetParameterValue(parameter, valueToSet);
    }
}
