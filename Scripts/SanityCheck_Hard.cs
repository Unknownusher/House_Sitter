using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SanityCheck_Hard : MonoBehaviour {

    [SerializeField] Slider sanityMeter;

    [SerializeField] Text hours;
    [SerializeField] Text minutes;

    [SerializeField] Transform ceilingLight;
    [SerializeField] Camera camera;

    const int SIZE = 4;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] AudioClips = new AudioClip[SIZE];

    [SerializeField] Animator jumpscareAnimator;
    [SerializeField] GameObject HUD;

    Coroutine decreaseCoroutine;
    Coroutine increaseCoroutine;

    // Update is called once per frame
    void Update() {

        bool ceilingLightVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()), ceilingLight.GetComponent<Renderer>().bounds);

        // if it's 12:40 or later and light isn't visible
        if (sanityMeter.value > 0f && ((hours.text == "12" && Convert.ToInt32(minutes.text) >= 4) || Convert.ToInt32(hours.text) < 12) && !ceilingLightVisible && decreaseCoroutine == null) {

            decreaseCoroutine = StartCoroutine(DecreaseSanity());

        }
        
        if (ceilingLightVisible && decreaseCoroutine != null) {

            StopCoroutine(decreaseCoroutine);
            decreaseCoroutine = null;

        }

        if (sanityMeter.value < 100f && increaseCoroutine == null && ceilingLightVisible) {

            increaseCoroutine = StartCoroutine(IncreaseSanity());

        }
        
        if (!ceilingLightVisible && increaseCoroutine != null) {

            StopCoroutine(increaseCoroutine);
            increaseCoroutine = null;

        }

        if (sanityMeter.value == 0f)
            StartCoroutine(RandomJumpscare());

        //Debug.Log("Should Decrease Sanity: " + (sanityMeter.value > 0f && ((hours.text == "12" && Convert.ToInt32(minutes.text) >= 4) || Convert.ToInt32(hours.text) < 12) && !ceilingLightVisible && decreaseCoroutine == null));

        if (hours.text == "6")
            StopAllCoroutines();

    }

    IEnumerator RandomJumpscare() {

        jumpscareAnimator.SetBool("SanityJumpscare", true);
        audioSource.PlayOneShot(AudioClips[0]);
        HUD.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Sanity_GameOverScene_REALISTIC_HARD");

    }

    IEnumerator DecreaseSanity() {
    
        while (sanityMeter.value > 0f) {

            yield return new WaitForSeconds(0.6f);
            sanityMeter.value--;

        }

    }

    IEnumerator IncreaseSanity() {

        while (sanityMeter.value < 100f) {

            yield return new WaitForSeconds(1f);
            sanityMeter.value++;

        }

    }

}
