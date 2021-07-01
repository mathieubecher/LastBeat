using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Switch Group", menuName = "PampleEngine/New Switch Group")]
public class SwitchGroupSO : ScriptableObject
{
    public bool globalScope;
    public string[] switches;
}
