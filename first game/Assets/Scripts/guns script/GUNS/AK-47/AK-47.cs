using System.Collections;
using TMPro;
using UnityEngine;

public class AK47 : MonoBehaviour
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
    public GameControl gc;
    public GameObject bullet_out_point;
    public GameObject bullet;

    
    


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
        Debug.Log("EonEnable is active");
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
    

    private void Update()
    {
        
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (canItFire == true && Time.time > innerfireFrequency && !animator.GetCurrentAnimatorStateInfo(0).IsName("reload") && remainingbullets != 0)
            {
                fire();
                innerfireFrequency = Time.time + outerfireFrequency;
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
                animator.Play("reload");
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
        if(Physics.Raycast(mycam.transform.position,mycam.transform.forward,out hit, 4f))
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
                allbulletnumber += bulletnumber;
                PlayerPrefs.SetInt("AK-47 bullet", allbulletnumber);
                allbullet_Text.text = allbulletnumber.ToString();
                break;
            case "Magnum":
                PlayerPrefs.SetInt("Magnum bullet", PlayerPrefs.GetInt("Magnum bullet") + bulletnumber);
                break;
            case "shotgun":
                PlayerPrefs.SetInt("shotgun bullet", PlayerPrefs.GetInt("shotgun bullet") + bulletnumber);
                break;
            case "Sniper":
                PlayerPrefs.SetInt("Sniper bullet", PlayerPrefs.GetInt("Sniper bullet") + bulletnumber);
                break;
        }
    }

    
    void fire()
    {
        StartCoroutine(shakecamera(0.05f, 0.1f));
        firesound.Play();
        fireEffect.Play();
        animator.Play("fire");
        RaycastHit hit;

        GameObject obj = Instantiate(kovan, pointOfComingKovan.transform.position,pointOfComingKovan.transform.rotation);
        Rigidbody rbobj = obj.GetComponent<Rigidbody>();
        rbobj.AddRelativeForce(new Vector3(-10f, 1, 0) * 30);


        Instantiate(bullet, bullet_out_point.transform.position, bullet_out_point.transform.rotation);

        if (Physics.Raycast(mycam.transform.position,mycam.transform.forward,out hit, range))
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

        remainingbullets--;
        PlayerPrefs.SetInt(gunname + " remainingbullet", remainingbullets);
        remainingbullet_Text.text = remainingbullets.ToString();
        if(remainingbullets == 0 && allbulletnumber != 0)
        {
            animator.Play("reload");
            StartCoroutine(fullMagnumCoroutine());
            
        }

    }
    IEnumerator fullMagnumCoroutine()
    {
        canItFire = false;
        gc.isReloading = true;
        yield return new WaitForSeconds(1.90f);
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
        gc.isReloading = false;
        canItFire = true;
    }
    IEnumerator MagnumCoroutine()
    {
        canItFire = false;
        gc.isReloading = true;
        yield return new WaitForSeconds(1.90f);
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
    IEnumerator shakecamera(float shaketime, float magnitude)
    {
        Vector3 original_position = mycam.transform.localPosition;

        float passingtime = 0.0f;
        while (passingtime < shaketime)
        {
            float x = Random.Range(-1, 1) * magnitude;
            mycam.transform.localPosition = new Vector3(x, original_position.y, original_position.z);
            passingtime += Time.deltaTime;
            yield return null;
        }
        mycam.transform.localPosition = original_position;
    }


}
