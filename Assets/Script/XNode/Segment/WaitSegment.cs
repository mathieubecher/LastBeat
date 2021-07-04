using System.Collections;
using System.Collections.Generic;
using NarrativeSystem;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
    public class WaitSegment : NarrativeSegment {
        public float duration;
        private float _timer;
        
        public override void ResetNode()
        {
            _timer = 0.0f;
            base.ResetNode();
        }

        public override void InitNode(NarrativeGraph _graph)
        {
            base.InitNode(_graph);
            _timer = duration;
            Debug.Log("Start [ Wait ]");
        }

        public override void UpdateNode()
        {
            base.UpdateNode();
            if (_timer >= 0f)
            {
                _timer -= Time.deltaTime;
                if(_timer < 0) EndNode();
            }
        }
        
        
        public override void EndNode()
        {
            base.EndNode();
            Debug.Log("End [ Wait ]");
        }
    }

    
    
    #region EDITOR
        
    [NodeEditor.CustomNodeEditorAttribute(typeof(WaitSegment))]
    public class WaitEditor : NarrativeSegmentEditor
    {
        public override void Body(NarrativeSegment segment)
        {
            var wait = (WaitSegment)segment;

            wait.duration = EditorGUILayout.FloatField("Duration", wait.duration);
        }
    }
    #endregion
}