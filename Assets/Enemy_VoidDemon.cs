using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VoidDemon : Enemy
{
    [Space(5)]
    [SerializeField] private float gravityForce;
    [SerializeField] private float minDistanceToDimLight;
    [SerializeField] private float maxDistanceToDimLight;

    private SphereCollider sphereCollider;

    private PlayerController playerController;

    [HideInInspector] public LevelManager LevelMngr;

    protected override void Start()
    {
        base.Start();

        sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void Roaming()
    {
        base.Roaming();

        CalculateLightDimming();
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerController = trigger.gameObject.GetComponent<PlayerController>();
            if (CheckObstacles(trigger.gameObject))
            {
                // Checks if players within the threshold
                if (Vector3.Distance(trigger.transform.position, transform.position) <= sphereCollider.radius)
                {
                    ApplyGravity(trigger.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerController.VanishPlayer();
        }
    }

    private bool CheckObstacles(GameObject target)
    {
        Vector3 playerDirection = target.transform.position - transform.position;
        if (Physics.Raycast(transform.position, playerDirection, out RaycastHit hit, sphereCollider.radius))
        {
            if (hit.collider.gameObject == target)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

    private void ApplyGravity(GameObject player)
    {
        float force = CalculateForce(player.transform);
        Vector3 directionalForce = (transform.position - player.transform.position).normalized * force;
        playerController.ApplyExternalForce(directionalForce);
    }

    private float CalculateForce(Transform target)
    {
        float playerDistance = Vector3.Distance(target.position, transform.position);
        float distanceInRange = playerDistance / sphereCollider.radius;
        float flippedPercentage = 1 - distanceInRange;
        return gravityForce * flippedPercentage;
    }

    private void CalculateLightDimming()
    {
        foreach(var light in LevelMngr.Torches)
        {
            float distance = Vector3.Distance(transform.position, light.transform.position);
            if(distance < maxDistanceToDimLight)
            {
                float percentage = (distance - minDistanceToDimLight) / (maxDistanceToDimLight - minDistanceToDimLight);
                light.AdjustLightIntensity(percentage);
            }
        }
    }
}
