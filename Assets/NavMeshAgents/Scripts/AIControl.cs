using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace NavMeshAgents
{
    public class AIControl : MonoBehaviour
    {
        public NavMeshAgent agent;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}