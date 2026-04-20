using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    public float health;
    GameObject mainControl;
    public float power;
    Animator anim;
    bool isDead = false;
    AudioSource voice;
    bool voiceplaying;
    public float voicetime;
    bool isdead = false;
    
    
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mainControl = GameObject.FindWithTag("maincontrol");
        voice = GetComponent<AudioSource>();
    }

    public void indicateTarget(GameObject obj)
    {
        target = obj;
    }
    void Update()
    {
        if (target == null || isDead) return;
        agent.SetDestination(target.transform.position);


        if(!voiceplaying && isDead == false)
        {
            StartCoroutine(outVoice());
        }
    }
    IEnumerator outVoice()
    {
        voiceplaying = true;
        voice.Play();
        yield return new WaitForSeconds(voicetime);
        voiceplaying = false;
    }
    public void takeAttack(float attack)
    {
        if(isdead == false)
        {
            health -= attack;
            if (health <= 0)
            {
                voice.mute = true;
                isdead = true;
                dead();
            }
        }
        
    }
    public void dead()
    {
        isDead = true;
        mainControl.GetComponent<GameControl>().update_enemy_number();
        anim.SetTrigger("DÝE");
        
        Destroy(gameObject,5f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("target"))
        {
            
            mainControl.GetComponent<GameControl>().takeAttack(power);
            dead();

        }
    }
    

}
