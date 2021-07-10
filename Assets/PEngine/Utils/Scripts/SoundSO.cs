using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public enum AttenuationType
{
    Linear,
    Exp,
}

[CreateAssetMenu(fileName = "New Sound", menuName = "PampleEngine/New Sound")]
public class SoundSO : ScriptableObject
{
    [Space]
    public AudioClip clip;

    [Space]
    public AudioMixerGroup bus;

    [Space]
    [Range(0, 1)]
    public float volume = 1;
    public bool setVolumeParameter;
    public ParameterSO volumeParameter;
    public AnimationCurve volumeCurve;

    [Space]
    [Range(-3, 3)]
    public float pitch = 1;
    public bool setPitchParameter;
    public ParameterSO pitchParameter;
    public AnimationCurve pitchCurve;

    [Space]
    public bool loop;

    [Space]
    [Range(-1, 1)]
    public float panning = 0;
    public bool setPanningParameter;
    public ParameterSO panningParameter;
    public AnimationCurve panningCurve;

    [Space]
    [Header("0 = 2D, 1 = 3D")]
    [Range(0,1)]
    public float spatialBlend = 0;
    public bool setSpatialBlendParameter;
    public ParameterSO spatialBlendParameter;
    public AnimationCurve spatialBlendCurve;

    [Space]
    [Range(0,256)]
    public int priority = 128;
    public bool setPriorityParameter;
    public ParameterSO priorityParameter;
    public AnimationCurve priorityCurve;

    [Space]
    public bool customSpatialization;
    public bool bypassPanning;

    [Space]
    public float maxAttenuationDistance;
    public AnimationCurve attenuationCurve;
}
