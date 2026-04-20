using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;
using System.Linq.Expressions;

public class scenescripts : MonoBehaviour
{
    public Camera gamecam;

    public GameObject character;

    public bool move = false;

    public Vector2 target = new Vector2(-457.1f, -619.5f);

    public GameObject scene;
    public GameObject scene2;
    public GameObject scene3;
    public GameObject scene4;

    public Rigidbody2D scenerb;
    public Rigidbody2D scenerb2;
    public Rigidbody2D scenerb3;
    public Rigidbody2D scenerb4;

    public Vector2 targetscene = new Vector2(-332.5f, -620f);
    public Vector2 targetscene2 = new Vector2(-52.5f, -619.7f);
    public Vector2 targetscene3 = new Vector2(227.4f, -619f);
    public Vector2 targetscene4 = new Vector2(507f, -619.5f);

    

    public float maxspeed = 100f;

    

    public CharacterController characterc;

    private void Start()
    {
        highscoretext.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        scenerb = scene.GetComponent<Rigidbody2D>();
        scenerb2 = scene2.GetComponent<Rigidbody2D>();
        scenerb3 = scene3.GetComponent<Rigidbody2D>();
        scenerb4 = scene4.GetComponent<Rigidbody2D>();
        

    }
    public Volume vo;
    public gameaudio gavo; 
   
    public void Startscene()
    {

        if (gamecam.enabled == true)
        {
            Debug.Log("hareket baþlamasý lazým");

            characterc.gameover.enabled = false;

            character.transform.position = target;

            scene.transform.position = targetscene;

            scene2.transform.position = targetscene2;

            scene3.transform.position = targetscene3;

            scene4.transform.position = targetscene4;

            StartCoroutine(waitsecond());

            textscore.text = 0.ToString();
            score = 0;
            
        }
    }
    public float timer = 0;
    public float updateýnterval = 1f;
    public TextMeshProUGUI textscore;
    public int score = 0;
    public int highscore = 0;
    public TextMeshProUGUI highscoretext;
    public int boundryscore = 30;
  
    private void Update()
    {
        if(score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            highscoretext.text = score.ToString();
        }
        if (move)
        {
            scenerb.AddForce(Vector2.left.normalized * 50);

            scenerb2.AddForce(Vector2.left.normalized * 50);

            scenerb3.AddForce(Vector2.left.normalized * 50);

            scenerb4.AddForce(Vector2.left.normalized * 50);

            timer += Time.deltaTime;
            
            if(timer >= updateýnterval)
            {
                score++;
                textscore.text = score.ToString();
                timer = 0;
            }

            if(score > boundryscore)
            {
                boundryscore += 30;
                maxspeed += 8;
            }
                
        }
        if(scene.transform.position.x < -612.5)
        {
            scene.transform.position = targetscene4;
        }
        if(scene2.transform.position.x < -612.5)
        {
            scene2.transform.position = targetscene4;
        }
        if (scene3.transform.position.x < -612.5)
        {
            scene3.transform.position = targetscene4;
        }
        if (scene4.transform.position.x < -612.5)
        {
            scene4.transform.position = targetscene4;
        }
        if(gamecam.enabled == true)
        {
            gavo.gameaudio1.gameObject.GetComponent<AudioSource>().mute = false;
            vo.audio2.gameObject.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            gavo.gameaudio1.gameObject.GetComponent<AudioSource>().mute = true;
            vo.audio2.gameObject.GetComponent<AudioSource>().mute = false;
        }
            Debug.Log(scenerb.linearVelocity);
    }
    private void FixedUpdate()
    {
        if(scenerb.linearVelocity.magnitude >= maxspeed)
        {
            scenerb.linearVelocity = scenerb.linearVelocity.normalized * maxspeed;
        }
        if (scenerb2.linearVelocity.magnitude >= maxspeed)
        {
            scenerb2.linearVelocity = scenerb2.linearVelocity.normalized * maxspeed;
        }
        if (scenerb3.linearVelocity.magnitude >= maxspeed)
        {
            scenerb3.linearVelocity = scenerb3.linearVelocity.normalized * maxspeed;
        }
        if (scenerb4.linearVelocity.magnitude >= maxspeed)
        {
            scenerb4.linearVelocity = scenerb4.linearVelocity.normalized * maxspeed;
        }
    }

    IEnumerator waitsecond()
    {
        yield return new WaitForSeconds(3f);
        move = true;
    }
    
}
