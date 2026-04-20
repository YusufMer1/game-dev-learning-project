using System.Collections;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl:MonoBehaviour
{
    int activeGun›ndex;
    public GameObject mycam;
    [Header("Guns Settings")]
    public GameObject[] guns;
    public AudioSource changeGunAudio;
    public GameObject bombpoint;
    public GameObject grenade;

    [Header("Enemy Settings")]
    public GameObject[] Enemys;
    public GameObject[] bornPoint;
    public GameObject[] targetPoint;
    public float timeComingEnemy;
    public TextMeshProUGUI enemy_number_Text;
    public int Beginning_enemy_number;
    public int Enemy_number;

    [Header("Another Settings")]
    public GameObject GameOverCanvas;
    public GameObject WinCanvas; 

    public float health;
    public Image healthBar;
    
    
    public TextMeshProUGUI bomb_number;
    public bool isReloading;
    public AK47 ak;
    public shotgun shotgun1;
    public Sniper sniper;
    
    
    
    
    private void Start()
    {
        ak = FindFirstObjectByType<AK47>();
        shotgun1 = FindFirstObjectByType<shotgun>();
        sniper = FindFirstObjectByType<Sniper>();
        first_applications();
        isReloading = false;
        
        StartCoroutine(spawnEnemys());
    }

    void first_applications()
    {
        PlayerPrefs.SetInt("AK_47 bullet", 70);
        if (!PlayerPrefs.HasKey("startgame")) //this condition will start if it has never started before. 
        {
            PlayerPrefs.SetInt("AK_47 bullet", 70);
            PlayerPrefs.SetInt("AK_47 remainingbullet", 10);
            PlayerPrefs.SetInt("Magnum bullet", 20);
            PlayerPrefs.SetInt("shotgun bullet", 30);
            PlayerPrefs.SetInt("Sniper bullet", 15);
            PlayerPrefs.SetInt("Bomb_number", 2);



            PlayerPrefs.SetInt("startgame", 1); //as we write it, we create a key, so when the gam start again, dont work this condition
        }
        enemy_number_Text.text = Beginning_enemy_number.ToString();
        Enemy_number = Beginning_enemy_number;

        bomb_number.text = PlayerPrefs.GetInt("Bomb_number").ToString();
        

        activeGun›ndex = 0; //user start the game with AK-47
        
    }
    public void update_enemy_number()
    {
        Enemy_number--;
        if(Enemy_number <= 0)
        {
            StartCoroutine(Win());
        }
        enemy_number_Text.text = Enemy_number.ToString();
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(3f);
        WinCanvas.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerPrefs.SetInt("Bomb_number", PlayerPrefs.GetInt("Bomb_number") + 1);
        bomb_number.text = PlayerPrefs.GetInt("Bomb_number").ToString();
    }
    

    public void AddHealth()
    {
        if(health <= 70)
        {
            health += 30;
        }
        else
        {
            health = 100;
        }
            healthBar.fillAmount = health / 100;
    }

    IEnumerator spawnEnemys()
    {
        int num = Beginning_enemy_number;
        while (num != 0)
        {
                yield return new WaitForSeconds(timeComingEnemy);

                int enemytype = Random.Range(0, 5);
                int bornplace = Random.Range(0, 2);
                int targetplace = Random.Range(0, 2);
                GameObject obj = Instantiate(Enemys[enemytype], bornPoint[bornplace].transform.position, Quaternion.identity);
                obj.GetComponent<Enemy>().indicateTarget(targetPoint[targetplace]);
                num--;
            
            
        }
    }


    bool CanChangeGun()
    {
       
        if (isReloading)
            return false;
        if (activeGun›ndex == 0 && ak != null)
            return true;

        if (activeGun›ndex == 1 && shotgun1 != null)
            return shotgun1.canItFire;

        if (activeGun›ndex == 2 && sniper != null)
            return sniper.canItFire;

        return true;
    }


    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && CanChangeGun())
            changeTheGun(0);

        if (Input.GetKeyDown(KeyCode.Alpha2) && CanChangeGun())
            changeTheGun(1);

        if (Input.GetKeyDown(KeyCode.Alpha3) && CanChangeGun())
            changeTheGun(2);

        if (Input.GetKeyDown(KeyCode.Q) && CanChangeGun())
            changeTheGunWithQ();

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (PlayerPrefs.GetInt("Bomb_number") >0)
            {
                GameObject obj = Instantiate(grenade, bombpoint.transform.position, bombpoint.transform.rotation);
                Rigidbody rg = obj.GetComponent<Rigidbody>();
                Vector3 angle = Quaternion.AngleAxis(90, mycam.transform.forward) * mycam.transform.forward;
                rg.AddForce(angle * 150f);
                PlayerPrefs.SetInt("Bomb_number", PlayerPrefs.GetInt("Bomb_number") - 1);
                bomb_number.text = PlayerPrefs.GetInt("Bomb_number").ToString();
            }
            
        }
    }
    public void takeAttack(float power)
    {
        health -= power;
        healthBar.fillAmount = health / 100;
        if(health <= 0)
        {
            gameOver();
        }
    }
    void gameOver()
    {
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        
    }
    public void changeTheGun(int index)
    { 
        activeGun›ndex = index;
        changeGunAudio.Play();
        foreach (GameObject gun in guns)
        {
            gun.SetActive(false);
        }
        guns[index].SetActive(true);
    }
    public void changeTheGunWithQ()
    {
        
        
            int newactiveindex;
            activeGun›ndex++;
            changeGunAudio.Play();
            newactiveindex = activeGun›ndex % guns.Length;
            foreach (GameObject gun in guns)
            {
                gun.SetActive(false);
            }
            guns[newactiveindex].SetActive(true);

        

    }
    public void TryAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Bas˝ld˝");

    }
}
