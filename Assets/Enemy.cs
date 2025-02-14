using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Roaming, Targeting, Fleeing, Hiding };
    public EnemyState CurrentState;

	protected Vector3 movePoint;

    [SerializeField] protected float roamingSpeed = 3.5f;
    [SerializeField] protected float roamingRotationSpeed = 120;

    [Space(5)]

	[SerializeField] protected float randomLocationRange;
	[SerializeField] protected int validPositionAttempts;

    [Space(5)]

    [SerializeField] protected float destinationStopDistance;

	protected NavMeshAgent navAgent;

    protected bool hasDestination;

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();

        CheckSpeed();

        hasDestination = false;
    }

    protected virtual void Update()
    {
        EnemyBehaviour();
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
    }

    protected virtual void SwitchState(EnemyState newState)
    {
        CheckSpeed();
        hasDestination = false;
        CurrentState = newState;
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
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * Random.Range(10, randomLocationRange);
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
                randomPosition = transform.position + Random.insideUnitSphere * Random.Range(10, randomLocationRange);
            }
        }
   
        return hasDestination = true;
    }
    private void OnDrawGizmos()
    {
        /*if(movePoint != null)
        {
            Gizmos.DrawWireSphere(movePoint, 5);
        }*/
    }
}
