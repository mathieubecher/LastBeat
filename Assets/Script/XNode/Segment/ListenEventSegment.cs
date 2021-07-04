using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
	[NodeTint(100,70,70)]
	public class ListenEventSegment : NarrativeSegment {
		private static float TOLERANCE = 0.001f;
		
		public enum Listener
		{
			Frequency
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
		public float exitTime;

		private float _exitTimer;
		
		public override void InitNode(NarrativeGraph _graph)
		{
			base.InitNode(_graph);
			Debug.Log("Start [ ListenEvent ]");
			_exitTimer = exitTime;
		}

		public override void UpdateNode()
		{
			base.UpdateNode();
			if (_exitTimer > 0f)
			{
				_exitTimer -= Time.deltaTime;
				CloseNode();
			}

			if(ListenEvent()) EndNode();
		}

		private bool ListenEvent()
		{
			float toCompare = 0.0f;
            switch (listener)
            {
            	case Listener.Frequency:
	                toCompare = ((NarrativeGraph) graph).narrativeManager.controller.Frequency(duration);
            		break;
            }
			
            switch (comparator)
            {
            	case Comparator.Equals:
	                return Math.Abs(toCompare - value) < TOLERANCE;
            	case Comparator.Less:
	                return toCompare < value;
            	case Comparator.More:
	                return toCompare > value;
            }

            return false;
		}

		public override void CloseNode()
		{
			Debug.Log("Exit [ ListenEvent ]");
			base.CloseNode();
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
			
			GUILayout.Space(10);
			
			listenEvent.exitTime = EditorGUILayout.FloatField("Exit Time", listenEvent.exitTime);
		}
	}
	#endregion
}
