using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem{
    public abstract class NarrativeSegment : Node {


	    [Serializable]
	    public struct Connection {}
        [Input] public Connection input;
        [Output(dynamicPortList = true)] public List<Connection> outputs;

        public bool active = false;
        private NarrativeGraph narrativeGraph;
        
        public virtual void ResetNode()
        {
	        active = false;
        }
        public virtual void InitNode(NarrativeGraph _graph)
        {
	        active = true;
	        narrativeGraph = _graph;

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
	        NodePort port;
	        int i = 0;

	        foreach (var fieldname in GetOutputPortsName ())
	        {
		        if (fieldname != "outputs")
		        {
			        port = GetOutputPort(fieldname);
			        ++i;
			        if (port != null && port.IsConnected)
			        {
				        narrativeGraph.RequestingNode(port.Connection.node as NarrativeSegment);
			        } 
		        }
		        
	        }
        }
        public override object GetValue(NodePort port)
        {
            return null;
        }

    }

    
    
	
    #region EDITOR

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
    #endregion
    
}