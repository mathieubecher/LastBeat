using System.Collections;
using System.Collections.Generic;
using NarrativeSystem;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
    {
    [NodeTint(100,70,100), NodeWidth(300)]
    public class AudioSegment : NarrativeSegment {

        public PlayingType type = PlayingType.ONESHOT;
        
        [FMODUnity.EventRef]
        public string source;

        public string actorId;
        
        public float waiting;

        protected float _timer = 0.0f;
        protected float _waitingTimer = 0.0f;
        
        
        public override void ResetNode()
        {
            _timer = 0.0f;
            base.ResetNode();
        }

        public override void InitNode(NarrativeGraph _graph)
        {
            Debug.Log("Start [ AudioSegment ]");
            base.InitNode(_graph);
            _timer = 0.0f;
            if (type == PlayingType.ONESHOT) _timer = 3.0f;
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
        
        
        public virtual void Play()
        {
            NarrativeActor actor = ((NarrativeGraph) graph).narrativeManager.GetActor(actorId);
            if (actor == null)
            {
                Debug.LogError("Actor "+actorId+ " doesn't exit");
                return;
            }
            actor.PlaySource(source, type);
            
        }
        
        
        public override void EndNode()
        {
            base.EndNode();
            Debug.Log("End [ AudioSegment ]");
        }
    }

    
    
    #region EDITOR
        
    [NodeEditor.CustomNodeEditorAttribute(typeof(AudioSegment))]
    public class AudioSegmentEditor : NarrativeSegmentEditor
    {
        SerializedProperty source;
     
        void OnEnable()
        {
            // Fetch the objects from the GameObject script to display in the inspector
        }
        public override void Body(NarrativeSegment segment)
        {
            source = serializedObject.FindProperty("source");
            var audio = (AudioSegment)segment;

            audio.waiting = EditorGUILayout.FloatField("Waiting", audio.waiting);
            GUILayout.Space(EDITOR_SPACING);
            
            audio.actorId = EditorGUILayout.TextField("Actor ID", audio.actorId);
            
            audio.type = (PlayingType)EditorGUILayout.EnumPopup("Type", audio.type);
            EditorGUILayout.PropertyField(source, new GUIContent("Source"), GUILayout.Height(40));
            
        }
    }
    #endregion
}