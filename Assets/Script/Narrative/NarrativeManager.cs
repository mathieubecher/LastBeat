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

        private Dictionary<string, NarrativeActor> actors;
        
        void Start()
        {
            actors = new Dictionary<string, NarrativeActor>();
            foreach (var actor in FindObjectsOfType<NarrativeActor>())
            {
                actors.Add(actor.id, actor);
            }
            
            controller = FindObjectOfType<Controller>();
            _narrativeGraph.InitGraph(this);
        }

        void Update()
        {
            _narrativeGraph.UpdateGraph();
        }

        public NarrativeActor GetActor(string id)
        {
            return HasActor(id)? actors[id]:null;
        }

        public bool HasActor(string id)
        {
            return actors.ContainsKey(id);
        }
    }
}