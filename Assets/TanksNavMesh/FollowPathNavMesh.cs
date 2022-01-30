using UnityEngine;
using UnityEngine.AI;

public class FollowPathNavMesh : MonoBehaviour
{
    public GameObject wpManager;
    GameObject[] wps;

    NavMeshAgent agent; 

    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToHeli()
    {
        agent.SetDestination(wps[1].transform.position);
    }

    public void GoToRuin()
    {
        agent.SetDestination(wps[6].transform.position);
    }

    public void GoToFactory()
    {
        agent.SetDestination(wps[8].transform.position);
    }

    private void LateUpdate()
    { 

    }

}
