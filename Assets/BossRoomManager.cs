using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    [SerializeField] private Enemy_BossDemon bossDemon;
    [SerializeField] private Animator exitGateAnimator;

    private void Awake()
    {
        bossDemon.RoomManager = this;
    }

    public void OpenExitGate(Enemy boss)
    {
        if(boss == null) return;
        exitGateAnimator.SetTrigger("Open");
    }
}
