using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundSO))]
public class SoundSOInspector : Editor
{
    //SerializedProperty bus;
    private SoundSO soundSO;
    private SerializedObject serializedSoundSO;
    
    public SerializedProperty clip;

    public SerializedProperty bus;
    
    public SerializedProperty volume;
    public SerializedProperty setVolumeParameter;
    public SerializedProperty volumeParameter;
    public SerializedProperty volumeCurve;

    public SerializedProperty pitch;
    public SerializedProperty setPitchParameter;
    public SerializedProperty pitchParameter;
    public SerializedProperty pitchCurve;

    public SerializedProperty loop;

    public SerializedProperty panning;
    public SerializedProperty setPanningParameter;
    public SerializedProperty panningParameter;
    public SerializedProperty panningCurve;

    public SerializedProperty spatialBlend;
    public SerializedProperty setSpatialBlendParameter;
    public SerializedProperty spatialBlendParameter;
    public SerializedProperty spatialBlendCurve;

    public SerializedProperty priority;
    public SerializedProperty setPriorityParameter;
    public SerializedProperty priorityParameter;
    public SerializedProperty priorityCurve;

    public SerializedProperty customSpatialization;
    public SerializedProperty bypassPanning;

    public SerializedProperty maxAttenuationDistance;
    public SerializedProperty attenuationCurve;

    private void OnEnable()
    {
        soundSO = target as SoundSO;
        serializedSoundSO = new SerializedObject(target);

        clip = serializedSoundSO.FindProperty("clip");

        bus = serializedSoundSO.FindProperty("bus");

        volume = serializedSoundSO.FindProperty("volume");
        setVolumeParameter = serializedSoundSO.FindProperty("setVolumeParameter");
        volumeParameter = serializedSoundSO.FindProperty("volumeParameter");
        volumeCurve = serializedSoundSO.FindProperty("volumeCurve");

        pitch = serializedSoundSO.FindProperty("pitch");
        setPitchParameter = serializedSoundSO.FindProperty("setPitchParameter");
        pitchParameter = serializedSoundSO.FindProperty("pitchParameter");
        pitchCurve = serializedSoundSO.FindProperty("pitchCurve");

        loop = serializedSoundSO.FindProperty("loop");

        panning = serializedSoundSO.FindProperty("panning");
        setPanningParameter = serializedSoundSO.FindProperty("setPanningParameter");
        panningParameter = serializedSoundSO.FindProperty("panningParameter");
        panningCurve = serializedSoundSO.FindProperty("panningCurve");

        spatialBlend = serializedSoundSO.FindProperty("spatialBlend");
        setSpatialBlendParameter = serializedSoundSO.FindProperty("setSpatialBlendParameter");
        spatialBlendParameter = serializedSoundSO.FindProperty("spatialBlendParameter");
        spatialBlendCurve = serializedSoundSO.FindProperty("spatialBlendCurve");

        priority = serializedSoundSO.FindProperty("priority");
        setPriorityParameter = serializedSoundSO.FindProperty("setPriorityParameter");
        priorityParameter = serializedSoundSO.FindProperty("priorityParameter");
        priorityCurve = serializedSoundSO.FindProperty("priorityCurve");

        customSpatialization = serializedSoundSO.FindProperty("customSpatialization");
        bypassPanning = serializedSoundSO.FindProperty("bypassPanning");

        maxAttenuationDistance = serializedSoundSO.FindProperty("maxAttenuationDistance");
        attenuationCurve = serializedSoundSO.FindProperty("attenuationCurve");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(clip);

        EditorGUILayout.PropertyField(bus);

        EditorGUILayout.PropertyField(volume);
        EditorGUILayout.PropertyField(setVolumeParameter);

        if (soundSO.setVolumeParameter)
        {
            EditorGUILayout.PropertyField(volumeParameter);
            if (soundSO.volumeParameter)
                EditorGUILayout.PropertyField(volumeCurve);
        }
        
        EditorGUILayout.PropertyField(pitch);
        EditorGUILayout.PropertyField(setPitchParameter);

        if (soundSO.setPitchParameter)
        {
            EditorGUILayout.PropertyField(pitchParameter);
            if (soundSO.pitchParameter)
                EditorGUILayout.PropertyField(pitchCurve);
        }
        
        EditorGUILayout.PropertyField(loop);

        EditorGUILayout.PropertyField(panning);
        EditorGUILayout.PropertyField(setPanningParameter);

        if (soundSO.setPanningParameter)
        {
            EditorGUILayout.PropertyField(panningParameter);
            if (soundSO.panningParameter)
                EditorGUILayout.PropertyField(panningCurve);
        }

        EditorGUILayout.PropertyField(priority);
        EditorGUILayout.PropertyField(setPriorityParameter);

        if (soundSO.setPriorityParameter)
        {
            EditorGUILayout.PropertyField(priorityParameter);
            if (soundSO.priorityParameter)
                EditorGUILayout.PropertyField(priorityCurve);
        }
        
        EditorGUILayout.PropertyField(customSpatialization);

        if (soundSO.customSpatialization)
            EditorGUILayout.PropertyField(bypassPanning);
        else
        {
            EditorGUILayout.PropertyField(spatialBlend);
            EditorGUILayout.PropertyField(setSpatialBlendParameter);

            if (soundSO.setSpatialBlendParameter)
            {
                EditorGUILayout.PropertyField(spatialBlendParameter);
                if (soundSO.spatialBlendParameter)
                    EditorGUILayout.PropertyField(spatialBlendCurve);
            }
        }

        EditorGUILayout.PropertyField(maxAttenuationDistance);
        EditorGUILayout.PropertyField(attenuationCurve);

        if (soundSO.clip)
        {
            soundSO.name = soundSO.clip.name;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(soundSO), soundSO.clip.name);
        }

        if (EditorGUI.EndChangeCheck())
            serializedSoundSO.ApplyModifiedProperties();
    }
}
