using Unity.VisualScripting;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public scenescripts mv;

    public CharacterController cs;

    public GameObject pausepanel;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P)))
        {
            if (!pausepanel.activeSelf)
            {
                onpause();
            }
            else
            {
                continue1();
            }
            
        }
    }
    public void onpause()
    {
        Time.timeScale = 0f;
        pausepanel.SetActive(true);
    }
    public void continue1()
    {
        Time.timeScale = 1f;
        pausepanel.SetActive(false);
    }

}
