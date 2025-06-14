using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for all enemies, contains basic enemy functionality
/// </summary>

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Roaming, Targeting, Fleeing, Hiding };
    public EnemyState CurrentState;

    protected event Action<Enemy> OnDead;

    [SerializeField] protected float maxHealth = 100;
    private float health;

    [Space(5)]

    private Dictionary<Renderer, Material> enemyMaterials = new Dictionary<Renderer, Material>();
    [SerializeField] protected Material damagedMaterial;
    [SerializeField] protected bool canBeDamaged;
    private IEnumerator damageRoutine;
    private bool isDamaged;

    [Space(5)]

    protected AudioSource audioSource;
    protected float audioCutOffDistance;

    [Space(5)]

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

    protected bool isDead;

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        audioCutOffDistance = audioSource.maxDistance;

        CheckSpeed();

        health = maxHealth;

        hasDestination = false;

        isDead = false;

        stuckTimer = isStuckTime;
        lastLocation = transform.position;

        foreach(var bodyPart in GetComponentsInChildren<Renderer>())
        {
            enemyMaterials.Add(bodyPart, bodyPart.material);
        }
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

        CheckAudioRange();
    }

    private void CheckAudioRange()
    {
        if (audioSource != null && Camera.main != null)
            if (Vector3.Distance(transform.position, Camera.main.transform.position) > audioCutOffDistance && audioSource.isPlaying)
                audioSource.Stop();
    }

#if UNITY_EDITOR
    [ContextMenu("Kill Enemy")]
#endif
    public void KillEnemy()
    {
        isDead = true;
        OnDead?.Invoke(this);
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) KillEnemy();
        else
        {
            if (canBeDamaged)
            {
                if (isDamaged)
                {
                    StopCoroutine(damageRoutine);
                    StartCoroutine(damageRoutine = DamageMaterial());
                }
                else StartCoroutine(damageRoutine = DamageMaterial());
            }
        }

        Debug.Log("Enemy damaged, health reamining: " + health);
    }

    private IEnumerator DamageMaterial()
    {
        isDamaged = true;
        foreach (var bodyPart in enemyMaterials.Keys)
        {
            bodyPart.material = damagedMaterial;
        }
        yield return new WaitForSeconds(0.3f);
        foreach (var bodyPart in enemyMaterials.Keys)
        {
            bodyPart.material = enemyMaterials[bodyPart];
        }
        isDamaged = false;
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

    // Generates a random location on the NavMesh
#if UNITY_EDITOR
    [ContextMenu("GeneratePoint")]
#endif
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

    // Checks if the enemy is stuck in one spot for too long
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
