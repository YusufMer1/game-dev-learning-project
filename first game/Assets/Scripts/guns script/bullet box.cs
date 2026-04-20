using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class bulletbox : MonoBehaviour
{
    
    string[] guns =
    {
            "AK-47",
            "Magnum",
            "shotgun",
            "Sniper"
    };
    int[] numberofbullet =
    {
            20,
            30,
            50,
            10
    };
    public List<Sprite> guns_pircture = new List<Sprite>();
    public string guntype;
    public int bulletnumber;
    public Image pictureofgun;
    

    private void Start()
    {
        int randomkey = Random.Range(0, 4);
        guntype = guns[randomkey];
        bulletnumber = numberofbullet[Random.Range(0,numberofbullet.Length -1)];
        pictureofgun.sprite = guns_pircture[randomkey]; pictureofgun.enabled = true;    
       

    }
    private void Update()
    {
        
    }
    void producebullet()
    {
        
    }
}
