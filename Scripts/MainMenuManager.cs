using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] Animator colorAnimator;

    [SerializeField] AudioMixer musicMixer;
    [SerializeField] AudioMixer SFXMixer;
    [SerializeField] AudioMixer mahSexyVoiceMixer;
    [SerializeField] AudioMixer jumpscareMixer;

    [SerializeField] Button normalButton;
    [SerializeField] Button hardButton;
    [SerializeField] Button realisticButton;

    [SerializeField] Light bulb;
    [SerializeField] Light bulb1;

    [SerializeField] AudioSource leftBulb;
    [SerializeField] AudioSource rightBulb;

    readonly List<int> widths = new List<int>() { 3840, 2560, 1920, 1280, 960 };
    readonly List<int> heights = new List<int>() { 2160, 1440, 1080, 800, 540 };

    private void Start() => StartCoroutine(Thunder());

    public void NewGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void Quit() => Application.Quit();

    public void NewGameHard() => SceneManager.LoadScene("Load Hard Game");
    public void NewGameRealistic() => SceneManager.LoadScene("Load Realistic Game");

    public void SetMusicVolume(float volume) => musicMixer.SetFloat("musicVolume", volume);
    public void SetSFXVolume(float volume) => SFXMixer.SetFloat("SFXVolume", volume);
    public void SetMahSexyVoiceVolume(float volume) => mahSexyVoiceMixer.SetFloat("mahSexyVoiceVolume", volume);
    public void SetJumpscareVolume(float volume) => jumpscareMixer.SetFloat("jumpscareVolume", volume);

    public void SetFullScreen(bool isFullScreen) => Screen.fullScreen = isFullScreen;
    public void SetQuality(int qualityIndex) => QualitySettings.SetQualityLevel(qualityIndex);

    public void SetResolution(int resolutionIndex) {

        bool isFullScreen = Screen.fullScreen;

        int width  =  widths[resolutionIndex];
        int height = heights[resolutionIndex];

        Screen.SetResolution(width, height, isFullScreen);
        Debug.Log("The resolution is now " + width + " x " + height);

    }


    //problem is in the unity editor: light bulb's still appear to be yellow or red, nothing wrong with code
    void Update() {

        Color yellar = new Color(0.99489f, 1f, 0.7217f, 1f);
        Color red = new Color(0.7f, 0f, 0f, 1f);
        Color orange = new Color(0.7f, 0.4f, 0f, 1f);

        if (bulb.color == yellar)
            Debug.Log("Light bulbs' current color is yellow");

        if (bulb.color == red)
            Debug.Log("Light bulbs' current color is red");

        if (bulb.color == orange)
            Debug.Log("Light bulbs' current color is orange");

    }

    public void OnNormalButtonClick() {

        Color red = new Color(0.7f, 0f, 0f, 1f);
        Color yellar = new Color(0.99489f, 1f, 0.7217f, 1f);
        Color orange = new Color(0.7f, 0.4f, 0f, 1f);

        if (bulb.color == red) {

            colorAnimator.SetBool("wasRealistic", true);
            colorAnimator.SetBool("wasHard", false);
        
        }

        if (bulb.color == orange) {

            colorAnimator.SetBool("wasHard", true);
            colorAnimator.SetBool("wasRealistic", false);

        }

        colorAnimator.SetBool("wasNormal", false);
        StartFlicker();

        colorAnimator.SetBool("isHard", false);
        colorAnimator.SetBool("isRealistic", false);
        colorAnimator.SetBool("isNormal", true);

        bulb.color = yellar;
        bulb1.color = yellar;

    }

    public void OnHardButtonClick() {

        Color red = new Color(0.7f, 0f, 0f, 1f);
        Color yellar = new Color(0.99489f, 1f, 0.7217f, 1f);
        Color orange = new Color(0.7f, 0.4f, 0f, 1f);

        if (bulb.color == red) {

            colorAnimator.SetBool("wasRealistic", true);
            colorAnimator.SetBool("wasNormal", false);
        
        }

        if (bulb.color == yellar) {

            colorAnimator.SetBool("wasNormal", true);
            colorAnimator.SetBool("wasRealistic", false);

        }

        colorAnimator.SetBool("wasHard", false);
        StartFlicker();

        colorAnimator.SetBool("isHard", true);
        colorAnimator.SetBool("isRealistic", false);
        colorAnimator.SetBool("isNormal", false);

        bulb.color = orange;
        bulb1.color = orange;

    }

    public void OnRealisticButtonClick() {

        Color red = new Color(0.7f, 0f, 0f, 1f);
        Color yellar = new Color(0.99489f, 1f, 0.7217f, 1f);
        Color orange = new Color(0.7f, 0.4f, 0f, 1f);

        if (bulb.color == yellar) {

            colorAnimator.SetBool("wasNormal", true);
            colorAnimator.SetBool("wasHard", false);

        }

        if (bulb.color == orange) {

            colorAnimator.SetBool("wasHard", true);
            colorAnimator.SetBool("wasNormal", false);

        }

        colorAnimator.SetBool("wasRealistic", false);
        StartFlicker();

        colorAnimator.SetBool("isRealistic", true);
        colorAnimator.SetBool("isHard", false);
        colorAnimator.SetBool("isNormal", false);

        bulb.color = red;
        bulb1.color = red;

    }

    void StartFlicker() {

        leftBulb.Play();
        rightBulb.Play();

        colorAnimator.SetBool("flicker", true);
        Invoke("StopFlicker", 1.0f);

    }

    void StopFlicker() {

        leftBulb.Stop();
        rightBulb.Stop();

        colorAnimator.SetBool("flicker", false);

    }

    IEnumerator Thunder() {

        while (true) {

            float randomValue = UnityEngine.Random.Range(15f, 30f);

            yield return new WaitForSeconds(randomValue);
            animator.SetBool("Lightning", true);

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animator.SetBool("Lightning", false);

        }

    }

}
