using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    public class NarrativeManager : MonoBehaviour
    {
        [SerializeField] private NarrativeGraph _narrativeGraph;
        [SerializeField] private Node _activeSegment;

        void Start()
        {
            foreach (Node node in _narrativeGraph.nodes)
            {
                if (!node.GetInputPort("input").IsConnected)
                {
                    _activeSegment = node;
                    break;
                }
            }
        }
        
    }
}