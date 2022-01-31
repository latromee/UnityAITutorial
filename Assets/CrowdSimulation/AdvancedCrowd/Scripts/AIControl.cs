using UnityEngine;
using UnityEngine.AI;

namespace AdvancedCrowd
{
    public class AIControl : MonoBehaviour
    {
        GameObject[] goalLocations;
        NavMeshAgent agent;
        Animator anim;
        float speedMult;
        float detectionRadius = 100f;
        float fleeRadis = 5f;


        void ResetAgent()
        {
            speedMult = Random.Range(0.7f, 1.3f);
            agent.speed = speedMult;
            agent.angularSpeed = 120f;
            anim.SetFloat("speedMult", speedMult);
            anim.SetFloat("wOffset", Random.Range(0f, 1f));
            anim.SetTrigger("isWalking");
            agent.ResetPath();
        }
        public void DetectNewObstacle(Vector3 position)
        {
            if(Vector3.Distance(transform.position,position) < detectionRadius)
            {
                Vector3 fleeDir = transform.position - position;
                Vector3 newGoal = transform.position + fleeDir * fleeRadis;

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(newGoal, path);

                if(path.status != NavMeshPathStatus.PathInvalid)
                {
                    agent.SetDestination(path.corners[path.corners.Length - 1]);
                    anim.SetTrigger("isRunning");
                    agent.speed = 10f;
                    agent.angularSpeed = 500f;
                }
            }
        }
        void Start()
        {
            goalLocations = GameObject.FindGameObjectsWithTag("goal");
            agent = this.GetComponent<NavMeshAgent>();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
            anim = this.GetComponent<Animator>();

            ResetAgent();
        }

        void Update()
        {
            if (agent.remainingDistance < 1)
            {
                ResetAgent();
                agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
            }
        }
    }
}