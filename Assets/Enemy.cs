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

	[SerializeField] protected float randomLocationRange = 170;

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
            if (GenerateRandomNavLocation(out Vector3 result))
            {
                movePoint = result;
                hasDestination = true;
            }
            else return;
        }

        navAgent.SetDestination(movePoint);

        // Reached destination?
        if(transform.position.x == movePoint.x && transform.position.z == movePoint.z)
        {
            hasDestination = false;
        }
    }

    protected virtual void SwitchState(EnemyState newState)
    {
        CheckSpeed();
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

    private bool GenerateRandomNavLocation(out Vector3 returnLocation)
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * randomLocationRange;
        if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hitData, 100, NavMesh.AllAreas))
        {
            returnLocation = hitData.position;
            return true;
        }
        returnLocation = Vector3.zero;
        return false;
    }
    private void OnDrawGizmos()
    {
        /*if(movePoint != null)
        {
            Gizmos.DrawWireSphere(movePoint, 5);
        }*/
    }
}
