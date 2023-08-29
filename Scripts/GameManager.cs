using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Animations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] GameObject Light;
    [SerializeField] GameObject Canvas;

    [SerializeField] GameObject explosions;
    [SerializeField] GameObject fire;

    [SerializeField] AudioSource lightSource;
    [SerializeField] AudioSource PhoneSource;

    [SerializeField] AudioClip lightSound1;
    [SerializeField] AudioClip lightSound2;
    [SerializeField] AudioClip pickUpClip;
    [SerializeField] AudioClip voiceLine;
    [SerializeField] AudioClip endClip;
    [SerializeField] AudioClip call;

    [SerializeField] Text hoursText;

    [SerializeField] GameObject SkipText;

    [SerializeField] Light light;

    [SerializeField] Animator animator;
    [SerializeField] Animator jumpscareAnimator;

    [SerializeField] Transform chair;

    [SerializeField] AudioSource[] explosionsAndFlames;

    [SerializeField] AudioSource blackCanvas;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject HUD;

    bool hasPlayed = false;
    bool shouldSkip = false;
    bool isEnded = false;

    float timer = 0f;

    void Start() => StartCoroutine(Thunder());

    void Awake() => PhoneSource.PlayOneShot(call);

    // Update is called once per frame
    void Update() {

        bool isPaused = Time.timeScale == 0f;

        int hours = Convert.ToInt32(hoursText.text);

        if (Input.GetKeyDown(KeyCode.F) && hours != 6 && !isPaused)
            lightSource.PlayOneShot(lightSound1);

        if (Input.GetKeyUp(KeyCode.F) && hours != 6 && !isPaused)
            lightSource.PlayOneShot(lightSound2);

        Light.SetActive(Input.GetKey(KeyCode.F) && hours != 6 && !isPaused);

        if (Input.GetKeyDown(KeyCode.A) && hours != 6)
            Rotate(-90f);

        else if (Input.GetKeyDown(KeyCode.D) && hours != 6 && !isPaused)
            Rotate(90f);

        if (hours == 6 && isEnded == false) {

            //chair.position = new Vector3(10.30661f, 1.078354f, -18.40996f);
            HUD.SetActive(false);
            Canvas.SetActive(false);
            chair.rotation = Quaternion.Euler(0f, -30f, 0f);
            PhoneSource.PlayOneShot(pickUpClip);
            PlayEndClip();

            isEnded = true;

        }

        if (hours == 6) {

            timer += Time.deltaTime;
            Debug.Log(timer + "s");

        }

        if (!PhoneSource.isPlaying && !hasPlayed && !shouldSkip && !isPaused) { 
        
            PhoneSource.PlayOneShot(pickUpClip);
            hasPlayed = true;

            StartCoroutine(PlayVoiceLine());

        }

    }

    public void PauseGame() {

        HUD.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        PhoneSource.Pause();

    }

    public void ResumeGame() {

        HUD.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
        PhoneSource.UnPause();

    }

    public void QuitToMenu() {

        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    
    }

    public void Skip() => shouldSkip = true;

    void PlayEndClip() => StartCoroutine(EndClip());

    void Rotate(float angle) {
        // Calculate the new rotation based on the input angle
        Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, angle, 0f);

        // Rotate the object smoothly using Slerp
        StartCoroutine(RotateSmoothly(newRotation, 10f));
    }

    IEnumerator RotateSmoothly(Quaternion targetRotation, float speed) {
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < 1f) {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure the final rotation is exact
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

    IEnumerator PlayVoiceLine() {

        yield return new WaitForSeconds(0.5f);
        PhoneSource.PlayOneShot(voiceLine);

        yield return new WaitForSeconds(voiceLine.length);
        SkipText.SetActive(false);

    }

    IEnumerator EndClip() {

        yield return new WaitForSeconds(1f);
        PhoneSource.PlayOneShot(endClip);

        yield return new WaitForSeconds(29f);
        jumpscareAnimator.SetBool("isEnd", true);

        yield return new WaitForSeconds(0.5f);
        explosions.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        fire.SetActive(true);

        foreach (var explode in explosionsAndFlames)
            explode.Play();

        yield return new WaitForSeconds(0.5f);
        jumpscareAnimator.SetBool("runAway", true);

        yield return new WaitForSeconds(jumpscareAnimator.GetCurrentAnimatorStateInfo(0).length);
        jumpscareAnimator.SetBool("lookLeft", true);

        yield return new WaitForSeconds(jumpscareAnimator.GetCurrentAnimatorStateInfo(0).length);
        jumpscareAnimator.SetBool("lookBehind", true);

        yield return new WaitForSeconds(jumpscareAnimator.GetCurrentAnimatorStateInfo(0).length);
        jumpscareAnimator.SetBool("moveRight", true);


        yield return new WaitForSeconds(jumpscareAnimator.GetCurrentAnimatorStateInfo(0).length - 1f);
        blackCanvas.gameObject.SetActive(true);
        blackCanvas.Play();
        StartCoroutine(LoadCredits());


    }

    IEnumerator LoadCredits() {

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("CreditsScene");

    }

}