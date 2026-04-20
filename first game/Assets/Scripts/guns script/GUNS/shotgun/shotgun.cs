using bullet.fx.pack;
using System.Collections;
using TMPro;
using UnityEngine;

public class shotgun : MonoBehaviour
{
    Animator animator;
    [Header("Settings")]
    public bool canItFire;
    float innerfireFrequency;
    public float outerfireFrequency;
    public float range;
    
    
    
    
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
    public GameObject bullet1;
    public GameObject bullet2;
    public GameControl gc;
    

    void Start()
    {
        
        gc = FindFirstObjectByType<GameControl>();
        animator = GetComponentInParent<Animator>();
        
    }
    void OnEnable()
    {

        allbulletnumber = PlayerPrefs.GetInt(gunname + " bullet");//we take bullet number as we can use playerprefs from creating gamecontrol script
        remainingbullets = PlayerPrefs.GetInt(gunname + " remainingbullet");
        //beginningreloadbullet();

        remainingbullet_Text.text = remainingbullets.ToString();
        allbullet_Text.text = allbulletnumber.ToString();
        StartCoroutine(FixShellVisibilityNextFrame());
    }
    IEnumerator FixShellVisibilityNextFrame()
    {
        yield return null; // 1 frame bekle
        UpdateShellVisibility();
    }


    void UpdateShellVisibility()
    {
        bullet1.SetActive(remainingbullets >= 1);
        bullet2.SetActive(remainingbullets >= 2);
    }

    IEnumerator waitaftershoot()
    {

        canItFire = false;
        yield return new WaitForSeconds(0.25f);
        canItFire = true;


    }
    private void Update()
    {
      
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canItFire == true && Time.time > innerfireFrequency && !animator.GetCurrentAnimatorStateInfo(0).IsName("reload") && remainingbullets != 0)
            {

                fire();
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
                animator.Play("shotgun reload anim");
                StartCoroutine(MagnumCoroutine());
            }
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            takebullet();
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
                allbulletnumber += bulletnumber;
                PlayerPrefs.SetInt("shotgun bullet", PlayerPrefs.GetInt("shotgun bullet") + bulletnumber);
                allbullet_Text.text = allbulletnumber.ToString();
                break;
            case "Sniper":
                PlayerPrefs.SetInt("Sniper bullet", PlayerPrefs.GetInt("Sniper bullet") + bulletnumber);
                break;
        }
    }

    
    void fire()
    {
 
        canItFire = false;
        StartCoroutine(shakecamera(.10f, .2f));
        firesound.Play();
        fireEffect.Play();
        animator.Play("Shoutgun fire anim");
        RaycastHit hit;

        GameObject obj = Instantiate(kovan, pointOfComingKovan.transform.position,pointOfComingKovan.transform.rotation);
        Rigidbody rbobj = obj.GetComponent<Rigidbody>();
        rbobj.AddRelativeForce(new Vector3(-10f, 1, 0) * 30);
        int i = 0;
        while(i != 10)
        {
            Vector3 direction = mycam.transform.forward;
            direction += mycam.transform.right * Random.Range(-0.1f, 0.1f);
            direction += mycam.transform.up * Random.Range(-0.1f, 0.1f);
            if (Physics.Raycast(mycam.transform.position, direction, out hit, range))
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
                Debug.Log(hit.transform.name);
            }
            i++;
        }

        remainingbullets--;
        PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
        remainingbullet_Text.text = remainingbullets.ToString();
        if(remainingbullets == 0 && allbulletnumber != 0)
        {
            animator.Play("shotgun reload anim");

            StartCoroutine(fullMagnumCoroutine());
        }


    }



    IEnumerator fullMagnumCoroutine()
    {
        canItFire = false;
        gc.isReloading = true;
        yield return new WaitForSeconds(2.7f);
        if (allbulletnumber >= magnumcapacity)
        {
            remainingbullets = magnumcapacity;
            allbulletnumber = allbulletnumber - magnumcapacity;
            PlayerPrefs.SetInt(gunname +  " bullet", allbulletnumber);
            PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            remainingbullet_Text.text = remainingbullets.ToString();
            allbullet_Text.text = allbulletnumber.ToString();
        }
        else
        {
            remainingbullets = allbulletnumber;
            allbulletnumber = 0;
            PlayerPrefs.SetInt(gunname + " bullet", 0);
            PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
            remainingbullet_Text.text = remainingbullets.ToString();
            allbullet_Text.text = allbulletnumber.ToString();
            
        }
        canItFire = true;
        gc.isReloading = false;
    }
    IEnumerator MagnumCoroutine()
    {
        gc.isReloading = true;
        canItFire= false;
        yield return new WaitForSeconds(2.7f);
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
        canItFire = true;
        gc.isReloading = false;
    }

    IEnumerator shakecamera(float shaketime, float magnitude)   
    {
        Vector3 original_position = mycam.transform.localPosition;

        float passingtime = 0.0f;
        while(passingtime < shaketime)
        {
            float x = Random.Range(-1,1) * magnitude;
            mycam.transform.localPosition =new Vector3(x,original_position.y,original_position.z);
            passingtime += Time.deltaTime;
            yield return null;
        }
        mycam.transform.localPosition = original_position;
    }


}
