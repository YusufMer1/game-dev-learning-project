
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Sniper : MonoBehaviour
{
    Animator animator;
    [Header("Settings")]
    public bool canItFire;
    float innerfireFrequency;
    public float outerfireFrequency;
    public float range;
    public GameObject snip_scope1;
    

    [Header("Audios")]
    public AudioSource firesound;
    public AudioSource reload;
    public AudioSource finishbullet;
    public AudioSource takebullets;

    [Header("Effects")]
    public ParticleSystem bulletmark;
    public ParticleSystem bloodeffect;
    public ParticleSystem fireEffect;

    [Header("Anothers")]
    public Camera mycam;
    float pointOfView;
    float variable_zoom=20;

    [Header("gun settings")]
    int remainingbullets;
    public int magnumcapacity;
    int allbulletnumber;
    public TextMeshProUGUI remainingbullet_Text;
    public TextMeshProUGUI allbullet_Text;
    public string gunname;
    public float gunPower;

    public bool isComeoutKovan;
    public GameObject kovan;
    public GameObject pointOfComingKovan;
    GameControl gc;

    
    


    void Start()
    {
        
        gc = FindFirstObjectByType<GameControl>();
        animator = GetComponentInParent<Animator>();
        pointOfView = 55.3f;    
    }
    void OnEnable()
    {
        allbulletnumber = PlayerPrefs.GetInt(gunname + " bullet");//we take bullet number as we can use playerprefs from creating gamecontrol script
        remainingbullets = PlayerPrefs.GetInt(gunname + " remainingbullet");
        //beginningreloadbullet();

        remainingbullet_Text.text = remainingbullets.ToString();
        allbullet_Text.text = allbulletnumber.ToString();
    }
    void beginningreloadbullet()
    {
        

        if (allbulletnumber <= magnumcapacity)
        {
            remainingbullets = allbulletnumber;
            allbulletnumber = 0;
            PlayerPrefs.SetInt(gunname + " bullet", allbulletnumber);          
        }
        else
        {
            
            remainingbullets = magnumcapacity;
            allbulletnumber -= magnumcapacity;
            PlayerPrefs.SetInt(gunname + " bullet", allbulletnumber);
        }
    }
    
    IEnumerator waitaftershoot()
    {
        canItFire = false;
        yield return new WaitForSeconds(2f);
        canItFire = true;
       
}
    private void Update()
    {
        
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canItFire == true && Time.time > innerfireFrequency && !animator.GetCurrentAnimatorStateInfo(0).IsName("reload") && remainingbullets != 0)
            {
                fire();
                fireEffect.Play();
                innerfireFrequency = Time.time + outerfireFrequency;
                StartCoroutine(waitaftershoot());
            }
            else if(!finishbullet.isPlaying && remainingbullets == 0)
            {
                finishbullet.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(allbulletnumber != 0 && remainingbullets != magnumcapacity)
            {
                animator.Play("Snip_reload");
                StartCoroutine(MagnumCoroutine());
            }
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            takebullet();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetBool("do_zoom", true);
            StartCoroutine(make_zoom());
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("do_zoom", false);
            StartCoroutine(leave_zoom());
        }
        remainingbullet_Text.text = remainingbullets.ToString();
        allbullet_Text.text = allbulletnumber.ToString();
        if (Input.GetKeyDown(KeyCode.E))
        {
            takeMedKit();
        }
    }
    void takeMedKit()
    {

        RaycastHit hit;
        if (Physics.Raycast(mycam.transform.position, mycam.transform.forward, out hit, 4f))
        {

            if (hit.transform.gameObject.CompareTag("health_box"))
            {

                GameControl gc = FindFirstObjectByType<GameControl>();
                gc.AddHealth();


                Destroy(hit.transform.gameObject);
                
                HealthBoxPoints.hashealthbox = false;
            }
        }
    }
    IEnumerator make_zoom()
    {
        yield return new WaitForSeconds(0.1f);
        mycam.fieldOfView = variable_zoom;
        snip_scope1.SetActive(true);
        mycam.cullingMask = ~(1 << 6); //burda 6. kamerada 6.katmana sahip layerların görünmeisni engelliyorum

    }
    IEnumerator leave_zoom()
    {
        yield return new WaitForSeconds(0.01f);
        mycam.fieldOfView = pointOfView;
        snip_scope1.SetActive(false);
        mycam.cullingMask = -1; //burdada tüm katmanları göster diyorum
       
       
    }

    void takebullet()
    {
        RaycastHit hit;
        if(Physics.Raycast(mycam.transform.position,mycam.transform.forward, out hit, 4f))
        {
            if (hit.transform.gameObject.CompareTag("bullet"))
            {
                savebullet(hit.transform.gameObject.GetComponent<bulletbox>().guntype, hit.transform.gameObject.GetComponent<bulletbox>().bulletnumber);
                //hit.transform.gameObject.GetComponent<bulletbox>().guntype;
                Destroy(hit.transform.parent.gameObject);
                bulletboxpoints.hasbulletbox = false;
                
            }
        }

        
    }
    void savebullet(string guntype, int bulletnumber)
    {
        takebullets.Play();
        switch (guntype)
        {
            case "AK-47":
                PlayerPrefs.SetInt("AK_47 bullet", PlayerPrefs.GetInt("AK_47 bullet") + bulletnumber);
                break;
            case "Magnum":
                PlayerPrefs.SetInt("Magnum bullet", PlayerPrefs.GetInt("Magnum bullet") + bulletnumber);
                break;
            case "shotgun":
                
                PlayerPrefs.SetInt("shotgun bullet", PlayerPrefs.GetInt("shotgun bullet") + bulletnumber);
                break;
            case "Sniper":
                allbulletnumber += bulletnumber;
                PlayerPrefs.SetInt("Sniper bullet", allbulletnumber);
                allbullet_Text.text = allbulletnumber.ToString();
                break;
        }
    }

    
    void fire()
    {
        
        
        animator.Play("snip_fire");
        
        RaycastHit hit;

        GameObject obj = Instantiate(kovan, pointOfComingKovan.transform.position,pointOfComingKovan.transform.rotation);
        Rigidbody rbobj = obj.GetComponent<Rigidbody>();
        rbobj.AddRelativeForce(new Vector3(-10f, 1, 0) * 30);

        if(Physics.Raycast(mycam.transform.position,mycam.transform.forward,out hit, range))
        {
            if (hit.transform.gameObject.CompareTag("enemy"))
            {
                Instantiate(bloodeffect, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Enemy>().takeAttack(gunPower);
            }
            else
            {
                Instantiate(bulletmark, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        remainingbullets--;
        PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
        remainingbullet_Text.text = remainingbullets.ToString();
        if(remainingbullets == 0 && allbulletnumber != 0)
        {
            StartCoroutine(fullMagnumCoroutine());
            animator.Play("Snip_reload");
        }
  

    }
    

    IEnumerator fullMagnumCoroutine()
    {
        gc.isReloading = true;
        canItFire = false;
        yield return new WaitForSeconds(3.8f);
        if (allbulletnumber >= magnumcapacity)
        {
            
            remainingbullets = magnumcapacity;
            allbulletnumber = allbulletnumber - magnumcapacity;
            PlayerPrefs.SetInt(gunname +  " bullet", allbulletnumber);
            PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            remainingbullet_Text.text = remainingbullets.ToString();
            allbullet_Text.text = allbulletnumber.ToString();
            canItFire = true;
        }
        else
        {
            
            remainingbullets = allbulletnumber;
            allbulletnumber = 0;
            PlayerPrefs.SetInt(gunname + " bullet", 0);
            PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            remainingbullet_Text.text = remainingbullets.ToString();
            allbullet_Text.text = allbulletnumber.ToString();
            canItFire= true;
        }
        gc.isReloading = false;
    }
    IEnumerator MagnumCoroutine()
    {
        gc.isReloading = true;
        canItFire = false;
        yield return new WaitForSeconds(3.8f);
        if(allbulletnumber <= magnumcapacity)
        {
            int allbulletandremainingbullet = remainingbullets + allbulletnumber;
            if(allbulletandremainingbullet >= magnumcapacity)
            {
                remainingbullets = magnumcapacity;
                allbulletnumber = allbulletandremainingbullet - magnumcapacity;
                PlayerPrefs.SetInt(gunname + " bullet", allbulletnumber);
                PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
                
            }
            else
            {
                remainingbullets += allbulletnumber;
                allbulletnumber = 0;
                PlayerPrefs.SetInt(gunname + " bullet", 0);
                PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            }
            allbullet_Text.text = allbulletnumber.ToString();
            remainingbullet_Text.text = remainingbullets.ToString();
            
        }
        else
        {
            allbulletnumber -= (magnumcapacity - remainingbullets);
            remainingbullets = magnumcapacity;
            PlayerPrefs.SetInt(gunname + " bullet", allbulletnumber);
            PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            remainingbullet_Text.text = remainingbullets.ToString();
            allbullet_Text.text = allbulletnumber.ToString();
            
        }
        gc.isReloading = false;
        canItFire = true;
    }


}
