using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event Action<Enemy> OnSpawnEnemy;

    [SerializeField] private EnemySpawnManager spawnManager;

    [SerializeField] private List<Enemy> enemiesToSpawn;

    [SerializeField] private float timeBetweenSpawns;

    private float spawnTimer;

    private void Awake()
    {
        OnSpawnEnemy += spawnManager.NewEnemy;
        spawnTimer = timeBetweenSpawns;
    }

    private void Start()
    {
        SpawnEnemy();
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

        if (enemiesToSpawn[randomIndex] is Enemy_VoidDemon && spawnManager.VoidsInLevel >= spawnManager.SpawnData.MaxVoidsInLevel)
            return;
        else if (enemiesToSpawn[randomIndex] is Enemy_ThiefDemon && spawnManager.ThievesInLevel >= spawnManager.SpawnData.MaxThievesInLevel)
            return;
        else if (enemiesToSpawn[randomIndex] is Enemy_MimicDemon && spawnManager.MimicsInLevel >= spawnManager.SpawnData.MaxMimicsInLevel)
            return;

        Enemy newEnemy = Instantiate(enemiesToSpawn[randomIndex], transform.position, Quaternion.identity);
        if(newEnemy is Enemy_MimicDemon) 
            newEnemy.GetComponent<Enemy_MimicDemon>().LootManager = spawnManager.GetComponent<LootSpawnManager>();

        newEnemy.SpawnManager = spawnManager;
        OnSpawnEnemy?.Invoke(newEnemy);
    }
}
