using UnityEngine;

public class FirstAid : MonoBehaviour
{
    public ParticleSystem particle;
    void Start()
    {
        Instantiate(particle, transform.position, Quaternion.identity, transform);

    }

    
    void Update()
    {
        
    }
}
