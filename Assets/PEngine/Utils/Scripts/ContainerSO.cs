using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEditor;

public enum ContainerType
{
    Switch,
    Random,
    Sequence,
    Blend,
}

public enum TriggerMethods
{
    Step,
    Delay,
    TriggerRate,
}

[CreateAssetMenu(fileName = "New Container", menuName = "PampleEngine/New Container")]
public class ContainerSO : ScriptableObject
{
    public SoundSO[] sounds;
    public SwitchGroupSO switchGroup;
    public List<SwitchSounds> switchSoundsList = new List<SwitchSounds>();

    [Header("Container Parameters")]
    public ContainerType type;
    public TriggerMethods triggerMethod;
    public float delay;

    [Header("Sounds Parameters")]
    [Range(-6, 6)]
    public float pitchOffset = 0;
    [Range(-2, 2)]
    public float panningOffset = 0;
    [Range(-256, 256)]
    public float priorityOffset = 0;

    [Space]
    public bool overrideBus;
    public AudioMixerGroup bus;
    public bool overrideLoop;
    public bool loop;
    public bool overrideSpatialBlend;
    [Range(0, 1)]
    public float spatialBlend = 0;

    private int playingIndex = 0;
    [HideInInspector]
    public bool playing;

    private SwitchGroupSO currentSwitchGroup = null;
    private SwitchGroupSO formerSwitchGroup = null;

    /*
    private void OnEnable()
    {
        if (switchGroup && !EditorApplication.isPlaying)
        {
            switchSoundsList.Clear();
            for (int i = 0; i < switchGroup.switches.Length; i++)
            {
                SwitchSounds switchSoundsToAdd = new SwitchSounds();
                switchSoundsToAdd.switchName = switchGroup.switches[i];
                switchSoundsList.Add(switchSoundsToAdd);
            }
            Debug.Log("Update switch sounds list from enable");
        }
    }
    */

    public void UpdateSwitchSoundsList()
    {
        if (switchGroup && !EditorApplication.isPlaying)
        {
            switchSoundsList.Clear();
            for (int i = 0; i < switchGroup.switches.Length; i++)
            {
                SwitchSounds switchSoundsToAdd = new SwitchSounds();
                switchSoundsToAdd.switchName = switchGroup.switches[i];
                switchSoundsList.Add(switchSoundsToAdd);
            }
            Debug.Log("Update switch sounds list from InspectorGUI");
        }
    }

    public SoundSO GetSound(GameObject parent)
    {
        SoundSO sound = null;
        switch (type)
        {
            case ContainerType.Random:
                sound = sounds[Mathf.RoundToInt(Random.Range(0, sounds.Length))];
                break;

            case ContainerType.Sequence:
                sound = sounds[playingIndex];
                playingIndex += 1;
                if (playingIndex > sounds.Length)
                    playingIndex = 0;
                break;

            case ContainerType.Switch:
                GameObject switchGO;

                if (switchGroup.globalScope)
                    switchGO = PEngine.instance.gameObject;
                else
                    switchGO = parent;

                int _switchIndex = 0;
                SetSwitch switchGroupToLook;
                SetSwitch[] totalSwitchGroups = switchGO.GetComponents<SetSwitch>();

                if (totalSwitchGroups.Length == 0)
                {
                    if (switchGroup.globalScope)
                        Debug.LogWarning("PEngine: Event requiring switch: " + switchGroup.name + " but didn't find one set on the AudioManager.");
                    else
                        Debug.LogWarning("PEngine: Event requiring switch: " + switchGroup.name + " but didn't find one set on the emitter.");
                    return null;
                }

                foreach (SetSwitch setSwitch in totalSwitchGroups)
                {
                    if (setSwitch.switchGroup == switchGroup)
                    {
                        switchGroupToLook = setSwitch;
                        _switchIndex = setSwitch.switchIndex;
                    }
                }

                if (switchSoundsList[_switchIndex].soundsList.Length != 1)
                    sound = switchSoundsList[_switchIndex].soundsList[Mathf.RoundToInt(Random.Range(0, sounds.Length))];
                else
                    sound = switchSoundsList[_switchIndex].soundsList[0];
                //Debug.Log("Get list of " + switchSoundsList[_switchIndex].switchName);
                break;

            case ContainerType.Blend:
                break;
        }
        return sound;
    }
}
