using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        // Valor inicial
        slider.value = AudioListener.volume;

        // Listener executado sempre que o slider for movido
        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float v)
    {
        AudioListener.volume = v;
    }
}
