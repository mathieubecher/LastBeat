using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
    [NodeTint(70,100,100)]
    public class DialogSegment : AudioSegment
    {
        public string dialogText;

        public override void Play()
        {
            Debug.Log("Start [ "+dialogText+" ]");
        }
    }
    
    
    #region EDITOR
    [NodeEditor.CustomNodeEditorAttribute(typeof(DialogSegment))]
    public class DialogSegmentEditor : NarrativeSegmentEditor
    {
        public override void Body(NarrativeSegment segment)
        {
            var dialog = (DialogSegment)segment;

            dialog.waiting = EditorGUILayout.FloatField("Waiting", dialog.waiting);
            dialog.source = EditorGUILayout.TextField("Source", dialog.source);
            GUILayout.Label("Dialog Text");
            dialog.dialogText = GUILayout.TextArea(dialog.dialogText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });
        }
    }
    #endregion
}