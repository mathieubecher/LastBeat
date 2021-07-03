using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
    public class DialogSegment : NarrativeSegment
    {
        public float waiting;
        public string dialogText;
        public string source;
        private float _timer = 0.0f;
        private float _waitingTimer = 0.0f;

        public override void ResetNode()
        {
            _timer = 0.0f;
            base.ResetNode();
        }

        public override void InitNode(NarrativeGraph _graph)
        {
            base.InitNode(_graph);
            _timer = 3.0f;
            _timer += waiting;
            Debug.Log("Start [ "+dialogText+" ]");
        }

        public override void UpdateNode()
        {
            base.UpdateNode();
            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                if(_timer < 0) EndNode();
            }
        }

        public override void EndNode()
        {
            base.EndNode();
            
            Debug.Log("End [ "+dialogText+" ]");
        }
    }
    
    
    [NodeEditor.CustomNodeEditorAttribute(typeof(DialogSegment))]
    public class DialogSegmentEditor : NarrativeSegmentEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            
            var segment = serializedObject.targetObject as DialogSegment;
            NodeEditorGUILayout.PortField(segment.GetPort("input"));
            
            GUILayout.Space(10);

            segment.waiting = EditorGUILayout.FloatField("Waiting", segment.waiting);
            segment.source = EditorGUILayout.TextField("Source", segment.source);
            GUILayout.Label("Dialog Text");
            segment.dialogText = GUILayout.TextArea(segment.dialogText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });
            
            GUILayout.Space(10);
            NodeEditorGUILayout.DynamicPortList(
                "outputs",
                typeof(float),
                serializedObject,
                NodePort.IO.Input,
                Node.ConnectionType.Override,
                Node.TypeConstraint.None);
            
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}