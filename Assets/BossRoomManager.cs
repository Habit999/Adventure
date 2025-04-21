using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages essential events in boss room
/// </summary>

public class BossRoomManager : MonoBehaviour
{
    [SerializeField] private Enemy_BossDemon bossDemon;
    [SerializeField] private Animator exitGateAnimator;
    [SerializeField] private BoxCollider blockingWall;

    private void Awake()
    {
        bossDemon.RoomManager = this;

        blockingWall.enabled = false;
    }

    private void Start()
    {
        GameManager.Instance.GivePlayerData();
    }

    public void OpenExitGate(Enemy boss)
    {
        if(boss == null) return;
        exitGateAnimator.SetTrigger("Open");
    }

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if (enterTrigger.CompareTag("Player"))
        {
            blockingWall.enabled = true;
        }
    }
}
