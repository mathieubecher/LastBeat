using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PEngineSource : MonoBehaviour
{
    private AudioSource audioSource;


    [HideInInspector] public float volumeValue;
    [HideInInspector] public float volumeRTPC;
    [HideInInspector] public float volumeFadeFactor;
    [HideInInspector] public bool setVolumeParameter;
    [HideInInspector] public ParameterSO volumeParameter;
    public AnimationCurve volumeCurve;

    [HideInInspector] public float pitchValue;
    [HideInInspector] public bool setPitchParameter;
    [HideInInspector] public ParameterSO pitchParameter;
    [HideInInspector] public AnimationCurve pitchCurve;
    [HideInInspector] public float pitchRTPC;

    [HideInInspector] public float panningRTPC;
    [HideInInspector] public float panningValue;
    [HideInInspector] public bool setPanningParameter;
    [HideInInspector] public ParameterSO panningParameter;
    [HideInInspector] public AnimationCurve panningCurve;

    [HideInInspector] public float spatialBlendRTPC;
    [HideInInspector] public float spatialBlendValue;
    [HideInInspector] public bool setSpatialBlendParameter;
    [HideInInspector] public ParameterSO spatialBlendParameter;
    [HideInInspector] public AnimationCurve spatialBlendCurve;

    [HideInInspector] public float priorityRTPC;
    [HideInInspector] public float priorityValue;
    [HideInInspector] public bool setPriorityParameter;
    [HideInInspector] public ParameterSO priorityParameter;
    [HideInInspector] public AnimationCurve priorityCurve;

    [HideInInspector] public PEngineListener listener;

    [HideInInspector] public bool customSpatialization;
    [HideInInspector] public bool bypassPanning;
    [HideInInspector] public float maxAttDistance;
    [HideInInspector] public AnimationCurve attenuationCurve;

    [HideInInspector] public bool fadingIn;

    public void EventsSuscribing()
    {
        if (volumeParameter)
            volumeParameter.OnValueChange += OnVolumeParameterChange;

        if (pitchParameter)
            pitchParameter.OnValueChange += OnPitchParameterChange;

        if (panningParameter)
            panningParameter.OnValueChange += OnPanningParameterChange;

        if (spatialBlendParameter)
            spatialBlendParameter.OnValueChange += OnSpatialBlendParameterChange;

        if (priorityParameter)
            priorityParameter.OnValueChange += OnPriorityParameterChange;
    }

    private void OnDisable()
    {
        if (volumeParameter)
            volumeParameter.OnValueChange -= OnVolumeParameterChange;

        if (pitchParameter)
            pitchParameter.OnValueChange -= OnPitchParameterChange;

        if (panningParameter)
            panningParameter.OnValueChange -= OnPanningParameterChange;

        if (spatialBlendParameter)
            spatialBlendParameter.OnValueChange -= OnSpatialBlendParameterChange;

        if (priorityParameter)
            priorityParameter.OnValueChange -= OnPriorityParameterChange;
    }

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.bypassListenerEffects = true;
    }

    private void Start()
    {
        //INIT WITH RTPC
        if (setVolumeParameter && volumeParameter)
            volumeRTPC = volumeCurve.Evaluate((volumeParameter.currentValue - volumeParameter.minimumValue) / (volumeParameter.maximumValue - volumeParameter.minimumValue));

        pitchValue = audioSource.pitch;

        if (setPitchParameter && pitchParameter)
        {
            pitchRTPC = pitchCurve.Evaluate((pitchParameter.currentValue - pitchParameter.minimumValue) / (pitchParameter.maximumValue - pitchParameter.minimumValue));
            pitchRTPC = (pitchRTPC * 6) -3;
            Debug.Log("pitchRTPC = " + pitchRTPC);
            audioSource.pitch = Mathf.Clamp(pitchValue + pitchRTPC, -3, 3);
        }

        panningValue = audioSource.panStereo;

        if (setPanningParameter && panningParameter)
        {
            panningRTPC = panningCurve.Evaluate((panningParameter.currentValue - panningParameter.minimumValue) / (panningParameter.maximumValue - panningParameter.minimumValue));
            panningRTPC = (panningRTPC * 2) - 1;
            Debug.Log("panningRTPC = " + panningRTPC);
        }
        
        spatialBlendValue = audioSource.spatialBlend;

        if (setSpatialBlendParameter && spatialBlendParameter)
        {
            spatialBlendRTPC = spatialBlendCurve.Evaluate((spatialBlendParameter.currentValue - spatialBlendParameter.minimumValue) / (spatialBlendParameter.maximumValue - spatialBlendParameter.minimumValue));
            Debug.Log("spatialBlendRTPC = " + spatialBlendRTPC);
        }

        priorityValue = audioSource.priority;

        if (setPriorityParameter && priorityParameter)
        {
            priorityRTPC = priorityCurve.Evaluate((priorityParameter.currentValue - priorityParameter.minimumValue) / (priorityParameter.maximumValue - priorityParameter.minimumValue));
            priorityRTPC = Mathf.RoundToInt(priorityRTPC * 256);
            Debug.Log("priorityRTPC: " + priorityRTPC);
        }

        if (customSpatialization)
        {
            UpdateVolumeAttenuation();
            UpdatePan();
        }
        else
        {
            if (attenuationCurve.length != 0)
                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, attenuationCurve);
            audioSource.maxDistance = maxAttDistance;

            if (setVolumeParameter && volumeParameter)
                audioSource.volume = volumeValue * volumeRTPC * volumeFadeFactor;
            else
                audioSource.volume = volumeValue * volumeFadeFactor;
        }
        
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void Update()
    {
        if (customSpatialization)
        {
            UpdateVolumeAttenuation();
            UpdatePan();
        }
        //UPDATING WITH RTPC
        else if (setVolumeParameter && volumeParameter)
            audioSource.volume = volumeValue * volumeRTPC * volumeFadeFactor;
        else
            audioSource.volume = volumeValue * volumeFadeFactor;

        if (setPitchParameter && pitchParameter)
        {
            audioSource.pitch = Mathf.Clamp(pitchValue + pitchRTPC, -3, 3);
            pitchValue = audioSource.pitch - pitchRTPC;
        }

        if (setPanningParameter && panningParameter)
        {
            audioSource.panStereo = Mathf.Clamp(panningValue + panningRTPC, -1, 1);
            panningValue = audioSource.panStereo - panningRTPC;
        }

        if (setSpatialBlendParameter && spatialBlendParameter)
        {
            audioSource.spatialBlend = Mathf.Clamp(spatialBlendValue + spatialBlendRTPC, 0, 1);
            spatialBlendValue = audioSource.spatialBlend - spatialBlendRTPC;
        }

        if (setPriorityParameter && priorityParameter)
        {
            audioSource.priority = Mathf.Clamp(Mathf.RoundToInt(priorityValue + priorityRTPC), 0, 256);
            pitchValue = audioSource.priority - priorityRTPC;
        }
    }

    private void UpdateVolumeAttenuation()
    {
        float volumeFactor = 1;
        float dist = Vector3.Distance(gameObject.transform.position, listener.transform.position);
        
        float distRatio = dist / maxAttDistance;
        
        volumeFactor = attenuationCurve.Evaluate(distRatio);

        if (setVolumeParameter && volumeParameter)
        {
            audioSource.volume = volumeValue * volumeFactor * volumeRTPC * volumeFadeFactor;
        }
        else
            audioSource.volume = volumeValue * volumeFactor * volumeFadeFactor;
    }

    private void UpdatePan()
    {
        if (bypassPanning)
            audioSource.panStereo = Mathf.Clamp(panningValue + listener.panningOffset, -1, 1);
        else
        {
            //float newPan = Mathf.Atan2(gameObject.transform.position, listener.transform.position) / 180;
            Vector3 dir = gameObject.transform.position - listener.transform.position;
            float angle = Vector3.SignedAngle(listener.transform.forward, dir, Vector3.up);

            if (angle < -90)
                angle = -1 * (angle + 180);
            else if (angle > 90)
                angle = -1 * (angle - 180);

            //Debug.Log("angle: " + angle);

            float newPan = angle / 90;

            audioSource.panStereo = Mathf.Clamp(newPan + listener.panningOffset, -1, 1);
        }
            
    }


    private void OnVolumeParameterChange(object sender, float in_value)
    {
        //volume = (in_value - volumeParameter.minimumValue) / (volumeParameter.maximumValue - volumeParameter.minimumValue);
        volumeRTPC = volumeCurve.Evaluate((in_value - volumeParameter.minimumValue) / (volumeParameter.maximumValue - volumeParameter.minimumValue));  
        Debug.Log("volumeRTPC: " + volumeRTPC);
    }

    private void OnPitchParameterChange(object sender, float in_value)
    {
        pitchRTPC = pitchCurve.Evaluate((in_value - pitchParameter.minimumValue) / (pitchParameter.maximumValue - pitchParameter.minimumValue));
        pitchRTPC = (pitchRTPC * 6) - 3;
        Debug.Log("pitchRTPC: " + pitchRTPC);
    }

    private void OnPanningParameterChange(object sender, float in_value)
    {
        panningRTPC = panningCurve.Evaluate((in_value - panningParameter.minimumValue) / (panningParameter.maximumValue - panningParameter.minimumValue));
        panningRTPC = (panningRTPC * 2) - 1;
        Debug.Log("panningRTPC: " + panningRTPC);
    }

    private void OnSpatialBlendParameterChange(object sender, float in_value)
    {
        spatialBlendRTPC = spatialBlendCurve.Evaluate((in_value - spatialBlendParameter.minimumValue) / (spatialBlendParameter.maximumValue - spatialBlendParameter.minimumValue));
        Debug.Log("spatialBlendRTPC: " + spatialBlendRTPC);
    }

    private void OnPriorityParameterChange(object sender, float in_value)
    {
        priorityRTPC = priorityCurve.Evaluate((in_value - priorityParameter.minimumValue) / (priorityParameter.maximumValue - priorityParameter.minimumValue));
        priorityRTPC = Mathf.RoundToInt(priorityRTPC * 256);
        Debug.Log("priorityRTPC: " + priorityRTPC);
    }

    public IEnumerator FadeCoroutine(float currentValue, float startValue, float endValue, float duration, AnimationCurve curve)
    {
        volumeFadeFactor = currentValue;

        yield return new WaitForEndOfFrame();

        if (currentValue < endValue && fadingIn)
        {
            currentValue += (endValue - startValue) * (Time.deltaTime / duration);

            if (currentValue >= endValue)
                currentValue = endValue;
            else
                StartCoroutine(FadeCoroutine(currentValue, startValue, endValue, duration, curve));
        }
        else if (!fadingIn)
        {
            currentValue -= (startValue - endValue) * (Time.deltaTime / duration);

            if (currentValue <= endValue)
                currentValue = endValue;
            else
                StartCoroutine(FadeCoroutine(currentValue, startValue, endValue, duration, curve));
        }

        volumeFadeFactor = currentValue;
    }
}
