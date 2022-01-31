using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavMeshAgents
{
    public class AgentManager : MonoBehaviour
    {
        GameObject[] agents;
        void Start()
        {
            agents = GameObject.FindGameObjectsWithTag("ai");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100f))
                {
                    foreach (GameObject agent in agents)
                    {
                        agent.GetComponent<AIControl>().agent.SetDestination(hit.point);
                    }
                }
            }
        }
    }
}