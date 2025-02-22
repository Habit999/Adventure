using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicComponent : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    [Space(5)]

    [SerializeField] private float timeBetweenParticleSpawns;
    [SerializeField] private float particleDespawnHeight;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float floatUpSpeed;

    [HideInInspector] public bool isMimic;

    private List<GameObject> spawnedParticles = new List<GameObject>();
    private List<GameObject> particlesToDestroy = new List<GameObject>();

    private float particleTimer;

    private Enemy_MimicDemon hiddenMimic;

    private void Update()
    {
        if (isMimic)
        {
            ParticleBehaviour();
            DestroyInactiveParticles();
        }
        else if (!isMimic && (spawnedParticles.Count > 0 || particlesToDestroy.Count > 0))
        {
            foreach (var particle in spawnedParticles)
            {
                particle.SetActive(false);
                particlesToDestroy.Add(particle);
            }
            DestroyInactiveParticles();
        }
    }

    public void ActivateComponent(Enemy_MimicDemon mimic)
    {
        isMimic = true;
        particleTimer = timeBetweenParticleSpawns;
        hiddenMimic = mimic;
    }

    public void DeactivateComponent()
    {
        hiddenMimic.SwitchState(Enemy.EnemyState.Fleeing);
        ReleaseMimic();
    }

    public void TriggerMimic()
    {
        hiddenMimic.SwitchState(Enemy.EnemyState.Targeting);
        ReleaseMimic();
    }

    private void ReleaseMimic()
    {
        isMimic = false;
        hiddenMimic.transform.position = transform.position + Vector3.up;
        hiddenMimic.transform.eulerAngles = transform.forward;
        gameObject.SetActive(false);
    }

    private void ParticleBehaviour()
    {
        particleTimer -= Time.deltaTime;

        if (particleTimer <= 0)
        {
            SpawnParticle();
            particleTimer = timeBetweenParticleSpawns;
        }

        foreach (var particle in spawnedParticles)
        {
            particle.transform.position += Vector3.up * floatUpSpeed * Time.deltaTime;
            if (particle.transform.position.y >= particleDespawnHeight)
            {
                particle.SetActive(false);
                particlesToDestroy.Add(particle);
            }
        }
    }

    private void SpawnParticle()
    {
        var newParticle = Instantiate(particlePrefab, transform.position + Random.insideUnitSphere * spawnRadius, Quaternion.identity);
        spawnedParticles.Add(newParticle);
    }

    private void DestroyInactiveParticles()
    {
        foreach (var destroyingParticle in particlesToDestroy)
        {
            spawnedParticles.Remove(destroyingParticle);
            Destroy(destroyingParticle);
        }
        particlesToDestroy.Clear();
    }
}
