using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
	[NodeTint(100,70,70)]
	public class ListenEventSegment : NarrativeSegment {
		public enum Listener
		{
			LastBeat,
			Mean
		}

		public Listener listener;
		
		public enum Comparator
		{
			Less,
			More,
			Equals
		}

		public Comparator comparator;

		public float value;
		public float duration;
		
		
		
		
		public override void InitNode(NarrativeGraph _graph)
		{
			base.InitNode(_graph);
			Debug.Log("Start [ ListenEvent ]");
		}

		public override void UpdateNode()
		{
			base.UpdateNode();
			EndNode();
		}

		public override void EndNode()
		{
			Debug.Log("End [ ListenEvent ]");
			base.EndNode();
		}
	}
	
	
	
	
	
	[NodeEditor.CustomNodeEditorAttribute(typeof(ListenEventSegment))]
	public class ListenEventSegmentEditor : NarrativeSegmentEditor
	{
		public override void OnBodyGUI()
		{
			serializedObject.Update();
            
			var segment = serializedObject.targetObject as ListenEventSegment;
			NodeEditorGUILayout.PortField(segment.GetPort("input"));
            
			GUILayout.Space(10);

			segment.listener = (ListenEventSegment.Listener) EditorGUILayout.EnumPopup("Listener", segment.listener);
			segment.comparator = (ListenEventSegment.Comparator) EditorGUILayout.EnumPopup("Comparator", segment.comparator);
			segment.value = EditorGUILayout.FloatField("Value", segment.value);
			segment.duration = EditorGUILayout.FloatField("Duration", segment.duration);
				
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
