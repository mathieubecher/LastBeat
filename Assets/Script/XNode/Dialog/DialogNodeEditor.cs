using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    [NodeEditor.CustomNodeEditorAttribute(typeof(DialogSegment))]
    public class DialogNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            var segment = serializedObject.targetObject as DialogSegment;
            NodeEditorGUILayout.PortField(segment.GetPort("input"));
            
            GUILayout.Space(10);
            
            segment.waiting = EditorGUILayout.FloatField("Waiting", segment.waiting);
            segment.dialogText = EditorGUILayout.TextField("Source", segment.source);
            GUILayout.Label("Dialog Text");
            segment.dialogText = GUILayout.TextArea(segment.dialogText, new GUILayoutOption[]
            {
                GUILayout.MinHeight(50),
            });
            
            GUILayout.Space(10);
            NodeEditorGUILayout.DynamicPortList(
                "Answers",
                typeof(float),
                serializedObject,
                NodePort.IO.Input,
                Node.ConnectionType.Override,
                Node.TypeConstraint.None);
            
            
            serializedObject.ApplyModifiedProperties();
            
            
        }
    }
}
