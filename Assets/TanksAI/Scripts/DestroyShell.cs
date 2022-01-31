using UnityEngine;

public class DestroyShell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5);
    }
}
