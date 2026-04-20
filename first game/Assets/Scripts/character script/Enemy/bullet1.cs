using UnityEngine;

public class bullet1 : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * 250;
        Destroy(gameObject, 0.2f);
    }

}
