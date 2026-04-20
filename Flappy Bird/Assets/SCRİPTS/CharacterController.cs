using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject character;

    public Rigidbody2D rb1;
    public Rigidbody2D rb2;
    public Rigidbody2D rb3;
    public Rigidbody2D rb4;

    public scenescripts scs;
    

    public Canvas gameover;

    private void Start()
    {
        rb1 = character.GetComponent<Rigidbody2D>();
        gameover.enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb1.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("StopObject"))
        {
            Debug.Log("scene stops");
            scs.move = false;
            scs.scenerb.bodyType = RigidbodyType2D.Static;
            scs.scenerb2.bodyType = RigidbodyType2D.Static;
            scs.scenerb3.bodyType = RigidbodyType2D.Static;
            scs.scenerb4.bodyType = RigidbodyType2D.Static;
            rb1.bodyType = RigidbodyType2D.Static;
            /*rb2.bodyType = RigidbodyType2D.Static;
            rb3.bodyType = RigidbodyType2D.Static;
            rb4.bodyType = RigidbodyType2D.Static;*/
            
            gameover.enabled = true;

            if(scs.highscore < scs.score)
            {
                scs.highscore = scs.score;
            }
        }
    }
}
