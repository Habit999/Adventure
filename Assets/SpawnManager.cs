using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
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
	}
}
