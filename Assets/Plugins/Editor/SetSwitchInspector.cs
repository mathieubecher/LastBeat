using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SetSwitch))]
public class SetSwitchInspector : Editor
{
    string[] _choices;
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        var setSwitch = target as SetSwitch;

        // Draw the default inspector
        DrawDefaultInspector();

        if (setSwitch.switchGroup)
        {
            _choices = setSwitch.switchGroup.switches;

            // Set the choice index to the previously selected index
            _choiceIndex = Array.IndexOf(_choices, setSwitch.switchToSet);

            // If the choice is not in the array then the _choiceIndex will be -1 so set back to 0
            if (_choiceIndex < 0)
                _choiceIndex = 0;

            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);

            // Update the selected choice in the underlying object
            setSwitch.switchToSet = _choices[_choiceIndex];
            setSwitch.switchIndex = _choiceIndex;
        }
        
        // Save the changes back to the object
        EditorUtility.SetDirty(target);
    }
}
