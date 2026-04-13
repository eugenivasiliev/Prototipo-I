using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private Slider sliderMusic, sliderSFX;
        [SerializeField] private AudioSource musicSource, sfxSource;
        void Start()
        {
            sliderMusic.value = 0.25f;
            sliderSFX.value = 0.5f;
            SetVolumeMusic(sliderMusic.value);
            SetVolumeMusic(sliderSFX.value);
            sliderMusic.onValueChanged.AddListener(SetVolumeMusic);
            sliderSFX.onValueChanged.AddListener(SetVolumeSFX);
        }
        void SetVolumeMusic(float value)
        {
            musicSource.volume = value;
        }
        void SetVolumeSFX(float value)
        {
            sfxSource.volume = value;
        }
    }
}