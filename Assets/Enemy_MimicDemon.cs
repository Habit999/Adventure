using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_MimicDemon : Enemy
{
    [HideInInspector] public LootSpawnManager LootManager;

    [Space(5)]
    [SerializeField] private float targetingSpeed;
    [SerializeField] private float targetingRotationSpeed;

    [Space(5)]
    [SerializeField] private float fleeingSpeed;
    [SerializeField] private float fleeingRotationSpeed;

    [Space(5)]
    [SerializeField] private float timeToExitFlee;

    [Space(5)]
    [SerializeField] private float damageDistance;
    [SerializeField] private float damage;

    [Space(5)]
    [SerializeField][Range(-1, 1)] private float fieldOfViewRange;

    private Transform moveTarget;

    private PlayerController playerTarget;

    private SphereCollider sphereCollider;

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
        if (!hasDestination)
        {
            if (FindClosestMimicObject())
                hasDestination = true;
            else
            {
                base.Roaming();
            }
        }
        if(hasDestination && moveTarget == null)
        {
            base.Roaming();
            return;
        }

        if (moveTarget.GetComponent<TreasureChest>().IsOpen)
        {
            FindClosestMimicObject();
            return;
        }

            navAgent.SetDestination(movePoint);

        if(Vector3.Distance(transform.position, moveTarget.position) <= destinationStopDistance)
        {
            moveTarget.GetComponent<MimicComponent>().ActivateComponent(this);
            SwitchState(EnemyState.Hiding);
        }
    }

    private void Targeting()
    {
        if (!hasDestination)
        {
            moveTarget = playerTarget.transform;
        }

        navAgent.SetDestination(moveTarget.position);

        if(Vector3.Distance(transform.position, moveTarget.position) <= damageDistance)
        {
            playerTarget.DamagePlayer(damage);
            SwitchState(EnemyState.Fleeing);
        }
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

        if (fleeExitTimer <= 0)
        {
            SwitchState(EnemyState.Roaming);
        }
    }

    public override void SwitchState(EnemyState newState)
    {
        base.SwitchState(newState);
        if(newState == EnemyState.Hiding) gameObject.SetActive(false);
        else gameObject.SetActive(true);
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

    private bool FindClosestMimicObject()
    {
        if (LootManager.MimicObjects.Count > 0)
        {
            // Find closest mimic object
            GameObject closestMimicObject = null;
            float closestDistance = 0;
            foreach (var mimicObj in LootManager.MimicObjects)
            {
                if (mimicObj.GetComponent<TreasureChest>().IsOpen)
                    continue;

                if (closestMimicObject == null)
                {
                    closestMimicObject = mimicObj;
                    closestDistance = Vector3.Distance(transform.position, mimicObj.transform.position);
                    continue;
                }

                float mimicDistance = Vector3.Distance(transform.position, mimicObj.transform.position);
                if (mimicDistance < closestDistance)
                {
                    closestMimicObject = mimicObj;
                    closestDistance = mimicDistance;
                }
            }

            // Set move target
            if (closestMimicObject == null) return false;
            else
            {
                moveTarget = closestMimicObject.transform;
                if (NavMesh.SamplePosition(closestMimicObject.transform.position, out NavMeshHit hitData, 10, NavMesh.AllAreas))
                {
                    movePoint = hitData.position;
                    return hasDestination = true;
                }
                else return false;
            }
        }
        else return false;
    }

    private void CheckPlayerInView()
    {
        Vector3 playerDirection = (playerTarget.transform.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, playerDirection) >= fieldOfViewRange)
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

    private void OnTriggerEnter(Collider enterTrigger)
    {
        if (enterTrigger.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInRange = true;
            if (playerTarget == null) playerTarget = enterTrigger.gameObject.GetComponent<PlayerController>();
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

    private void OnDrawGizmos()
    {
        if(moveTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(moveTarget.position, 1);
        }
    }
}
