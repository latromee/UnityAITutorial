using UnityEngine;

namespace AdvancedCrowd
{
    public class DropCylinder : MonoBehaviour
    {
        [SerializeField] GameObject obstacle;
        Camera cam;
        GameObject[] agents;
        private void Start()
        {
            agents = GameObject.FindGameObjectsWithTag("agent");
            cam = GetComponent<Camera>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Instantiate(obstacle, hit.point, obstacle.transform.rotation);
                    foreach (var agent in agents)
                    {
                        agent.GetComponent<AIControl>().DetectNewObstacle(hit.point);
                    }
                }
            }
        }
    }
}