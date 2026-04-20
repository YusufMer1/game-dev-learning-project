using UnityEngine;

public class emptyshellcasing : MonoBehaviour
{
    AudioSource voicefeltTodown;
    private void Start()
    {
        voicefeltTodown = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("çarpýþma oldu");
        if (collision.gameObject.CompareTag("floor"))
        {
            Debug.Log("the voice is need to play");
            voicefeltTodown.Play();
        }
       
    }
    private void Update()
    {
        Destroy(gameObject, 2f);
    }
}
