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
		public Texture image;
		public float waiting;
		public string actorId;
		public Vector2 pos;
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
		protected static Texture image;
		private Vector2 dotPos;
		public override void Body(NarrativeSegment segment)
		{
			if (image == null)
			{
				image = Resources.Load("plan") as Texture;
			}

			var goTo = (GoToSegment) segment;

			goTo.waiting = EditorGUILayout.FloatField("Waiting", goTo.waiting);
			GUILayout.Space(EDITOR_SPACING);
			goTo.actorId = EditorGUILayout.TextField("Actor ID", goTo.actorId);
			goTo.duration = EditorGUILayout.FloatField("Duration", goTo.duration);

			GUILayout.Space(EDITOR_SPACING);
			int width = GetWidth() - GetBodyStyle().padding.horizontal ;
			int height = width * image.height / image.width;
			GUILayout.Box(image, GUILayout.Width(width), GUILayout.Height(height));

			var rect = GUILayoutUtility.GetLastRect();
			
			
			Vector2 pos = Event.current.mousePosition - rect.position;
			if ((Event.current.button == 0) && (Event.current.type == EventType.MouseDown) && 
			    (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height))
			{
				
				goTo.pos = new Vector2(pos.x/width, pos.y/height);
			}
			dotPos = new Vector2(goTo.pos.x * width, goTo.pos.y * height);
			Rect dotRect = GUILayoutUtility.GetLastRect();
			dotRect.size = new Vector2(10, 10);
			dotRect.position = dotPos + rect.position - new Vector2(5,10);
			dotRect.y += 6;
			GUI.color = segment.active ? Color.green : Color.red;
			GUI.DrawTexture(dotRect, NodeEditorResources.dot);
			GUI.color = Color.white;
		}
	}
	#endregion
}