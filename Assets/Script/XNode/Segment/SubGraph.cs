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
		private NarrativeGraph _subGraphInstance;
		
		public override void InitNode(NarrativeGraph _graph)
		{
			base.InitNode(_graph);
			Debug.Log("Start [ SubGraph ]");
			if (subGraph == null) return;
			
			_subGraphInstance = subGraph.Copy() as NarrativeGraph;
			_subGraphInstance.InitGraph(((NarrativeGraph)graph).narrativeManager);
		}


		public override void UpdateNode()
		{
			base.UpdateNode();
			if (subGraph == null)
			{
				EndNode();
				return;
			}
			
			_subGraphInstance.UpdateGraph();
			
			if(_subGraphInstance.Ended())
				EndNode();
		}
		
		
		public override void EndNode()
		{
			Debug.Log("End [ SubGraph ]");
			base.EndNode();
		}
		
	}
	
	
	
	#region EDITOR

	[NodeEditor.CustomNodeEditorAttribute(typeof(SubGraph))]
	public class SubGraphEditor : NarrativeSegmentEditor
	{
		public override void Body(NarrativeSegment segment)
		{
			var subGraph = (SubGraph) segment;
			subGraph.subGraph = EditorGUILayout.ObjectField("Graph", subGraph.subGraph, typeof(NarrativeGraph)) as NarrativeGraph;
			if (subGraph.subGraph == null)
			{
				if (GUILayout.Button("Create"))
				{
					NarrativeGraph createdSubGraph = ScriptableObject.CreateInstance<NarrativeGraph>();

					int i = 0;
					UnityEngine.Object _exists = AssetDatabase.LoadAssetAtPath("Assets/Narrative/"+subGraph.name+" "+ (i==0?"":i.ToString()) +".asset", typeof(NarrativeGraph));
					while (_exists != null)
					{
						++i;
						_exists = AssetDatabase.LoadAssetAtPath("Assets/Narrative/"+subGraph.name+" "+ (i==0?"":i.ToString()) +".asset", typeof(NarrativeGraph));
					}
					
					AssetDatabase.CreateAsset(createdSubGraph, "Assets/Narrative/"+subGraph.name+" "+ (i==0?"":i.ToString()) +".asset");
					AssetDatabase.SaveAssets();

					EditorUtility.FocusProjectWindow();
					subGraph.subGraph = createdSubGraph;
				}
			}
		}
	}
	#endregion
}