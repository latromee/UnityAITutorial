using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int count = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;

    [Header("FishSettings")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(0f, 5f)]
    public float rotationSpeed;
    [Range(1f, 10f)]
    public float neighbourDistance;

    private void Start()
    {
        allFish = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                             Random.Range(-swimLimits.y, swimLimits.y),
                                                             Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        goalPos = transform.position;
    }
    private void Update()
    {
        if (Random.Range(0, 100) < 10)
            goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                             Random.Range(-swimLimits.y, swimLimits.y),
                                                             Random.Range(-swimLimits.z, swimLimits.z));
    }
}
