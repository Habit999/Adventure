using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector] public EnemySpawnManager SpawnMngr;

    [SerializeField] private List<Enemy> enemiesToSpawn;

    [SerializeField] private float timeBetweenSpawns;

    private float spawnTimer;

    private void Awake()
    {
        spawnTimer = 0;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if(spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = timeBetweenSpawns;
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemiesToSpawn.Count);

        SpawnMngr.SpawnNewEnemy(enemiesToSpawn[randomIndex], transform.position);
    }
}
