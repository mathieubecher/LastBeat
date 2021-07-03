using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace NarrativeSystem
{
    public class NarrativeManager : MonoBehaviour
    {
        [SerializeField] private NarrativeGraph _narrativeGraph;
        [SerializeField] private Node _activeSegment;

        void Start()
        {
            _narrativeGraph.InitGraph();
        }

        void Update()
        {
            _narrativeGraph.UpdateGraph();
        }
        
    }
}