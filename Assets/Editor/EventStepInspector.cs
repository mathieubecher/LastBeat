using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EventStep))]
public class EventStepInspector : PropertyDrawer
{
    string[] _choices;
    int _choiceIndex = 0;

    private float labelSize = 100;
    private float varSize = 200;
    private float lineHeight = 20;
    private float labelOffset = 1;

    private int lines = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        lines = 0;

        CreateProperty("Step Type", property.FindPropertyRelative("stepType"), position);

        switch (property.FindPropertyRelative("stepType").enumValueIndex)
        {
            case 0:
                CreateProperty("Sound To Play", property.FindPropertyRelative("soundToPlay"), position);
                CreateProperty("Initial Delay", property.FindPropertyRelative("playInitialDelay"), position);
                CreateProperty("Probability", property.FindPropertyRelative("playProbability"), position);
                property.FindPropertyRelative("playProbability").floatValue = 1;


                CreateProperty("Fade In", property.FindPropertyRelative("fadeIn"), position);
                if (property.FindPropertyRelative("fadeIn").boolValue)
                    CreateProperty("Fade Duration", property.FindPropertyRelative("fadeInDuration"), position);
                break;
            case 1:
                CreateProperty("Sound To Stop", property.FindPropertyRelative("soundToStop"), position);
                CreateProperty("Initial Delay", property.FindPropertyRelative("stopInitialDelay"), position);
                CreateProperty("Probability", property.FindPropertyRelative("stopProbability"), position);
                property.FindPropertyRelative("stopProbability").floatValue = 1;
                CreateProperty("Fade Out", property.FindPropertyRelative("fadeOut"), position);
                if (property.FindPropertyRelative("fadeOut").boolValue)
                    CreateProperty("Fade Duration", property.FindPropertyRelative("fadeOutDuration"), position);
                break;
            case 2:
                CreateProperty("Switch Group", property.FindPropertyRelative("switchGroup"), position);
                CreateProperty("Switch To Set", property.FindPropertyRelative("switchToSet"), position);
                break;
        }

        //Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 7;
    }

    private void CreateProperty(string myLabel, SerializedProperty property, Rect positionRect)
    {
        var varRect = new Rect(positionRect.x + labelSize + 15, positionRect.y + lineHeight * lines, varSize, lineHeight);
        var labelRect = new Rect(positionRect.x + 15, positionRect.y - labelOffset + lineHeight * lines, labelSize, lineHeight);

        EditorGUI.LabelField(labelRect, myLabel);
        EditorGUI.PropertyField(varRect, property, GUIContent.none);

        lines++;
    }
}
