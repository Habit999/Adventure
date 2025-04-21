using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the boss room exit
/// </summary>

public class BossRoomExit : MonoBehaviour
{
    [SerializeField] private Animator endingAnimator;
    [SerializeField] private AnimationClip endingClip;

    private IEnumerator EndRoutine()
    {
        endingAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(endingClip.length);
        GameManager.Instance.ReturnToMenu();
    }

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if(enterTrigger.gameObject.tag == "Player")
        {
            StartCoroutine(EndRoutine());
            GameManager.Instance.CurrentGameState = GameManager.GameStates.MainMenu;
        }
    }
}
