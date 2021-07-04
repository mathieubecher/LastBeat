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
        [HideInInspector] public Controller controller;
        
        void Start()
        {
            controller = FindObjectOfType<Controller>();
            _narrativeGraph.InitGraph(this);
        }

        void Update()
        {
            _narrativeGraph.UpdateGraph();
        }

    }
}