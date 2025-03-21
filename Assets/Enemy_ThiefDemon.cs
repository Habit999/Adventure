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
    [SerializeField] private float fleeingSpeed;
    [SerializeField] private float fleeingRotationSpeed;

    [Space(5)]
    [SerializeField] private float timeToExitFlee;

    [Space(5)]
    [SerializeField] private float distanceToSteal;

    [Space(5)]
    [SerializeField] private Transform hands;

    [Space(5)]
    [SerializeField][Range(-1, 1)] private float fieldOfViewRange;
    [SerializeField][Range(-1, 1)] private float playerCanSeeRange;

    private PlayerController playerTarget;

    private SphereCollider sphereCollider;

    private GameObject stolenItem;

    private float fleeExitTimer;

    private bool isInRange;
    private bool isInView;

    protected override void Awake()
    {
        base.Awake();

        sphereCollider = GetComponent<SphereCollider>();

        fleeExitTimer = timeToExitFlee;

        isInRange = false;
        isInView = false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(stolenItem != null)
        {
            playerTarget.InventoryMngr.AddItem(stolenItem, 1);
            stolenItem.transform.parent = null;
            stolenItem = null;
        }

        SwitchState(EnemyState.Fleeing);
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

        if(stolenItem == null && isInView) SwitchState(EnemyState.Targeting);
    }

    private void Targeting()
    {
        navAgent.SetDestination(playerTarget.transform.position);

        // Close enough to steal?
        if(Vector3.Distance(transform.position, playerTarget.transform.position) <= distanceToSteal)
        {
            if(StealItem()) EquipStolenItem();
            SwitchState(EnemyState.Fleeing);
        }

        // Is player looking this way?
        if(Vector3.Dot(playerTarget.transform.forward, (transform.position - playerTarget.transform.position).normalized) > playerCanSeeRange)
        {
            SwitchState(EnemyState.Fleeing);
        }

        CheckIfStuck();
    }

    private void Fleeing()
    {
        if (!hasDestination)
        {
            if (!GenerateRandomNavLocation())
            {
                fleeExitTimer = timeToExitFlee;
                return;
            }
        }
        if (isInView)
        {
            fleeExitTimer = timeToExitFlee;
        }

        fleeExitTimer -= Time.deltaTime;

        navAgent.SetDestination(movePoint);

        if(fleeExitTimer <= 0)
        {
            SwitchState(EnemyState.Roaming);
        }

        CheckIfStuck();
    }

    private bool StealItem()
    {
        if (playerTarget.InventoryMngr.CollectedItems.Count > 0)
        {
            int randomItem = Random.Range(0, playerTarget.InventoryMngr.CollectedItems.Count - 1);
            stolenItem = playerTarget.InventoryMngr.RemoveItem(playerTarget.InventoryMngr.CollectedItems.Keys.ElementAt(randomItem), 1);
            return true;
        }
        else return false;
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
        if(Vector3.Dot(transform.forward, playerDirection) >= fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, sphereCollider.radius))
            {

                if (hit.collider.transform == playerTarget.transform)
                {
                    isInView = true;
                    return;
                }
            }
        }
        isInView = false;
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

            case EnemyState.Fleeing:
                navAgent.speed = fleeingSpeed;
                navAgent.angularSpeed = fleeingRotationSpeed;
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
