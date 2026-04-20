using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioSource audio2;
    public Slider volumeslider;
    public Toggle toggle1;

    


    private void Start()
    {
        audio2 = GetComponent<AudioSource>();
        volumeslider.value = audio2.volume;
        volumeslider.onValueChanged.AddListener(setvolume);
        toggle1.onValueChanged.AddListener(mute1);

    }

    public void setvolume(float val)
    {
        audio2.volume = val;
    }
    public void mute1(bool val)
    {
        audio2.mute = val;
    }

    
    
}
