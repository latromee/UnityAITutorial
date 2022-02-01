using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    float speed;
    bool turning = false;

    private void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    private void Update()
    {
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2f);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);


        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, transform.forward * 5f, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
        }
        else
            turning = false;

        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                 Quaternion.LookRotation(direction),
                                 myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);

            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    void ApplyRules()
    {
        GameObject[] gos = myManager.allFish;

        Vector3 vCentre = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach (var go in gos)
        {
            if (go != gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vCentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1f)
                    {
                        vAvoid += (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vCentre = vCentre / groupSize + (myManager.goalPos - transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCentre + vAvoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
        }
    }
}
