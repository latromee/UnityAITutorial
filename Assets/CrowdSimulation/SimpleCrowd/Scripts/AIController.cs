using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleCrowdSimulation
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] GameObject goal;
        NavMeshAgent agent;
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(goal.transform.position);
        }
    }
}