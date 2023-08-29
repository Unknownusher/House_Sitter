using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class Increment_Realistic : MonoBehaviour {

    [SerializeField] Text secondText;
    [SerializeField] Text tensText;
    [SerializeField] Text hourText;

    [SerializeField] Animator JumpscareAnimator;
    [SerializeField] Animator cameraAnimator;

    [SerializeField] Animator JeremyAnimator;

    [SerializeField] AudioSource flickingSound;
    [SerializeField] AudioSource JeremyJumpscareSound;
    [SerializeField] AudioSource PeepsJumpscareSound;
    [SerializeField] AudioSource CrooketJumpscareSound;
    [SerializeField] AudioSource RatSoarJumpscareSound;
    [SerializeField] AudioSource ClydeJumpscareSound;
    [SerializeField] AudioSource DoorSlamSource;
    [SerializeField] AudioSource creakSound;
    [SerializeField] AudioSource phoneSource;

    [SerializeField] AudioSource ambientSource;

    [SerializeField] AudioSource Window;
    [SerializeField] AudioSource doorWindow;

    [SerializeField] AudioSource[] vents;

    [SerializeField] AudioClip jumpscareClip;
    [SerializeField] AudioClip jumpscareClip2;
    [SerializeField] AudioClip jumpscareClip3;
    [SerializeField] AudioClip jumpscareClip4;
    [SerializeField] AudioClip doorSlam;
    [SerializeField] AudioClip runningSound;
    [SerializeField] AudioClip groan;

    [SerializeField] Camera m_Camera;
    [SerializeField] Transform jeremy;

    [SerializeField] Transform chair;
    [SerializeField] Transform peeps;
    [SerializeField] Transform crooket;
    [SerializeField] GameObject clyde;

    [SerializeField] GameObject ClosetDoor;
    [SerializeField] GameObject flashlight;

    [SerializeField] GameObject HUD;

    bool GameWon = false;

    bool isJeremyActivated = false,
         isPeepsActivated = false,
         isRatSoarActivated = false,
         isClydeActivated = false,
         isCrooketActivated = false;

    bool shouldJumpscare = false;

    float visibleTimer = 0f;
    float visibilityDuration = 1.5f;

    float peepsTimer = 0f;
    float peepsDeathTimer = 0f;

    float peepsDeathDuration = 3f;
    float peepsDuration = 3f;

    float crooketTimer = 0f;
    float crooketDuration = 4f;

    float RatSoarTimer = 0f;
    float RatSoarDuration = 0.5f;

    float clydeTimer = 0f;
    float clydeFlashTimer = 0f;
    float clydeDuration = 5f;

    bool ventsPaused = false;

    private void Start() => StartCoroutine(CountTime());

    private void Update() {

        bool isPaused = Time.timeScale == 0f;

        if (isPaused && vents[0].isPlaying && vents[1].isPlaying) {

            foreach (var vent in vents)
                vent.Pause();

            ventsPaused = true;

        }

        if (!isPaused && ventsPaused) {

            foreach (var vent in vents)
                vent.UnPause();

            ventsPaused = false;

        }

        //Check if jeremy is visible in the camera view
        bool jeremyVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()), jeremy.GetComponent<Renderer>().bounds);
        bool peepsVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()), peeps.GetComponent<Renderer>().bounds);
        bool crooketVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()), crooket.GetComponent<Renderer>().bounds);
        bool clydeVisible = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(GetComponent<Camera>()), clyde.GetComponent<Renderer>().bounds);

        bool isMoving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

        if (jeremyVisible && jeremy.gameObject.activeSelf) {

            visibleTimer += Time.deltaTime;

            if (visibleTimer >= visibilityDuration)
                StartCoroutine(JeremyJumpscare());

        }

        if (!jeremy.gameObject.activeSelf)
            visibleTimer = 0f;

        if (peepsVisible && !ClosetDoor.activeSelf && !flashlight.activeSelf) {

            peepsTimer += Time.deltaTime;

            if (peepsTimer >= peepsDuration)
            {

                ClosetDoor.SetActive(true);
                creakSound.PlayOneShot(doorSlam);
                peepsTimer = 0f;
                peepsDeathTimer = 0f;

            }

        }

        //if peeps within camera view with door open and flashlight on OR peeps isn't visible while the door is open
        if ((peepsVisible && !ClosetDoor.activeSelf && flashlight.activeSelf) || (!peepsVisible && !ClosetDoor.activeSelf)) {

            peepsDeathTimer += Time.deltaTime;

            if (peepsDeathTimer >= peepsDeathDuration)
                StartCoroutine(PeepsJumpscare());

        }

        if ((!crooketVisible || (crooketVisible && !flashlight.activeSelf)) && crooket.gameObject.activeSelf) {

            crooketTimer += Time.deltaTime;

            if (crooketTimer >= crooketDuration)
                StartCoroutine(CrooketJumpscare());

        }

        if (crooketVisible && flashlight.activeSelf && crooket.gameObject.activeSelf) {

            crooket.gameObject.SetActive(false);
            crooketTimer = 0f;

            Window.PlayOneShot(runningSound);

        }

        if ((!clydeVisible || (clydeVisible && !flashlight.activeSelf)) && clyde.activeSelf)
        {
        
            clydeTimer += Time.deltaTime;

            if (clydeTimer >= clydeDuration)
                StartCoroutine(ClydeJumpscare());

        }

        if (clydeVisible && flashlight.activeSelf && clyde.activeSelf) {
        
            clydeFlashTimer += Time.deltaTime;

            if (clydeFlashTimer >= 3f) {

                clyde.SetActive(false);

                clydeTimer = 0f;
                clydeFlashTimer = 0f;

                doorWindow.PlayOneShot(groan);

            }

        }

        foreach (var vent in vents) {

            if (vent.isPlaying && isMoving) {

                RatSoarTimer += Time.deltaTime;

                if (RatSoarTimer >= RatSoarDuration)
                    StartCoroutine(RatSoarJumpscare());

            }

            if (!vent.isPlaying)
                RatSoarTimer = 0f;

        }

        if (GameWon) {

            //load next scene

        }

        //Debug.Log("Clyde Timer: " + clydeTimer + "s");

    }

    IEnumerator CountTime() {

        while (!GameWon) {

            //yield return new WaitForSeconds(60f);

            yield return new WaitForSeconds(0.025f);

            int second = Convert.ToInt32(secondText.text);
            int tens = Convert.ToInt32(tensText.text);
            int hours = Convert.ToInt32(hourText.text);
            second++;

            if (second > 9) {

                second = 0;
                tens++;

            }

            if (tens > 5) {

                tens = 0;
                hours++;

            }

            if (hours > 12)
                hours = 1;

            if (hours < 12 && isJeremyActivated == false) { //if the hour is 1am or further and jeremy isn't activated yet

                ActivateJeremy();
                isJeremyActivated = true;

            }

            if (((hours == 12 && tens >= 4) || hours < 12) && isPeepsActivated == false) //12:40 or later
            {

                ActivatePeeps();
                isPeepsActivated = true;

            }

            //if (hours == 12 && tens < 4 && !phoneSource.isPlaying)
            //    tens = 4;

            if (hours < 12 && tens >= 3 && isCrooketActivated == false) { //1:30

                ActivateCrooket();
                isCrooketActivated = true;

            }

            if (hours < 12 && hours >= 2 && isRatSoarActivated == false) { //2:00

                ActivateRatSoar();
                isRatSoarActivated = true;

            }

            if (hours < 12 && hours >= 2 && tens >= 4 && second >= 5 && isClydeActivated == false) //2:45
            {

                ActivateClyde();
                isClydeActivated = true;

            }

            secondText.text = Convert.ToString(second);
            tensText.text = Convert.ToString(tens);
            hourText.text = Convert.ToString(hours);

            if (hours == 6) {

                DeactivateEnemies();
                ambientSource.Stop();

                jeremy.gameObject.SetActive(false);
                peeps.gameObject.SetActive(false);
                crooket.gameObject.SetActive(false);
                clyde.SetActive(false);

                cameraAnimator.SetBool("TurnRight", true);
                JumpscareAnimator.SetBool("TurnRight", true);

            }

        }

    }

    public void ActivateJeremy() => StartCoroutine(Jeremy());
    public void ActivatePeeps() => StartCoroutine(Peeps());
    public void ActivateCrooket() => StartCoroutine(Crooket());
    public void ActivateRatSoar() => StartCoroutine(RatSoar());
    public void ActivateClyde() => StartCoroutine(Clyde());

    void DeactivateEnemies() => StopAllCoroutines();

    IEnumerator Jeremy() {

        while (!GameWon) {

            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 50f));
            JeremyAnimator.SetBool("isWarning", true);
            flickingSound.Play();

            yield return new WaitForSeconds(JeremyAnimator.GetCurrentAnimatorStateInfo(0).length);
            flickingSound.Stop();
            yield return new WaitForSeconds(2f);

            flickingSound.gameObject.SetActive(false);

            JeremyAnimator.SetBool("isWarning", false);
            JeremyAnimator.SetBool("isAttacking", true);
            JeremyAnimator.SetBool("failed", false);

            yield return new WaitForSeconds(JeremyAnimator.GetCurrentAnimatorStateInfo(0).length); //wait for door to shut
            yield return new WaitForSeconds(6f);

            flickingSound.gameObject.SetActive(true);

            JeremyAnimator.SetBool("isAttacking", false);
            JeremyAnimator.SetBool("isWarning", false);
            JeremyAnimator.SetBool("failed", true);

            DoorSlamSource.Play();

         }

    }

    IEnumerator JeremyJumpscare() {

        JumpscareAnimator.SetBool("JeremyJumpscare", true);
        JeremyJumpscareSound.PlayOneShot(jumpscareClip);

        HUD.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Jeremy_GameOverScene_REALISTIC_HARD");

     }

    IEnumerator Peeps() {

        while (!GameWon) {

            yield return new WaitForSeconds(UnityEngine.Random.Range(30f, 50f));

            ClosetDoor.SetActive(false);
            creakSound.Play();

        }

    }

    IEnumerator PeepsJumpscare() {

        JumpscareAnimator.SetBool("PeepsJumpscare", true);
        PeepsJumpscareSound.PlayOneShot(jumpscareClip);

        HUD.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Peeps_GameOverScene_REALISTIC_HARD");

    }

    IEnumerator Crooket() {
       
        while (!GameWon) {

            float randomSec = UnityEngine.Random.Range(30f, 60f);

            yield return new WaitForSeconds(randomSec);

            crooket.gameObject.SetActive(true);
            Window.Play();

            //Debug.Log("Crooket Active: " + crooket.gameObject.activeSelf);

        }

     }

    IEnumerator CrooketJumpscare() {

        JumpscareAnimator.SetBool("CrooketJumpscare", true);
        CrooketJumpscareSound.PlayOneShot(jumpscareClip2);

        HUD.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Crooket_GameOverScene_REALISTIC_HARD");

    }

    IEnumerator RatSoar() {

        while (!GameWon) {

            yield return new WaitForSeconds(UnityEngine.Random.Range(40, 60));

            foreach (var vent in vents)
                vent.Play();

        }
    }

    IEnumerator RatSoarJumpscare() {

        JumpscareAnimator.SetBool("RatSoarJumpscare", true);
        RatSoarJumpscareSound.PlayOneShot(jumpscareClip3);

        HUD.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("RatSoar_GameOverScene_REALISTIC_HARD");

    }

    IEnumerator Clyde() {

        while (!GameWon) {

            yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 60f));

            clyde.SetActive(true);
            doorWindow.Play();
        
        }

    }

    IEnumerator ClydeJumpscare() {

        JumpscareAnimator.SetBool("ClydeJumpscare", true);
        ClydeJumpscareSound.PlayOneShot(jumpscareClip4);

        HUD.SetActive(false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Clyde_GameOverScene_REALISTIC_HARD");

    }

}
