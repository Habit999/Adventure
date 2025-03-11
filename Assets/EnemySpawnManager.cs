using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [HideInInspector] public LevelManager LevelMngr;

    public SO_LevelSpawnData SpawnData { get { return LevelMngr.SpawnData; } }

    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private List<Enemy> deadEnemies = new List<Enemy>();

    [HideInInspector] public int VoidsInLevel;
    [HideInInspector] public int ThievesInLevel;
    [HideInInspector] public int MimicsInLevel;

    private void Awake()
    {
        LevelMngr = GetComponent<LevelManager>();
    }

    private void Update()
    {
        foreach(var enemy in deadEnemies)
        {
            spawnedEnemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
        deadEnemies.Clear();
    }

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
