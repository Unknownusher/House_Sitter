using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOn : MonoBehaviour {

    public void SetActiveFalse() => StartCoroutine(TurnOff());

    IEnumerator TurnOff() {

        yield return new WaitForSeconds(10f);

        gameObject.SetActive(false);

    }

}
