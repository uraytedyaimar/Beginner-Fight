using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour {
    public static VolumeSettings Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private float musicVolume;
    private float sfxVolume;

    public event EventHandler OnMusicVolumeChanged;
    public event EventHandler OnSFXVolumeChanged;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if(PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume")) {
            LoadVolume();
        } else {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume() {
        musicVolume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(musicVolume) *20);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSFXVolume() {
        sfxVolume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        OnSFXVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetMusicVolume() {
        return musicVolume;
    }

    public float GetSFXVolume() {
        return sfxVolume;
    }

    private void LoadVolume() {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
