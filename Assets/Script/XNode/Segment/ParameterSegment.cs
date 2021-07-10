using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace NarrativeSystem
{
    public class ParameterSegment : NarrativeSegment
    {
        public Target target = Target.GLOBAL;
        
        [FMODUnity.EventRef]
        public string eventFMod;
        
        public string actorId;
        
        public float waiting;

        protected float _timer = 0.0f;
        protected float _waitingTimer = 0.0f;
        
        [FMODUnity.EventRef]
        public string parameter;
        public float value;
        
        
        public override void InitNode(NarrativeGraph _graph)
        {
            Debug.Log("Start [ Parameter ]");
            base.InitNode(_graph);
        }

        public override void UpdateNode()
        {
            base.UpdateNode();
            if (_waitingTimer >= 0f)
            {
                _waitingTimer -= Time.deltaTime;
                if (_waitingTimer < 0)
                {
                    SendParameter();
                    EndNode();
                }
            }
        }

        private void SendParameter()
        {
            NarrativeActor actor = ((NarrativeGraph) graph).narrativeManager.GetActor(actorId);
            if (actor == null)
            {
                Debug.LogError("Actor "+actorId+ " doesn't exit");
                return;
            }
            actor.SetParameter(parameter, value, target, eventFMod);
        }
        
        public override void EndNode()
        {
            base.EndNode();
            Debug.Log("End [ Parameter ]");
        }
    }

    
    #region EDITOR
        
    [NodeEditor.CustomNodeEditorAttribute(typeof(ParameterSegment))]
    public class SetParameterEditor : NarrativeSegmentEditor
    {
        public override void Body(NarrativeSegment segment)
        {
            var parameter = (ParameterSegment)segment;
            
            parameter.waiting = EditorGUILayout.FloatField("Waiting", parameter.waiting);
            GUILayout.Space(EDITOR_SPACING);
            
            parameter.actorId = EditorGUILayout.TextField("Actor ID", parameter.actorId);
            parameter.target = (Target)EditorGUILayout.EnumPopup("Target", parameter.target);
            if (parameter.target == Target.LOCAL)
            {    
                parameter.eventFMod = EditorGUILayout.TextField("Event", parameter.eventFMod);
            }
            GUILayout.Space(EDITOR_SPACING);
            parameter.parameter = EditorGUILayout.TextField("Parameter", parameter.parameter);
            
            parameter.value = EditorGUILayout.FloatField("Value", parameter.value);

        }
    }
    #endregion
}