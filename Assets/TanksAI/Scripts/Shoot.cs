using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject shellPrefab;
    [SerializeField] GameObject shellSpawnPos;
    [SerializeField] GameObject target;
    [SerializeField] GameObject parent;

    [SerializeField] float speed = 15f;
    [SerializeField] float turnSpeed = 2f;
    [SerializeField] float fireRate = 0.1f;

    float timeOfLastShot = 0f;
    void Fire()
    {
        if (Time.realtimeSinceStartup - timeOfLastShot > fireRate)
        {
            GameObject shell = GameObject.Instantiate(shellPrefab, shellSpawnPos.transform.position, shellSpawnPos.transform.rotation);
            shell.GetComponent<Rigidbody>().velocity = speed * transform.forward;

            timeOfLastShot = Time.realtimeSinceStartup;
        }
    }

    float? RotateTurret()
    {
        float? angle = CalculateAngle(low: true);

        if (angle != null)
        {
            transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }
    float? CalculateAngle(bool low)
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float y = targetDir.y;
        targetDir.y = 0;
        float x = targetDir.magnitude;
        float gravity = 9.81f;
        float sSqr = speed * speed;
        float underSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underSqrRoot < 0f) return null;

        float root = Mathf.Sqrt(underSqrRoot);
        float highAngle = sSqr + root;
        float lowAngle = sSqr - root;

        if (low)
            return Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg;
        else
            return Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg;
    }

    private void Start()
    {
        timeOfLastShot = Time.realtimeSinceStartup;
    }
    private void Update()
    {
        Vector3 direction = (target.transform.position - parent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);

        float? angle = RotateTurret();


        if (angle != null && Vector3.Angle(direction, parent.transform.forward) < 5f)
        {
            Fire();

        }
    }
}