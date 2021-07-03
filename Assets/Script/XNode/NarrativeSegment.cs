using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem{
    public class NarrativeSegment : Node {
        
        
        [Serializable]
        public struct Connection{}
        [Input] public Connection input;
        [Output(dynamicPortList = true)] public List<Connection> outputs;

        public bool active = false;
        private NarrativeGraph graph;
        
        public virtual void ResetNode()
        {
	        active = false;
        }
        public virtual void InitNode(NarrativeGraph _graph)
        {
	        active = true;
	        graph = _graph;

        }
        public virtual void UpdateNode()
        {
        }
        public virtual void EndNode()
        {
	        active = false;
	        NextNode();
			
        }

        private void NextNode()
        {
	        for (int i = 0; i < outputs.Count; ++i)
	        {
		        var port = GetPort("outputs " + i);
		        if (port.IsConnected)
		        {
			        graph.RequestingNode(port.Connection.node as NarrativeSegment);
		        }
	        }
        }
        public override object GetValue(NodePort port)
        {
            return null;
        }

    }

    [NodeEditor.CustomNodeEditorAttribute(typeof(NarrativeSegment))]
    public class NarrativeSegmentEditor : NodeEditor
    {
	    public override void OnHeaderGUI() {
		    // Initialization
		    var segment = serializedObject.targetObject as NarrativeSegment;
		    base.OnHeaderGUI();
		    Rect dotRect = GUILayoutUtility.GetLastRect();
		    dotRect.size = new Vector2(16, 16);
		    dotRect.y += 6;

		    GUI.color = segment.active ? Color.green : Color.red;
		    GUI.DrawTexture(dotRect, NodeEditorResources.dot);
		    GUI.color = Color.white;
	    }
	    
    }
    /*
    Rect dotRect = GUILayoutUtility.GetLastRect();
	dotRect.size = new Vector2(16, 16);
	dotRect.y += 6;

	GUI.color = graphEditor.GetLerpColor(Color.red, Color.green, node, node.led);
	GUI.DrawTexture(dotRect, NodeEditorResources.dot);
	GUI.color = Color.white;
     */
    
}