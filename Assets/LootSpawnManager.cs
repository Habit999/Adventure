using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles loot spawning and management
/// </summary>

public class LootSpawnManager : MonoBehaviour
{
    public event Action OnSpawnChestLoot;

    private LevelManager levelManager;

    [SerializeField] private CustomGrid targetGrid;

    public List<Item> LootItems = new List<Item>();

    [HideInInspector] public List<GameObject> MimicObjects = new List<GameObject>();

    private List<TreasureChest> treasureChests = new List<TreasureChest>();
    private HashSet<TreasureChest> spawnedChests = new HashSet<TreasureChest>();

    private List<Experience> experiencePoints = new List<Experience>();
    private HashSet<Experience> spawnedXP = new HashSet<Experience>();

    private int chestsToSpawn;
    private int experienceToSpawn;

    private void OnEnable()
    {
        targetGrid.OnGridSpawned += SelectAndSpawnLootInGrid;
    }

    private void OnDisable()
    {
        targetGrid.OnGridSpawned -= SelectAndSpawnLootInGrid;
    }

    private void Awake()
    {
        levelManager = GetComponent<LevelManager>();
    }

	// Interacts with 3D grid system to spawn loot
    private void SelectAndSpawnLootInGrid()
    {
        foreach (var entity in targetGrid.GeneratedData.SpawnedCells)
        {
            if (entity != null && entity.ActiveOccupant != null)
            {
                if (entity.ActiveOccupant.GetComponent<TreasureChest>() != null)
                {
                    TreasureChest chest = entity.ActiveOccupant.GetComponent<TreasureChest>();

                    if (chest != null)
                    {
                        treasureChests.Add(chest);
                        chest.LootMngr = this;
                    }
                }
                else if (entity.ActiveOccupant.GetComponent<Experience>() != null)
                {
                    Experience xp = entity.ActiveOccupant.GetComponent<Experience>();

                    if (xp != null)
                    {
                        experiencePoints.Add(xp);
                    }
                }
            }
        }

        if (treasureChests.Count < levelManager.SpawnData.MaxChestsInLevel) chestsToSpawn = treasureChests.Count;
        else chestsToSpawn = levelManager.SpawnData.MaxChestsInLevel;

        if (experiencePoints.Count < levelManager.SpawnData.MaxExperienceInLevel) experienceToSpawn = experiencePoints.Count;
        else experienceToSpawn = levelManager.SpawnData.MaxExperienceInLevel;

        StartCoroutine(SpawnRandomLoot());
    }

    private IEnumerator SpawnRandomLoot()
    {
        if (chestsToSpawn > 0)
        {
            int randomChestIndex = UnityEngine.Random.Range(0, treasureChests.Count);
            if (!spawnedChests.Contains(treasureChests[randomChestIndex]))
            {
                spawnedChests.Add(treasureChests[randomChestIndex]);
                if (treasureChests[randomChestIndex].GetComponent<MimicComponent>() != null)
                    MimicObjects.Add(treasureChests[randomChestIndex].gameObject);

                treasureChests[randomChestIndex].gameObject.SetActive(true);
                OnSpawnChestLoot += treasureChests[randomChestIndex].SpawnLootItem;
                chestsToSpawn--;
            }
        }

        if (experienceToSpawn > 0)
        {
            int randomXPIndex = UnityEngine.Random.Range(0, experiencePoints.Count);
            if (!spawnedXP.Contains(experiencePoints[randomXPIndex]))
            {
                spawnedXP.Add(experiencePoints[randomXPIndex]);

                experiencePoints[randomXPIndex].gameObject.SetActive(true);
                experienceToSpawn--;
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (chestsToSpawn > 0 || experienceToSpawn > 0) StartCoroutine(SpawnRandomLoot());
        else OnSpawnChestLoot?.Invoke();
    }

    public Item GenerateRandomLoot()
    {
        return LootItems[UnityEngine.Random.Range(0, LootItems.Count)];
    }
}
