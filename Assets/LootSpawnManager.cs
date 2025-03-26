using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlayerController.Instance.InventoryMngr.AddItem(Instantiate(LootItems[2].gameObject, Vector3.zero, Quaternion.identity), 1);
        }
    }

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


    /*
	public static SpawnManager Instance;

	public enum RANDOMLOOTTYPE { Any, Weapon, Consumable };
	
	public SO_LevelSpawnData _levelSpawnData;
	
	public delegate void SpawnLootDelegate();
	public static event SpawnLootDelegate SpawnLevelLoot; 
	
	public List<GameObject> lootPrefabs = new List<GameObject>();
	
	[Space(10)]
	//Replace with grid system
	[SerializeField] Transform spawnPoints;
	List<GameObject> chestSpawnPoints = new List<GameObject>();
	List<GameObject> experienceSpawnPoints = new List<GameObject>();
	
	int chestToSpawn;
	
	public GameObject GenerateRandomLoot(RANDOMLOOTTYPE lootType)
	{
		int countIndex = 0;
		int randomIndex = 0;
		switch(lootType)
		{
			case RANDOMLOOTTYPE.Any:
				randomIndex = Random.Range(0, lootPrefabs.Count);
				foreach(GameObject lootPrf in lootPrefabs)
				{
					if(countIndex == randomIndex)
					{
						GameObject lootInstance = Instantiate(lootPrf, Vector3.zero, Quaternion.identity);
						return lootInstance;
					}
					else countIndex++;
				}
				return null;
				
			case RANDOMLOOTTYPE.Weapon:
				List<GameObject> weaponTypes = new List<GameObject>();
				foreach(GameObject lootPrf in lootPrefabs)
				{
					if(lootPrf.GetComponent<Item>().ItemData.Type == Item.ItemDataStructure.TYPE.Weapon)
					{
						weaponTypes.Add(lootPrf);
					}
				}
				randomIndex = Random.Range(0, weaponTypes.Count);
				foreach(GameObject weaponPrf in weaponTypes)
				{
					if(countIndex == randomIndex)
					{
						GameObject lootInstance = Instantiate(weaponPrf, Vector3.zero, Quaternion.identity);
						return lootInstance;
					}
					else countIndex++;
				}
				return null;
				
			case RANDOMLOOTTYPE.Consumable:
				List<GameObject> consumableTypes = new List<GameObject>();
				foreach(GameObject lootPrf in lootPrefabs)
				{
					if(lootPrf.GetComponent<Item>().ItemData.Type == Item.ItemDataStructure.TYPE.Consumable)
					{
						consumableTypes.Add(lootPrf);
					}
				}
				randomIndex = Random.Range(0, consumableTypes.Count);
				foreach(GameObject consumablePrf in consumableTypes)
				{
					if(countIndex == randomIndex)
					{
						GameObject lootInstance = Instantiate(consumablePrf, Vector3.zero, Quaternion.identity);
						return lootInstance;
					}
					else countIndex++;
				}
				return null;
				
			default:
				return null;
		}
	}
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		foreach(Transform spawn in spawnPoints)
		{
			if(spawn.gameObject.GetComponent<ChestSpawnPoint>() != null)
			{
				chestSpawnPoints.Add(spawn.gameObject);
			}
			else if(spawn.gameObject.GetComponent<ExperienceSpawnPoint>() != null)
			{
				experienceSpawnPoints.Add(spawn.gameObject);
			}
		}
		
		StartCoroutine(SpawnLootRoutine(_levelSpawnData.MaxChestsInLevel, _levelSpawnData.MaxExperienceInLevel));
	}
	
	IEnumerator SpawnLootRoutine(int chestsToSpawn, int experienceToSpawn)
	{
		int chestCount = chestsToSpawn;
		int experienceCount = experienceToSpawn;
		int randomSpawnPoint = Random.Range(0, chestSpawnPoints.Count);
		int countIndex = 0;
		
		// Chests
		foreach(GameObject chestSpawn in chestSpawnPoints)
		{
			if(countIndex == randomSpawnPoint)
			{
				if(!chestSpawn.GetComponent<ChestSpawnPoint>().WillSpawn)
				{
					chestSpawn.GetComponent<ChestSpawnPoint>().WillSpawn = true;
					chestCount -= 1;
					break;
				}
				else break;
			}
			else countIndex++;
		}
		
		// Experience
		randomSpawnPoint = Random.Range(0, experienceSpawnPoints.Count);
		countIndex = 0;
		foreach(GameObject xpSpawn in experienceSpawnPoints)
		{
			if(countIndex == randomSpawnPoint)
			{
				if(!xpSpawn.GetComponent<ExperienceSpawnPoint>()._willSpawn)
				{
					xpSpawn.GetComponent<ExperienceSpawnPoint>()._willSpawn = true;
					chestCount -= 1;
					break;
				}
				else break;
			}
			else countIndex++;
		}
		
		yield return new WaitForSeconds(0.01f);
		if(chestCount > 0 && experienceCount > 0) StartCoroutine(SpawnLootRoutine(chestCount, experienceCount));
		else SpawnLevelLoot();
	}*/
}
