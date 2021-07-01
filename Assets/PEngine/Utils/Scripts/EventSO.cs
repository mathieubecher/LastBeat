using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Event", menuName = "PampleEngine/New Event")]
public class EventSO : ScriptableObject
{
    public EventStep[] steps;
}
