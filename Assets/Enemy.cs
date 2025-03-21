using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Roaming, Targeting, Fleeing, Hiding };
    public EnemyState CurrentState;

    protected event Action<Enemy> OnDead;

    [SerializeField] private float maxHealth = 100;
    private float health;

    [HideInInspector] public EnemySpawnManager SpawnManager;

	protected Vector3 movePoint;

    [SerializeField] protected float roamingSpeed = 3.5f;
    [SerializeField] protected float roamingRotationSpeed = 120;

    [Space(5)]

	[SerializeField] protected float randomLocationRange;
	[SerializeField] protected int validPositionAttempts;

    [Space(5)]

    [SerializeField] protected float destinationStopDistance;
    [SerializeField] protected float isStuckTime;
    protected float stuckTimer;

	protected NavMeshAgent navAgent;

    protected Vector3 lastLocation;

    protected bool hasDestination;

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();

        CheckSpeed();

        health = maxHealth;

        hasDestination = false;

        stuckTimer = isStuckTime;
        lastLocation = transform.position;
    }

    protected virtual void Start()
    {
        OnDead += SpawnManager.KillEnemy;
    }

    protected virtual void OnDisable()
    {
        OnDead -= SpawnManager.KillEnemy;
    }

    protected virtual void Update()
    {
        EnemyBehaviour();
    }

    [ContextMenu("Kill Enemy")]
    public void KillEnemy()
    {
        OnDead(this);
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy hit for " + damage);
        Debug.Log("Remaining health " + health);
        if (health <= 0) KillEnemy();
    }

    protected virtual void EnemyBehaviour()
    {
        switch(CurrentState)
        {
            case EnemyState.Roaming:
                Roaming();
                break;

            default:
                break;
        }
    }

    protected virtual void Roaming()
    {
        // Find random location to roam to
        if(!hasDestination)
        {
            if (!GenerateRandomNavLocation()) return;
        }

        navAgent.SetDestination(movePoint);

        // Reached destination?
        if(!navAgent.pathPending && navAgent.remainingDistance <= destinationStopDistance)
        {
            hasDestination = false;
        }

        CheckIfStuck();
    }

    public virtual void SwitchState(EnemyState newState)
    {
        hasDestination = false;
        CurrentState = newState;
        CheckSpeed();
    }

    protected virtual void CheckSpeed()
    {
        switch (CurrentState)
        {
            case EnemyState.Roaming:
                navAgent.speed = roamingSpeed;
                navAgent.angularSpeed = roamingRotationSpeed;
                break;

            default:
                break;
        }
    }

    [ContextMenu("GeneratePoint")]
    protected bool GenerateRandomNavLocation()
    {
        Vector3 randomPosition = transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(10, randomLocationRange);
        NavMeshHit hitData;

        for (int i = 0; i < validPositionAttempts; i++)
        {
            if (NavMesh.SamplePosition(randomPosition, out hitData, 10, NavMesh.AllAreas))
            {
                movePoint = hitData.position;
                return hasDestination = true;
            }
            else
            {
                randomPosition = transform.position + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(10, randomLocationRange);
            }
        }
   
        return hasDestination = true;
    }

    protected void CheckIfStuck()
    {
        if (lastLocation == transform.position)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0) GenerateRandomNavLocation();
        }
        else
        {
            stuckTimer = isStuckTime;
            lastLocation = transform.position;
        }
    }
}
