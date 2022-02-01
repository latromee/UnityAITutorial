using UnityEngine;

namespace TanksAStar
{
    public class FollowPath : MonoBehaviour
    {
        Transform goal;

        [SerializeField] float speed = 5f;
        [SerializeField] float rotSpeed = 2f;
        [SerializeField] float accuracy = 1f;

        public GameObject wpManager;
        GameObject[] wps;
        GameObject currentNode;
        int currentWP = 0;
        Graph g;

        private void Start()
        {
            wps = wpManager.GetComponent<WPManager>().waypoints;
            g = wpManager.GetComponent<WPManager>().graph;
            currentNode = wps[0];
        }

        public void GoToHeli()
        {
            g.AStar(currentNode, wps[1]);
            currentWP = 0;
        }

        public void GoToRuin()
        {
            g.AStar(currentNode, wps[6]);
            currentWP = 0;
        }

        public void GoToFactory()
        {
            g.AStar(currentNode, wps[8]);
            currentWP = 0;
        }

        private void LateUpdate()
        {
            if (g.getPathLength() == 0 || currentWP == g.getPathLength()) return;

            currentNode = g.getPathPoint(currentWP);

            if (Vector3.Distance(
                g.getPathPoint(currentWP).transform.position,
                transform.position) < accuracy)
            {
                currentWP++;
            }

            if (currentWP < g.getPathLength())
            {
                goal = g.getPathPoint(currentWP).transform;
                Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
                Vector3 direction = lookAtGoal - transform.position;

                transform.Translate(0f, 0f, speed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            }
        }

    }
}