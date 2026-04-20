using UnityEngine;
using UnityEngine.UI;

public class gameaudio : MonoBehaviour
{
    public AudioSource gameaudio1;

    public Slider gameslider;

    private void Start()
    {
        gameaudio1 = GetComponent<AudioSource>();
        gameslider.value = gameaudio1.volume;
        gameslider.onValueChanged.AddListener(ongameaudio);
    }

    public void ongameaudio(float val)
    {
        gameaudio1.volume = val;
    }
}
