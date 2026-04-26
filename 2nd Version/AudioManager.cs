using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource bgSource; // ONLY ONE SOURCE

    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("SFX")]
    public AudioClip earnMoneySFX;
    public AudioClip teacherLeaveSFX;
    public AudioClip dayChangeSFX;
    public AudioClip newBuildingSFX;
    public AudioClip fullCapacitySFX;
    public AudioClip updateBuildingClickSFX;

    [Header("Volume")]
    [Range(0f, 1f)] public float bgVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadAudioSettings();
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (bgSource == null || backgroundMusic == null) return;

        bgSource.clip = backgroundMusic;
        bgSource.loop = true;
        bgSource.volume = bgVolume;
        bgSource.Play();
    }

    // 🔥 SAME SOURCE USED
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || bgSource == null) return;

        bgSource.PlayOneShot(clip, sfxVolume);
    }

    // 🎯 SOUND FUNCTIONS
    public void PlayMoneySound() => PlaySFX(earnMoneySFX);
    public void PlayTeacherLeaveSound() => PlaySFX(teacherLeaveSFX);
    public void PlayDayChangeSound() => PlaySFX(dayChangeSFX);
    public void PlayNewBuildingSound() => PlaySFX(newBuildingSFX); // 🔧 FIXED NAME
    public void PlayFullCapacity() => PlaySFX(fullCapacitySFX);
    public void UpdateBuildingClick() => PlaySFX(updateBuildingClickSFX);

    // 🎚️ SLIDER FUNCTIONS
    public void SetBGVolume(float value)
    {
        bgVolume = value;
        bgSource.volume = bgVolume;
        PlayerPrefs.SetFloat("BGVolume", bgVolume);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    void LoadAudioSettings()
    {
        bgVolume = PlayerPrefs.GetFloat("BGVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}