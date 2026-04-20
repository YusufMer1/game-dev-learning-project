using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class MainMenuScripts : MonoBehaviour
{
    public Camera maincam;

    public Camera settingscam;

    public Camera gamecam;

    public Canvas maincanvas;

    public Canvas secondcanvas;

    public scenescripts scenescripts;

    public Volume vo;

    public gameaudio gavo;

    
    private void Start()
    {
        maincam.enabled = true;
        settingscam.enabled = false;
        gamecam.enabled = false;
        gavo.gameaudio1.mute = true;
        

        
 
    }

    public void settings1()
    {
        maincam.enabled = false;
        settingscam.enabled = true;
        gamecam.enabled = false;
        maincanvas.gameObject.SetActive(false);
        secondcanvas.gameObject.SetActive(true);
    }

    public void mainmenuback()
    {
        maincam.enabled = true;
        settingscam.enabled = false;
        gamecam.enabled = false;
        maincanvas.gameObject.SetActive(true);
        secondcanvas.gameObject.SetActive(false);
    }

    public void gamecam1()
    {
        maincam.enabled = false;
        settingscam.enabled = false;
        gamecam.enabled = true;
        maincanvas.enabled = false;
        scenescripts.character.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        scenescripts.scenerb.bodyType = RigidbodyType2D.Dynamic;
        scenescripts.scenerb2.bodyType = RigidbodyType2D.Dynamic;
        scenescripts.scenerb3.bodyType = RigidbodyType2D.Dynamic;
        scenescripts.scenerb4.bodyType = RigidbodyType2D.Dynamic;
        
    }

    public void gamequit()
    {
        Application.Quit();
    }

}
