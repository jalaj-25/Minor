using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public Slider bgSlider;
    public Slider sfxSlider;
    public TMP_InputField nameInput;

    void Start()
    {
        // 🎯 Load saved values
        bgSlider.value = PlayerPrefs.GetFloat("BGVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        nameInput.text = PlayerPrefs.GetString("PlayerName", "Player");
    }

    // 🎵 BG volume change
    public void OnBGChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetBGVolume(value);
    }

    // 🔊 SFX volume change
    public void OnSFXChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }

    // 👤 Name change
    public void OnNameChanged(string newName)
    {
        PlayerPrefs.SetString("PlayerName", newName);
    }
}