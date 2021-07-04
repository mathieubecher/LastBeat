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

        public override void InitNode(NarrativeGraph _graph)
        {
            base.InitNode(_graph);
        }
        public override void Play()
        {
            base.Play();
            Debug.Log("[ "+dialogText+" ]");
        }
    }
    
    
    #region EDITOR
    [NodeEditor.CustomNodeEditorAttribute(typeof(DialogSegment))]
    public class DialogSegmentEditor : NarrativeSegmentEditor
    {
        public override void Body(NarrativeSegment segment)
        {
            base.Body(segment);
            var dialog = (DialogSegment)segment;
            GUILayout.Label("Dialog Text");
            dialog.dialogText = GUILayout.TextArea(dialog.dialogText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });
        }
    }
    #endregion
}