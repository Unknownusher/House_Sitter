using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour {

    //[SerializeField] string targetSceneName;
    [SerializeField] GameObject[] tips = new GameObject[5];

    [SerializeField] GameObject[] items;

    [SerializeField] GameObject playButton;
    [SerializeField] GameObject LoadingText;
    [SerializeField] GameObject LoadingIcon;


    private void Start() {

        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;

        StartCoroutine(LoadGame());

        if (currentSceneName == "LoadingScene")
            GenerateTip();

        else {

            foreach (var item in items)
                item.SetActive(false);

        }

    }

    public void GenerateTip() {

        int randomIndex = UnityEngine.Random.Range(0, 5);

        foreach (var tip in tips)
            tip.SetActive(false);

        tips[randomIndex].SetActive(true);

    }

    public void LoadNext() {

        Scene currentScene = SceneManager.GetActiveScene();
        int NextSceneIndex = currentScene.buildIndex + 1;

        SceneManager.LoadScene(NextSceneIndex);

    }

    IEnumerator LoadGame() {

        yield return new WaitForSeconds(3f);

        LoadingText.SetActive(false);
        LoadingIcon.SetActive(false);

        playButton.SetActive(true);

    }

}
