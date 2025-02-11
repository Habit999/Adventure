using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy_ThiefDemon : Enemy
{
    [Space(5)]
    [SerializeField] private float targetingSpeed;
    [SerializeField] private float targetingRotationSpeed;

    [Space(5)]
    [SerializeField] private float distanceToSteal;

    [Space(5)]
    [SerializeField] private Transform hands;

    [Space(5)]
    [SerializeField][Range(-1, 1)] private float fieldOfView;

    private PlayerController playerTarget;

    private SphereCollider sphereCollider;

    private GameObject stolenItem;

    private bool isInRange;
    private bool isInView;

    protected override void Awake()
    {
        base.Awake();

        sphereCollider = GetComponent<SphereCollider>();

        isInRange = false;
        isInView = false;
    }

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

            case EnemyState.Fleeing:
                Fleeing();
                break;

            default:
                break;
        }
    }

    protected override void Roaming()
    {
        base.Roaming();

        if(isInView ) SwitchState(EnemyState.Targeting);
    }

    private void Targeting()
    {
        navAgent.SetDestination(playerTarget.transform.position);

        // Close enough to steal?
        if(Vector3.Distance(transform.position, playerTarget.transform.position) <= distanceToSteal)
        {
            int randomItem = Random.Range(0, playerTarget.InventoryMngr.CollectedItems.Count - 1);
            stolenItem = playerTarget.InventoryMngr.RemoveItem(playerTarget.InventoryMngr.CollectedItems.Keys.ElementAt(randomItem), 1);
            EquipStolenItem();
            SwitchState(EnemyState.Fleeing);
        }
    }

    private void Fleeing()
    {

    }

    private void EquipStolenItem()
    {
        stolenItem = Instantiate(stolenItem, hands.position, Quaternion.identity);
        stolenItem.transform.parent = hands;
        stolenItem.SetActive(true);
    }

    private void CheckPlayerInView()
    {
        Vector3 playerDirection = (playerTarget.transform.position - transform.position).normalized;
        if(Vector3.Dot(transform.forward, playerDirection) >= fieldOfView)
        {
            if(Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, sphereCollider.radius))
            {
                if(hit.collider.transform == playerTarget)
                {
                    isInView = true;
                }
                else isInView = false;
            }
            else isInView = false;
        }
        else isInView = false;
    }

    protected override void CheckSpeed()
    {
        switch (CurrentState)
        {
            case EnemyState.Roaming:
                navAgent.speed = roamingSpeed;
                navAgent.angularSpeed = roamingRotationSpeed;
                break;

            case EnemyState.Targeting:
                navAgent.speed = targetingSpeed;
                navAgent.angularSpeed = targetingRotationSpeed;
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if(enterTrigger.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInRange = true;
            if(playerTarget == null) playerTarget = enterTrigger.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerStay(Collider stayTrigger)
    {
        if (isInRange)
        {
            CheckPlayerInView();
        }
    }

    private void OnTriggerExit(Collider exitTrigger)
    {
        if (exitTrigger.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInRange = false;
            isInView = false;
        }
    }
}
