using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSwitch : MonoBehaviour
{
    public SwitchGroupSO switchGroup;
    [HideInInspector]
    public string switchToSet;
    [HideInInspector]
    public int switchIndex;
}
