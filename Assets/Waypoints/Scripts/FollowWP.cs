using UnityEngine;

public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 10f;
    public float rotationSpeed = 10f;
    public float lookAhead = 5f;

    GameObject tracker;

    void Start()
    {
        tracker = new GameObject();
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 2)
        {
            currentWP = (currentWP + 1) % waypoints.Length;
        }
        tracker.transform.LookAt(waypoints[currentWP].transform);
        tracker.transform.Translate(0f, 0f, (speed + 3f) * Time.deltaTime);
    }

    void Update()
    {
        ProgressTracker();

        Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, rotationSpeed * Time.deltaTime);

        transform.Translate(0f, 0f, speed * Time.deltaTime);
    }
}
