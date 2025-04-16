using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossDemon : Enemy
{
    [HideInInspector] public BossRoomManager RoomManager;

    [Space(5)]
    [SerializeField] private float targetingSpeed;
    [SerializeField] private float targetingRotationSpeed;
    [Space(5)]
    [Range(-1, 1)]
    [SerializeField] private float periferalVision;
    [SerializeField] private float maxIdleLength;
    [Space(5)]
    [SerializeField] private AnimationClip attackAnimation;
    private Animator animator;
    [Space(5)]
    [SerializeField] private float distanceToDamage;
    [SerializeField] private float damage;
    [SerializeField] private List<AudioClip> gruntSounds;

    private float idleTimer;
    private float attackingTimer;

    private PlayerController playerTarget;
    private bool playerInRange;

    private AudioSource audioSource;

    private bool isAttacking;
    private bool isMoving;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        attackingTimer = attackAnimation.length;

        isMoving = true;

        StartCoroutine(IdleSoundRoutine());
    }

    protected override void Start()
    {
        OnDead += RoomManager.OpenExitGate;
    }

    protected override void OnDisable()
    {
        OnDead -= RoomManager.OpenExitGate;
    }

    private IEnumerator IdleSoundRoutine()
    {
        yield return new WaitForSeconds(5);
        audioSource.clip = gruntSounds[Random.Range(0, gruntSounds.Count)];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        StartCoroutine(IdleSoundRoutine());
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
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        audioSource.clip = gruntSounds[Random.Range(0, gruntSounds.Count)];
        audioSource.Play();

        SwitchState(EnemyState.Targeting);
    }

    protected override void Roaming()
    {
        if(isMoving)
        {
            // Find random location to roam to
            if (!hasDestination)
            {
                if (!GenerateRandomNavLocation()) return;
            }

            navAgent.SetDestination(movePoint);

            // Reached destination?
            if (!navAgent.pathPending && navAgent.remainingDistance <= destinationStopDistance)
            {
                hasDestination = false;
                isMoving = false;
                navAgent.SetDestination(navAgent.transform.position);
                idleTimer = Random.Range(1, maxIdleLength);
            }
        }
        else
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0) isMoving = true;
        }

        // Check if player is in range
        if (playerInRange)
        {
            Vector3 direction = playerTarget.transform.position - transform.position;
            if (Vector3.Dot(transform.forward, direction) > periferalVision)
            {
                isMoving = true;
                SwitchState(EnemyState.Targeting);
            }
        }

        UpdateAnimator();
    }

    private void Targeting()
    {
        if (!isAttacking)
        {
            navAgent.SetDestination(playerTarget.transform.position);

            if (Vector3.Distance(transform.position, playerTarget.transform.position) < distanceToDamage)
            {
                isAttacking = true;
                attackingTimer = attackAnimation.length;
                navAgent.SetDestination(navAgent.transform.position);
            }
            UpdateAnimator();
        }
        if (isAttacking)
        {
            if(attackingTimer <= 0)
            {
                if (Vector3.Distance(transform.position, playerTarget.transform.position) < distanceToDamage)
                {
                    playerTarget.DamagePlayer(damage);
                }
                isAttacking = false;
                UpdateAnimator();
            }
            else attackingTimer -= Time.deltaTime;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Attacking", isAttacking);
    }

    protected override void CheckSpeed()
    {
        switch(CurrentState)
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

    private void OnTriggerEnter(Collider enterCol)
    {
        if(enterCol.CompareTag("Player"))
        {
            playerInRange = true;
            if(playerTarget == null)
            {
                playerTarget = enterCol.gameObject.GetComponent<PlayerController>();
            }
        }
    }

    private void OnTriggerExit(Collider exitCol)
    {
        if (exitCol.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToDamage);
    }
#endif
}
