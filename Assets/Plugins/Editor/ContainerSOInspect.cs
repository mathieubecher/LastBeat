using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ContainerSO))]
public class ContainerSOInspect : Editor
{
    //SerializedProperty bus;
    private ContainerSO containerSO;

    private SerializedObject serializedContainerSO;

    private SerializedProperty sounds;
    private SerializedProperty switchGroup;
    private SerializedProperty switchSoundsList;

    private SerializedProperty type;
    private SerializedProperty delay;
    
    private SerializedProperty triggerMethod;
    private SerializedProperty overrideBus;
    private SerializedProperty bus;
    private SerializedProperty pitchOffset;
    private SerializedProperty overrideLoop;
    private SerializedProperty loop;
    private SerializedProperty panningOffset;
    private SerializedProperty overrideSpatialBlend;
    private SerializedProperty spatialBlend;
    private SerializedProperty priorityOffset;

    private SwitchGroupSO currentSwitchGroup = null;
    private SwitchGroupSO formerSwitchGroup = null;
    
    private void OnEnable()
    {
        containerSO = target as ContainerSO;
        serializedContainerSO = new SerializedObject(target);

        sounds = serializedContainerSO.FindProperty("sounds");
        switchSoundsList = serializedContainerSO.FindProperty("switchSoundsList");

        type = serializedContainerSO.FindProperty("type");
        delay = serializedContainerSO.FindProperty("delay");
        
        triggerMethod = serializedContainerSO.FindProperty("triggerMethod");
        overrideBus = serializedContainerSO.FindProperty("overrideBus");
        bus = serializedContainerSO.FindProperty("bus");
        pitchOffset = serializedContainerSO.FindProperty("pitchOffset");
        overrideLoop = serializedContainerSO.FindProperty("overrideLoop");
        loop = serializedContainerSO.FindProperty("loop");
        panningOffset = serializedContainerSO.FindProperty("panningOffset");
        overrideSpatialBlend = serializedContainerSO.FindProperty("overrideSpatialBlend");
        spatialBlend = serializedContainerSO.FindProperty("spatialBlend");
        priorityOffset = serializedContainerSO.FindProperty("priorityOffset");
    }

    private void UpdateProperty()
    {
        containerSO = target as ContainerSO;
        serializedContainerSO = new SerializedObject(target);

        sounds = serializedContainerSO.FindProperty("sounds");
        switchSoundsList = serializedContainerSO.FindProperty("switchSoundsList");

        type = serializedContainerSO.FindProperty("type");
        delay = serializedContainerSO.FindProperty("delay");

        triggerMethod = serializedContainerSO.FindProperty("triggerMethod");
        overrideBus = serializedContainerSO.FindProperty("overrideBus");
        bus = serializedContainerSO.FindProperty("bus");
        pitchOffset = serializedContainerSO.FindProperty("pitchOffset");
        overrideLoop = serializedContainerSO.FindProperty("overrideLoop");
        loop = serializedContainerSO.FindProperty("loop");
        panningOffset = serializedContainerSO.FindProperty("panningOffset");
        overrideSpatialBlend = serializedContainerSO.FindProperty("overrideSpatialBlend");
        spatialBlend = serializedContainerSO.FindProperty("spatialBlend");
        priorityOffset = serializedContainerSO.FindProperty("priorityOffset");
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        //DrawDefaultInspector();
        if (containerSO.type == ContainerType.Switch)
        {
            switchGroup = serializedContainerSO.FindProperty("switchGroup");
            EditorGUILayout.PropertyField(switchGroup);

            if (currentSwitchGroup != formerSwitchGroup)
            {
                UpdateProperty();
                containerSO.UpdateSwitchSoundsList();
                Debug.Log("Switch Group changed.");
            }

            if (containerSO.switchGroup)
            {
                //switchSoundsList = new SerializedObject(target as ContainerSO).FindProperty("switchSoundsList");
                EditorGUILayout.PropertyField(switchSoundsList);
            }
        }
        else
            EditorGUILayout.PropertyField(sounds);

        EditorGUILayout.PropertyField(type);
        EditorGUILayout.PropertyField(triggerMethod);

        if (containerSO.triggerMethod == TriggerMethods.Delay || containerSO.triggerMethod == TriggerMethods.TriggerRate)
            EditorGUILayout.PropertyField(delay);

        EditorGUILayout.PropertyField(pitchOffset);
        EditorGUILayout.PropertyField(panningOffset);
        EditorGUILayout.PropertyField(priorityOffset);

        EditorGUILayout.PropertyField(overrideBus);

        if (containerSO.overrideBus)
            EditorGUILayout.PropertyField(bus);

        EditorGUILayout.PropertyField(overrideLoop);

        if (containerSO.overrideLoop)
            EditorGUILayout.PropertyField(loop);

        EditorGUILayout.PropertyField(overrideSpatialBlend);

        if (containerSO.overrideSpatialBlend)
            EditorGUILayout.PropertyField(spatialBlend);

        //base.OnInspectorGUI();
        //bus = serializedObject.FindProperty("bus");

        /*
        bool overrideBus = containerSO.overrideBus;

        if (overrideBus)
        {
            SerializedProperty bus = serializedObject.FindProperty("bus");
            EditorGUILayout.PropertyField(bus);
        }
        */
        if (EditorGUI.EndChangeCheck())
        {
            if (!EditorApplication.isPlaying)
                formerSwitchGroup = containerSO.switchGroup;

            serializedContainerSO.ApplyModifiedProperties();

            if (!EditorApplication.isPlaying)
                currentSwitchGroup = containerSO.switchGroup;
        }
    }
}
