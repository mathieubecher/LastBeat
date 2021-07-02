using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogSystem
{
    public class DialogSegment : Node
    {
        [Serializable]
        public struct Connection{}

        public float waiting;
        
        [Input] public Connection input;
        public string dialogText;
        public string source;

        [Output(dynamicPortList = true)] 
        public List<string> Answers;

        public override object GetValue(NodePort port)
        {
	        return null;
        }
    }
}