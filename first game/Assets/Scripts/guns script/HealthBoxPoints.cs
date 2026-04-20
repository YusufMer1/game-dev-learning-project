using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class HealthBoxPoints : MonoBehaviour
{
     public List<GameObject> points = new List<GameObject>();
     public GameObject healthboxself;
     public static bool hashealthbox;
     
    private void Start()
    {
        hashealthbox = false;
        StartCoroutine(Createhealthbox());
    }
    IEnumerator Createhealthbox()
    {
        while (true)
        {
            yield return null; //bu kod sistemi her saniye kontrol eder 
                               //sahnede mermi olmadðý an itibariyle sayaç baþlýyor bu sayede
            
            if (!hashealthbox)
            {
                yield return new WaitForSeconds(5f);
                int random = Random.Range(0, 2);
                Instantiate(healthboxself, points[random].transform.position, points[random].transform.rotation);
                hashealthbox = true;
                
            }
            
        }
        
        
    }
}

