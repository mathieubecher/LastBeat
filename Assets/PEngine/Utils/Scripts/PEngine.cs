using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEditor;

public class PEngine : MonoBehaviour
{
    [SerializeField]
    //private GameObject audioSourcePrefab;
    //public EventSO[] eventsList;

    public static PEngine instance;

    private List<AudioSource> removeList = new List<AudioSource>();

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
        DontDestroyOnLoad(gameObject);

        FindEvents();
        FindPrefab();

        Debug.Log("Events found: " + GetEvent(0));
    }

    public static void PostEvent(string eventName, GameObject parent, List<PEngineListener> listeners)
    {
        EventSO foundEvent = Array.Find(m_Events, audioEvent => audioEvent.name == eventName);

        instance.PostEventCore(foundEvent, parent, listeners);
    }

    public static void PostEvent(EventSO pengineEvent, GameObject parent, List<PEngineListener> listeners)
    {
        instance.PostEventCore(pengineEvent, parent, listeners);
    }

    public static void PostEvent(string eventName, GameObject parent, PEngineListener[] listeners)
    {
        List<PEngineListener> listenersList = new List<PEngineListener>();

        foreach (PEngineListener listener in listeners)
            listenersList.Add(listener);

        EventSO foundEvent = Array.Find(m_Events, audioEvent => audioEvent.name == eventName);

        instance.PostEventCore(foundEvent, parent, listenersList);
    }

    public static void PostEvent(EventSO pengineEvent, GameObject parent, PEngineListener[] listeners)
    {
        List<PEngineListener> listenersList = new List<PEngineListener>();

        foreach (PEngineListener listener in listeners)
            listenersList.Add(listener);

        instance.PostEventCore(pengineEvent, parent, listenersList);
    }

    public static void PostEvent(EventSO pengineEvent, GameObject parent, int[] listenersID)
    {
        List<PEngineListener> listenersList = new List<PEngineListener>();

        foreach (int listenerID in listenersID)
            if (GetListener(listenerID))
                listenersList.Add(GetListener(listenerID));

        instance.PostEventCore(pengineEvent, parent, listenersList);
    }

    private void PostEventCore(EventSO pengineEvent, GameObject parent, List<PEngineListener> listeners)
    {
        if (!parent.GetComponent<PEngineObject>())
            parent.AddComponent<PEngineObject>();

        //Debug.Log("PEngine: " + pengineEvent.name + " event posted.");

        for (int i = 0; i < pengineEvent.steps.Length; i++)
        {
            EventStep currentStep = pengineEvent.steps[i];
            switch (currentStep.stepType)
            {
                case StepType.Play:
                    //Determining if it's a simple sound or a container
                    if (currentStep.soundToPlay.GetType().FullName == "SoundSO")
                    {
                        //Debug.Log("Playing Sound.");
                        PlaySound(currentStep, parent, listeners);
                    }
                    else if (currentStep.soundToPlay.GetType().FullName == "ContainerSO")
                    {
                        //Debug.Log("Playing Container.");
                        ContainerSO container = currentStep.soundToPlay as ContainerSO;
                        container.playing = true;
                        PlayContainer(currentStep, parent, listeners);
                    }
                    else
                        Debug.Log("Unknown sound type.");
                    break;

                case StepType.Stop:
                    //Determining if it's a simple sound or a container
                    if (currentStep.soundToStop.GetType().FullName == "SoundSO")
                    {
                        //Debug.Log("Stoping Sound.");
                        StopSound(currentStep, parent);
                    }
                    else if (currentStep.soundToStop.GetType().FullName == "ContainerSO")
                    {
                        //Debug.Log("Stoping Container.");
                        StopContainer(currentStep, parent);
                    }
                    else
                        Debug.Log("Unknown sound type.");
                    break;

                case StepType.ChangeSwitch:
                    break;

                case StepType.ChangeParameter:
                    SetParameterValue(currentStep.parameter, currentStep.valueToSet);
                    break;
            }
        }
    }

    private void PlaySound(EventStep currentStep, GameObject parent, List<PEngineListener> listeners)
    {
        SoundSO sound = currentStep.soundToPlay as SoundSO;

        //Debug.Log("Sound to post:" + sound);

        if (UnityEngine.Random.Range(0f, 1f) <= currentStep.playProbability)
        {
            if (sound.clip)
            {
                if (sound.customSpatialization)
                {
                    foreach (PEngineListener listener in listeners)
                        StartCoroutine(SourceInstantiation(parent, sound, sound, listener, currentStep));
                }
                else
                    StartCoroutine(SourceInstantiation(parent, sound, sound, null, currentStep));
            }
            else
                Debug.LogWarning("PEngine: Can't find any sound corresponding to the event.");
        }
    }

    private void StopSound(EventStep currentStep, GameObject parent)
    {
        SoundSO sound = currentStep.soundToStop as SoundSO;
        PEngineObject parentPEngineObject = parent.GetComponent<PEngineObject>();

        foreach (KeyValuePair<AudioSource, object> entry in parentPEngineObject.originObjectAndSources)
        {
            // do something with entry.Value or entry.Key
            if (entry.Value as SoundSO == sound)
            {
                //Debug.Log(entry.Value + " found. Stopping it.");
                if (currentStep.fadeOut)
                {
                    PEngineSource pEngineSource = entry.Key.gameObject.GetComponent<PEngineSource>();

                    pEngineSource.fadingIn = false;
                    StartCoroutine(DestroyDelay(currentStep.fadeOutDuration, entry.Key.gameObject));
                    StartCoroutine(pEngineSource.FadeCoroutine(pEngineSource.volumeFadeFactor, pEngineSource.volumeFadeFactor, 0, currentStep.fadeOutDuration, null));
                }
                else
                {
                    entry.Key.Stop();
                    Destroy(entry.Key.gameObject);
                }
                removeList.Add(entry.Key);
            }
        }

        foreach (AudioSource item in removeList)
        {
            parentPEngineObject.originObjectAndSources.Remove(item);
        }
        removeList.Clear();
    }

    private void PlayContainer(EventStep currentStep, GameObject parent, List<PEngineListener> listeners)
    {
        ContainerSO container = currentStep.soundToPlay as ContainerSO;
        SoundSO sound = container.GetSound(parent);

        if (UnityEngine.Random.Range(0f,1f) <= currentStep.playProbability)
        {
            if (sound.clip)
            {
                //Debug.Log("Get sound " + sound + " from a container of type " + container.type);
                if (sound.customSpatialization)
                {
                    foreach (PEngineListener listener in listeners)
                        StartCoroutine(SourceInstantiation(parent, container, sound, listener, currentStep));
                }
                else
                    StartCoroutine(SourceInstantiation(parent, container, sound, null, currentStep));

                switch (container.triggerMethod)
                {
                    case TriggerMethods.Step:
                        container.playing = false;
                        break;

                    case TriggerMethods.Delay:
                        StartCoroutine(WaitForEndOfClipDelay(sound.clip.length, container.delay, container, currentStep, parent, listeners));
                        break;

                    case TriggerMethods.TriggerRate:
                        StartCoroutine(SimplePlayDelay(container.delay, container, currentStep, parent, listeners)); //A interchanger avec triggerRate
                        break;
                }
            }
            else
                Debug.LogWarning("PEngine: Can't find any sound corresponding to the event.");
        }
    }

    private void StopContainer(EventStep currentStep, GameObject parent)
    {
        ContainerSO container = currentStep.soundToStop as ContainerSO;
        PEngineObject parentPEngineObject = parent.GetComponent<PEngineObject>();

        foreach (KeyValuePair<AudioSource, object> entry in parentPEngineObject.originObjectAndSources)
        {
            // do something with entry.Value or entry.Key
            if (entry.Value as ContainerSO == container)
            {
                //Debug.Log(entry.Value + " found. Stopping it.");
                entry.Key.Stop();
                Destroy(entry.Key.gameObject);
                //parentPEngineObject.originObjectAndSources.Remove(entry.Key);
                removeList.Add(entry.Key);
            }
        }

        foreach (AudioSource item in removeList)
        {
            parentPEngineObject.originObjectAndSources.Remove(item);
        }
        removeList.Clear();

        container.playing = false;
    }

    private void AssignSourceParameters(AudioSource audioSource, PEngineSource pEngineSource, SoundSO sound, PEngineListener listener)
    {
        audioSource.clip = sound.clip;
        audioSource.outputAudioMixerGroup = sound.bus;
        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.loop;
        audioSource.panStereo = sound.panning;
        
        audioSource.priority = sound.priority;
        audioSource.name = sound.name;

        if (sound.customSpatialization)
            audioSource.spatialBlend = 0;
        else
            audioSource.spatialBlend = sound.spatialBlend;

        pEngineSource.volumeValue = sound.volume;

        pEngineSource.volumeParameter = sound.volumeParameter;
        pEngineSource.setVolumeParameter = sound.setVolumeParameter;
        pEngineSource.volumeCurve = sound.volumeCurve;

        pEngineSource.pitchParameter = sound.pitchParameter;
        pEngineSource.setPitchParameter = sound.setPitchParameter;
        pEngineSource.pitchCurve = sound.pitchCurve;

        pEngineSource.panningParameter = sound.panningParameter;
        pEngineSource.setPanningParameter = sound.setPanningParameter;
        pEngineSource.panningCurve = sound.panningCurve;
        
        pEngineSource.spatialBlendParameter = sound.spatialBlendParameter;
        pEngineSource.setSpatialBlendParameter = sound.setSpatialBlendParameter;
        pEngineSource.spatialBlendCurve = sound.spatialBlendCurve;

        pEngineSource.priorityParameter = sound.priorityParameter;
        pEngineSource.setPriorityParameter = sound.setPriorityParameter;
        pEngineSource.priorityCurve = sound.priorityCurve;

        pEngineSource.customSpatialization = sound.customSpatialization;
        pEngineSource.bypassPanning = sound.bypassPanning;
        pEngineSource.maxAttDistance = sound.maxAttenuationDistance;
        pEngineSource.attenuationCurve = sound.attenuationCurve;

        pEngineSource.listener = listener;

        pEngineSource.EventsSuscribing();
    }

    IEnumerator SourceInstantiation(GameObject parent, object source, SoundSO sound, PEngineListener listener, EventStep step)
    {
        yield return new WaitForSeconds(step.playInitialDelay);

        GameObject sourceGO = Instantiate(m_AudioSource, parent.transform);
        AudioSource audioSource = sourceGO.GetComponent<AudioSource>();

        PEngineSource pEngineSource = sourceGO.GetComponent<PEngineSource>();
        parent.GetComponent<PEngineObject>().originObjectAndSources.Add(audioSource, source);

        //Parameters assignment
        AssignSourceParameters(audioSource, pEngineSource, sound, listener);

        if (step.fadeIn)
        {
            pEngineSource.fadingIn = true;
            StartCoroutine(pEngineSource.FadeCoroutine(0, 0, 1, step.fadeInDuration, null));
        }
        else
            pEngineSource.volumeFadeFactor = 1;

        StartCoroutine(DestroyDelay(sound.clip.length, sourceGO));
    }

    IEnumerator DestroyDelay(float delay, GameObject audioSource)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource)
        {
            audioSource.GetComponent<AudioSource>().Stop();
            Destroy(audioSource.gameObject);
        }
    }

    IEnumerator WaitForEndOfClipDelay(float clipLength, float delay, ContainerSO container, EventStep currentStep, GameObject parent, List<PEngineListener> listeners)
    {
        yield return new WaitForSeconds(delay);
        if (container.playing)
            StartCoroutine(SimplePlayDelay(delay, container, currentStep, parent, listeners));
    }

    IEnumerator SimplePlayDelay(float delay, ContainerSO container, EventStep currentStep, GameObject parent, List<PEngineListener> listeners)
    {
        yield return new WaitForSeconds(delay);
        if (container.playing)
            PlayContainer(currentStep, parent, listeners);
    }

    public static void SetSwitch(SwitchGroupSO switchGroup, string switchToSet, GameObject target)
    {
        SetSwitch[] detectedSwitches = target.GetComponents<SetSwitch>();
        bool switchAlreadyThere = false;

        foreach (SetSwitch setSwitch in detectedSwitches)
        {
            if (setSwitch.switchGroup == switchGroup)
            {
                switchAlreadyThere = true;
                setSwitch.switchToSet = switchToSet;
                setSwitch.switchIndex = Array.IndexOf(setSwitch.switchGroup.switches, switchToSet);
            }
        }

        if (switchAlreadyThere)
            return;
        else
        {
            SetSwitch newSwitch = target.AddComponent<SetSwitch>();
            newSwitch.switchGroup = switchGroup;
            newSwitch.switchToSet = switchToSet;
            newSwitch.switchIndex = Array.IndexOf(newSwitch.switchGroup.switches, switchToSet);
        }
    }

    public static void SetParameter(ParameterSO parameter, float in_value)
    {
        instance.SetParameterValue(parameter, in_value);
    }

    public void SetParameterValue(ParameterSO parameter, float in_value)
    {
        parameter.targetValue = in_value;

        if (parameter.interpolation && !parameter.valueChanging)
        {
            parameter.valueChanging = true;
            StartCoroutine(RTPC_Interpolation(parameter));
        }
        else
        {
            parameter.currentValue = parameter.targetValue;
            parameter.OnValueChange?.Invoke(this, parameter.currentValue);
        }
    }

    IEnumerator RTPC_Interpolation(ParameterSO parameter)
    {
        yield return new WaitForEndOfFrame();

        if (parameter.currentValue < parameter.targetValue)
        {
            parameter.currentValue = Mathf.Clamp(parameter.currentValue + parameter.changePerSec * Time.deltaTime, parameter.minimumValue, parameter.maximumValue);
            
            if (parameter.currentValue >= parameter.targetValue)
            {
                parameter.currentValue = parameter.targetValue;
                parameter.valueChanging = false;
            }
            else
                StartCoroutine(RTPC_Interpolation(parameter));
        }
        else
        {
            parameter.currentValue = Mathf.Clamp(parameter.currentValue - parameter.changePerSec * Time.deltaTime, parameter.minimumValue, parameter.maximumValue);
            
            if (parameter.currentValue <= parameter.targetValue)
            {
                parameter.currentValue = parameter.targetValue;
                parameter.valueChanging = false;
            }
            else
                StartCoroutine(RTPC_Interpolation(parameter));
        }
        parameter.OnValueChange?.Invoke(this, parameter.currentValue);
    }



    private static EventSO[] m_Events;
    private static GameObject m_AudioSource;
    private static Dictionary<int, PEngineListener> m_Listeners = new Dictionary<int, PEngineListener>();

#if UNITY_EDITOR
    [ContextMenu("Find Events")]
    public static void FindEvents()
    {
        List<EventSO> lEvents = new List<EventSO>();

        string[] lGuids = AssetDatabase.FindAssets("t:EventSO", new string[] { "Assets/PEngine/Events" });

        for (int i = 0; i < lGuids.Length; i++)
        {
            string lAssetPath = AssetDatabase.GUIDToAssetPath(lGuids[i]);
            lEvents.Add(AssetDatabase.LoadAssetAtPath<EventSO>(lAssetPath));
        }

        m_Events = lEvents.ToArray();
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Find AudioSource Prefab")]
    private static void FindPrefab()
    {
        List<GameObject> gameObjects = new List<GameObject>();

        string[] prefabs = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/PEngine/Utils/Prefabs" });

        for (int i = 0; i < prefabs.Length; i++)
        {
            string lAssetPath = AssetDatabase.GUIDToAssetPath(prefabs[i]);
            gameObjects.Add(AssetDatabase.LoadAssetAtPath<GameObject>(lAssetPath));
        }
        m_AudioSource = Array.Find(gameObjects.ToArray(), audioSource => audioSource.name == "AudioSource");
    }
#endif

    public static EventSO GetEvent(int eventIndex)
    {
        return (EventSO)m_Events.GetValue(eventIndex);
    }

    public static void AddListener(PEngineListener listener, int listenerID)
    {
        PEngineListener listenerToCheck;

        if (m_Listeners.TryGetValue(listenerID, out listenerToCheck))
            Debug.LogError("PEngine: A listener already got this index.");
        else
            m_Listeners.Add(listenerID, listener);
    }

    public static void RemoveListener(PEngineListener listener)
    {
        foreach (KeyValuePair<int, PEngineListener> entry in m_Listeners)
        {
            // do something with entry.Value or entry.Key
            if (entry.Value == listener)
                m_Listeners.Remove(entry.Key);
        }
    }

    public static void RemoveListener(int listenerID) => m_Listeners.Remove(listenerID);

    public static PEngineListener GetListener(int listenerID)
    {
        PEngineListener listenerToReturn;

        m_Listeners.TryGetValue(listenerID, out listenerToReturn);

        if (listenerToReturn)
            return listenerToReturn;
        else
        {
            //Debug.Log("Listener not found for index " + listenerID);
            return null;
        }
    }
}
