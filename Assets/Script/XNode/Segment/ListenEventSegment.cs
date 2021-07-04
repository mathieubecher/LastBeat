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
	
	
	
	
	#region EDITOR
	[NodeEditor.CustomNodeEditorAttribute(typeof(ListenEventSegment))]
	public class ListenEventSegmentEditor : NarrativeSegmentEditor
	{
		public override void Body(NarrativeSegment segment)
		{
			var listenEvent = (ListenEventSegment) segment; 
			listenEvent.listener = (ListenEventSegment.Listener) EditorGUILayout.EnumPopup("Listener", listenEvent.listener);
			listenEvent.comparator = (ListenEventSegment.Comparator) EditorGUILayout.EnumPopup("Comparator", listenEvent.comparator);
			listenEvent.value = EditorGUILayout.FloatField("Value", listenEvent.value);
			listenEvent.duration = EditorGUILayout.FloatField("Duration", listenEvent.duration);
		}
	}
	#endregion
}
