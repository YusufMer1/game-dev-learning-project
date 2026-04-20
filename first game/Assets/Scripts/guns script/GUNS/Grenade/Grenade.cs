using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour 
{
    public int power;
    public int range;
    public int upforce;
    public ParticleSystem exploringeffect;
    AudioSource exploringvoice;

    private void Start()
    {
        StartCoroutine(Exploring());
        exploringvoice = GetComponent<AudioSource>();
    }
    IEnumerator Exploring()
    {
        yield return new WaitForSeconds(2f);
        exploring();

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }

    void exploring()
    {
        Vector3 exploringposition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(exploringposition, range);
        exploringeffect.Play();
        exploringvoice.Play();   
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(hit!=null && rb != null)
            {
                rb.AddExplosionForce(power,exploringposition,range,upforce,ForceMode.Impulse);
                if (hit.gameObject.CompareTag("enemy"))
                {
                    if (hit.transform.gameObject.GetComponent<Enemy>().health > 0)
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().takeAttack(300f);
                    }
                        
                }
            }
        }
    }
    
}
