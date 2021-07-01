using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEngineObject : MonoBehaviour
{
    //public List<AudioSource> audioSources = new List<AudioSource>();
    public Dictionary<AudioSource, object> originObjectAndSources = new Dictionary<AudioSource, object>();
}
