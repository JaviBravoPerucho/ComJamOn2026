using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class VolumeManager : MonoBehaviour
{
    [Header("UI")]
    public Scrollbar volumeScrollbar;

    private Bus bus;

    void Start()
    {

        bus = RuntimeManager.GetBus("bus:/");

        // Cargar valor guardado
        float savedVolume = PlayerPrefs.GetFloat("bus:/", 1f);
        volumeScrollbar.value = savedVolume;
        bus.setVolume(savedVolume);

        volumeScrollbar.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        bus.setVolume(value);
        PlayerPrefs.SetFloat("bus:/", value);
    }
}