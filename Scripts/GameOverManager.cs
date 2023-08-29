using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] JeremyClips;
    [SerializeField] AudioClip[] PeepsClips;
    [SerializeField] AudioClip[] CrooketClips;
    [SerializeField] AudioClip[] ClydeClips;

    public void Retry() {

        string sceneName = SceneManager.GetActiveScene().name;

        string targetSceneName;

        if (sceneName.EndsWith('D'))
            targetSceneName = "Gameplay Scene Hard";

        else if (sceneName.EndsWith('C'))
            targetSceneName = "Gameplay Scene Realistic";

        else
            targetSceneName = "Gameplay Scene";

        SceneManager.LoadScene(targetSceneName);

    }

    public void    QuitToMenu() => SceneManager.LoadScene("TitleScreen");
    public void QuitToDesktop() => Application.Quit();

    void Start() {

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (sceneIndex) { //evaluate scene index value

            //scene index for normal and hard/realstic Jeremy
            case 8 or 14:

                PlayRandomClip(JeremyClips);

            break;

            //scene index for normal and hard/realstic Peeps
            case 9 or 15:
                
                PlayRandomClip(PeepsClips);

            break;

            //scene index for normal and hard/realstic Crooket
            case 10 or 16:

                PlayRandomClip(CrooketClips);

            break;

            //scene index for normal and hard/realstic Clyde
            case 12 or 18:

                PlayRandomClip(ClydeClips);

            break;

        }

    }

    void PlayRandomClip(AudioClip[] audioClips) => audioSource.PlayOneShot(audioClips[UnityEngine.Random.Range(0, audioClips.Length)]);

}
