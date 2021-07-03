using System;
using System.Collections;
using System.Collections.Generic;
using NarrativeSystem;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
	[NodeTint(70,70,100)]
	public class SubGraph : NarrativeSegment
	{
		public NarrativeGraph subGraph;
		
		public override void InitNode(NarrativeGraph _graph)
		{
			base.InitNode(_graph);
			Debug.Log("Start [ SubGraph ]");
			subGraph.InitGraph();
		}


		public override void UpdateNode()
		{
			base.UpdateNode();
			subGraph.UpdateGraph();
			
			if(subGraph.Ended())
				EndNode();
		}
		
		
		public override void EndNode()
		{
			Debug.Log("End [ SubGraph ]");
			base.EndNode();
		}
		
	}

	[NodeEditor.CustomNodeEditorAttribute(typeof(SubGraph))]
	public class SubGraphEditor : NarrativeSegmentEditor
	{
		public override void OnBodyGUI()
		{
			serializedObject.Update();

			var segment = serializedObject.targetObject as SubGraph;
			NodeEditorGUILayout.PortField(segment.GetPort("input"));

			GUILayout.Space(10);

			segment.subGraph = EditorGUILayout.ObjectField("Graph", segment.subGraph, typeof(NarrativeGraph)) as NarrativeGraph;

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