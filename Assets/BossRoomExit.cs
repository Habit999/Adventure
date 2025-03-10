using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoomExit : MonoBehaviour
{
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private AnimationClip endingClip;

    private IEnumerator EndRoutine()
    {
        endingAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(endingClip.length);
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if(enterTrigger.gameObject.tag == "Player")
            StartCoroutine(EndRoutine());
    }
}
