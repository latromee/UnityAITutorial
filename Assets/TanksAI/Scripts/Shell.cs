using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;
    Rigidbody rb;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "tank")
        {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (rb.velocity.magnitude > 1f) transform.forward = rb.velocity;
    }
}