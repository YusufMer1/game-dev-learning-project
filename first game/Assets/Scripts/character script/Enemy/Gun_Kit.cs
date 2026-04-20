using UnityEngine;

public class Gun_Kit : MonoBehaviour
{
    public ParticleSystem particle;
    void Start()
    {
         Instantiate(particle,gameObject.transform.position, Quaternion.identity);
    }

   
    void Update()
    {
        
    }
}
