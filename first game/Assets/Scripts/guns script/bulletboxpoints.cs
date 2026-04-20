using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class bulletboxpoints : MonoBehaviour
{
     public List<GameObject> points = new List<GameObject>();
     public GameObject bulletboxself;
     public static bool hasbulletbox;
    private void Start()
    {
        hasbulletbox = false;
        StartCoroutine(Createbulletbox());
    }
    IEnumerator Createbulletbox()
    {
        while (true)
        {
            yield return null; //bu kod sistemi her saniye kontrol eder 
                               //sahnede mermi olmadðý an itibariyle sayaç baþlýyor bu sayede
            
            if (!hasbulletbox)
            {
                yield return new WaitForSeconds(5f);
                int random = Random.Range(0, 5);
                Instantiate(bulletboxself, points[random].transform.position, points[random].transform.rotation);
                hasbulletbox = true;
            }
            
        }
        
        
    }
}

