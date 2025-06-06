using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSFX;

    void Start()
    {
        sliderMusic.value = AudioController.Instance.MusicVolume;
        sliderSFX.value = AudioController.Instance.SFXVolume;
    }

    public void Close()
    {
        UIController.Instance.OpenPanel(PanelType.Menu);
    }

    public void SetMusic()
    {
        AudioController.Instance.SetMusicVolume(sliderMusic.value);
    }

    public void SetSFX()
    {
        AudioController.Instance.SetSFXVolume(sliderSFX.value);
    }
}
