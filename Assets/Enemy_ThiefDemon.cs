using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ThiefDemon : Enemy
{
    private Transform playerTarget;

    protected override void EnemyBehaviour()
    {
        switch (CurrentState)
        {
            case EnemyState.Roaming:
                Roaming();
                break;

            case EnemyState.Targeting:
                Targeting();
                break;

            default:
                break;
        }
    }

    protected override void Roaming()
    {
        base.Roaming();


    }

    private void Targeting()
    {

    }
}
