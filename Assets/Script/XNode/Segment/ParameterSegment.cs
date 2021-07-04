using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace NarrativeSystem
{
    public class ParameterSegment : NarrativeSegment
    {
        public Motor motor = Motor.FMOD;
        public Target target = Target.GLOBAL;
        
        [FMODUnity.EventRef]
        public string eventFMod;
        
        public string actorId;
        
        public float waiting;

        protected float _timer = 0.0f;
        protected float _waitingTimer = 0.0f;
        
        [FMODUnity.EventRef]
        public string parameter;
        public EventSO parameterPEngine;
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
                    Play();
                    EndNode();
                }
            }
        }

        private void Play()
        {
            NarrativeActor actor = ((NarrativeGraph) graph).narrativeManager.GetActor(actorId);
            if (actor == null)
            {
                Debug.LogError("Actor "+actorId+ " doesn't exit");
                return;
            }
            if(motor == Motor.FMOD) actor.SetParameter(parameter, value, target, eventFMod);
            else actor.SetParameterPEngine(parameterPEngine, value, target);
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
            GUILayout.Space(10);
            
            parameter.actorId = EditorGUILayout.TextField("Actor ID", parameter.actorId);
            parameter.motor = (Motor)EditorGUILayout.EnumPopup("Motor", parameter.motor);
            parameter.target = (Target)EditorGUILayout.EnumPopup("Target", parameter.target);
            if (parameter.motor == Motor.FMOD && parameter.target == Target.LOCAL)
            {    
                parameter.eventFMod = EditorGUILayout.TextField("Event", parameter.eventFMod);
            }
            GUILayout.Space(10);
            if (parameter.motor == Motor.FMOD)
            {    
                parameter.parameter = EditorGUILayout.TextField("Parameter", parameter.parameter);
            }
            else
            {
                parameter.parameterPEngine = (EventSO) EditorGUILayout.ObjectField("Source", parameter.parameterPEngine, typeof(EventSO));
            }
            parameter.value = EditorGUILayout.FloatField("Value", parameter.value);

        }
    }
    #endregion
}