using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour {

    [SerializeField] Animator animator;

    void Start() => StartCoroutine(TransitionText());

    IEnumerator TransitionText() {

        yield return new WaitForSeconds(4f);
        animator.SetBool("transition", true);

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}
