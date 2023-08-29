using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseSettings : MonoBehaviour {

    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;
    [SerializeField] AudioMixer mahSexyVoiceMixer;
    [SerializeField] AudioMixer jumpscareMixer;

    readonly List<int> widths = new List<int>() { 3840, 2560, 1920, 1280, 960 };
    readonly List<int> heights = new List<int>() { 2160, 1440, 1080, 800, 540 };

    public void SetMusicVolume(float volume) => musicMixer.SetFloat("musicVolume", volume);
    public void SetSFXVolume(float volume) => SFXMixer.SetFloat("SFXVolume", volume);
    public void SetMahSexyVoiceVolume(float volume) => mahSexyVoiceMixer.SetFloat("mahSexyVoiceVolume", volume);
    public void SetJumpscareVolume(float volume) => jumpscareMixer.SetFloat("jumpscareVolume", volume);

    public void SetFullScreen(bool isFullScreen) => Screen.fullScreen = isFullScreen;
    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

    public void SetResolution(int resolutionIndex) {

        bool isFullScreen = Screen.fullScreen;

        int width = widths[resolutionIndex];
        int height = heights[resolutionIndex];

        Screen.SetResolution(width, height, isFullScreen);
        Debug.Log("The resolution is now " + width + " x " + height);

    }

}
