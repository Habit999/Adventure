using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages spawning and despawning of enemies
/// </summary>

public class EnemySpawnManager : MonoBehaviour
{
    [HideInInspector] public LevelManager LevelMngr;

    public SO_LevelSpawnData SpawnData { get { return LevelMngr.SpawnData; } }

    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private List<Enemy> deadEnemies = new List<Enemy>();

    [HideInInspector] public int VoidsInLevel;
    [HideInInspector] public int ThievesInLevel;
    [HideInInspector] public int MimicsInLevel;

    [SerializeField] private GameObject enemySpawners;

    private void Awake()
    {
        LevelMngr = GetComponent<LevelManager>();

        foreach(var spawner in enemySpawners.GetComponentsInChildren<EnemySpawner>())
        {
            spawner.SpawnMngr = this;
        }
    }

    private void Update()
    {
        // Destroy and remove dead enemies from the list before next frame
        foreach (var enemy in deadEnemies)
        {
            spawnedEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
        deadEnemies.Clear();
    }

    public void SpawnNewEnemy(Enemy enemy, Vector3 position)
    {
        // Checks if max number of enemby has been hit
        if (enemy is Enemy_VoidDemon && VoidsInLevel >= SpawnData.MaxVoidsInLevel)
            return;
        else if (enemy is Enemy_ThiefDemon && ThievesInLevel >= SpawnData.MaxThievesInLevel)
            return;
        else if (enemy is Enemy_MimicDemon && MimicsInLevel >= SpawnData.MaxMimicsInLevel)
            return;

        // Spawns enemy
        Enemy enemyInstance = Instantiate(enemy, position, Quaternion.identity);
        enemyInstance.SpawnManager = this;
        if (enemyInstance is Enemy_MimicDemon)
            enemyInstance.GetComponent<Enemy_MimicDemon>().LootManager = GetComponent<LootSpawnManager>();

        NewEnemy(enemyInstance);
    }

    // Adds new enemies to the list of spawned enemies
    public void NewEnemy(Enemy newEnemy)
    {
        spawnedEnemies.Add(newEnemy);

        if (newEnemy is Enemy_VoidDemon)
        {
            VoidsInLevel++;
            newEnemy.GetComponent<Enemy_VoidDemon>().LevelMngr = LevelMngr;
        }
        else if (newEnemy is Enemy_ThiefDemon)
        {
            ThievesInLevel++;
        }
        else if (newEnemy is Enemy_MimicDemon)
        {
            MimicsInLevel++;
        }
    }

    public void KillEnemy(Enemy deadEnemy)
    {
        deadEnemies.Add(deadEnemy);
    }
}
