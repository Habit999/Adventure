using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Mimic Demon Enemy AI
/// </summary>

public class Enemy_MimicDemon : Enemy
{
    // References
    [HideInInspector] public LootSpawnManager LootManager;

    [HideInInspector] public PlayerController PlayerTarget;
    private TreasureChest chestTarget;

    private SphereCollider sphereCollider;

    // Variables
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

    protected override void Update()
    {
        base.Update();

        if (audioSource != null && Camera.main != null)
            if (!audioSource.isPlaying && Vector3.Distance(transform.position, Camera.main.transform.position) < audioCutOffDistance)
                audioSource.Play();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // Makes them flee when hit
        if(!isDead) SwitchState(EnemyState.Fleeing);
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
        // Finds closest mimic object
        if (!hasDestination)
        {
            if (FindClosestMimicObject())
            {
                hasDestination = true;
            }
        }
        // Default roam if no mimic object is found
        if(chestTarget == null)
        {
            base.Roaming();
            return;
        }

        // Change target if mimic object is no longer available
        if (chestTarget.IsOpen || chestTarget.MimicHideout.IsMimic || chestTarget.MimicHideout.TargettingMimic != this)
        {
            FindClosestMimicObject();
            return;
        }

        navAgent.SetDestination(movePoint);

        // Hide in mimic object when at destination
        if (Vector3.Distance(transform.position, chestTarget.transform.position) <= destinationStopDistance)
        {
            chestTarget.GetComponent<MimicComponent>().ActivateComponent(this);
            chestTarget = null;
            hasDestination = false;
            SwitchState(EnemyState.Hiding);
        }

        CheckIfStuck();
    }

    private void Targeting()
    {
        navAgent.SetDestination(PlayerTarget.transform.position);

        // Damage player if in range
        if (Vector3.Distance(transform.position, PlayerTarget.transform.position) <= damageDistance)
        {
            PlayerTarget.DamagePlayer(damage);
            SwitchState(EnemyState.Fleeing);
        }
    }

    private void Fleeing()
    {
        if (!hasDestination)
        {
            // Sets randome flee point and timer
            if (GenerateRandomNavLocation())
            {
                fleeExitTimer = timeToExitFlee;
                return;
            }
            else return;
        }
        // Resets flee state if it can see player
        if (isInView)
        {
            fleeExitTimer = timeToExitFlee;
            GenerateRandomNavLocation();
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

    // Finds the closest available mimic object
    private bool FindClosestMimicObject()
    {
        if (LootManager.MimicObjects.Count > 0)
        {
            // Find closest mimic object
            GameObject closestMimicObject = null;
            float closestDistance = 0;
            chestTarget = null;
            foreach (var mimicObj in LootManager.MimicObjects)
            {
                TreasureChest chest = mimicObj.GetComponent<TreasureChest>();
                if (!chest.IsOpen && !chest.MimicHideout.IsMimic && chest.MimicHideout.TargettingMimic == null)
                {
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
            }

            // Set move target
            if (closestMimicObject == null) return false;
            else
            {
                chestTarget = closestMimicObject.GetComponent<TreasureChest>();
                chestTarget.MimicHideout.TargettingMimic = this;
                if (NavMesh.SamplePosition(closestMimicObject.transform.position, out NavMeshHit hitData, 10, NavMesh.AllAreas))
                {
                    movePoint = hitData.position;
                    return hasDestination = true;
                }
                else return hasDestination = false;
            }
        }
        else return false;
    }

    private void CheckPlayerInView()
    {
        Vector3 playerDirection = (PlayerTarget.transform.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, playerDirection) >= fieldOfViewRange)
        {
            if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, sphereCollider.radius))
            {
                if (hit.collider.transform == PlayerTarget.transform)
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
            if (PlayerTarget == null) PlayerTarget = enterTrigger.gameObject.GetComponent<PlayerController>();
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
