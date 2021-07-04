using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


namespace NarrativeSystem
{
	[NodeTint(50,50,50)]
	public class GoToSegment : NarrativeSegment
	{
		public float waiting;
		public string actorId;
		public string pos;
		public float duration;
		protected float _waitingTimer = 0.0f;
		protected float _timer = 0.0f;

		public override void ResetNode()
		{
			_timer = 0.0f;
			_waitingTimer = 0.0f;
			base.ResetNode();
		}
		public override void InitNode(NarrativeGraph _graph)
		{
			base.InitNode(_graph);
            _timer = duration;
            _waitingTimer = waiting;
		}
		
		public override void UpdateNode()
		{
			base.UpdateNode();
			if (_waitingTimer >= 0f)
			{
				_waitingTimer -= Time.deltaTime;
				if (_waitingTimer < 0)
				{
					Play();
				}
			} 
            
			if (_waitingTimer < 0 && _timer >= 0f)
			{
				_timer -= Time.deltaTime;
				if(_timer < 0) EndNode();
			}
		}
		public void Play()
		{
			NarrativeActor actor = ((NarrativeGraph) graph).narrativeManager.GetActor(actorId);
			if (actor == null)
			{
				Debug.LogError("Actor "+actorId+ " doesn't exit");
				return;
			}
			actor.Goto(duration);
		}

	}
    
    
	#region EDITOR
	[NodeEditor.CustomNodeEditorAttribute(typeof(GoToSegment))]
	public class GotoSegmentEditor : NarrativeSegmentEditor
	{
		public override void Body(NarrativeSegment segment)
		{
			var goTo = (GoToSegment)segment;

			goTo.waiting = EditorGUILayout.FloatField("Waiting", goTo.waiting);
			GUILayout.Space(10);
			goTo.actorId = EditorGUILayout.TextField("Actor ID", goTo.actorId);
			goTo.pos = EditorGUILayout.TextField("GoTo", goTo.pos);
			goTo.duration = EditorGUILayout.FloatField("Duration", goTo.duration);
		}
	}
	#endregion
}