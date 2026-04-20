using UnityEngine;

public class GameOverScripts : MonoBehaviour
{
    public MainMenuScripts mainmenu;

    public scenescripts scenescripts;

    public CharacterController cc;

    public Canvas maincanvas;

    public MainMenuScripts mainmenus;

    
    public void yesclick()
    {
        scenescripts.Startscene();

        cc.rb1.bodyType = RigidbodyType2D.Dynamic;

        cc.rb2.bodyType = RigidbodyType2D.Dynamic;

        cc.rb3.bodyType = RigidbodyType2D.Dynamic;

        cc.rb4.bodyType = RigidbodyType2D.Dynamic;

        scenescripts.scenerb.bodyType = RigidbodyType2D.Dynamic;

        scenescripts.scenerb2.bodyType = RigidbodyType2D.Dynamic;

        scenescripts.scenerb3.bodyType = RigidbodyType2D.Dynamic;

        scenescripts.scenerb4.bodyType = RigidbodyType2D.Dynamic;

        
    }
    public Volume vo;
    public void noclick()
    {
        

        cc.gameover.enabled = false;

        scenescripts.gamecam.enabled = false;

        maincanvas.enabled = true;

        mainmenus.maincam.enabled = true;

        

       

    }
}
