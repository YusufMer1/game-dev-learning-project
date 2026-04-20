using UnityEngine;

public class shotgunAudios : MonoBehaviour
{
    public AudioSource hitthegun;
    public AudioSource reloadshotguns;

    public void hitthegunaudio()
    {
        hitthegun.Play();
        
    }
    public void reloadshotgun()
    {
        reloadshotguns.Play();
    }
}
