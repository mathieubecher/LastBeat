using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace NarrativeSystem
{
	
	[CreateAssetMenu]
	public class NarrativeGraph : NodeGraph
	{
		[SerializeField] private List<NarrativeSegment> _actives;
		[SerializeField] private List<NarrativeSegment> _requestings;
		

		public void InitGraph()
		{
			_requestings = new List<NarrativeSegment>();
			_actives = new List<NarrativeSegment>();
			
			
			foreach (NarrativeSegment node in nodes)
			{
				node.ResetNode();
				if (!node.GetInputPort("input").IsConnected)
				{
					_actives.Add(node);
					node.InitNode(this);
				}
			}
		}
		
		public void UpdateGraph()
		{
			
			for (int i = _actives.Count - 1; i >= 0; --i)
			{
				if(!_actives[i].active)
					_actives.RemoveAt(i);
			}

			foreach(var requesting in _requestings)
			{
				_actives.Add(requesting);	
				requesting.InitNode(this);
			}
			_requestings = new List<NarrativeSegment>();
			
			foreach (NarrativeSegment node in _actives)
			{
				node.UpdateNode();
			}
		}

		public void RequestingNode(NarrativeSegment node)
		{
			_requestings.Add(node);
		}

		
		public bool Ended()
		{
			return _actives.Count == 0 && _requestings.Count == 0;
		}
	}

}