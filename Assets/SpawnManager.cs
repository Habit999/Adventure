using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public static SpawnManager Instance;
	
	public enum RANDOMLOOTTYPE { Any, Weapon, Healing };
	
	public SO_LevelSpawnData _levelSpawnData;
	
	public List<GameObject> lootPrefabs = new List<GameObject>();
	
	[Space(10)]
	//Replace with grid system
	public List<GameObject> _chestSpawnPoints = new List<GameObject>();
	
	int chestToSpawn;
	
	public GameObject GenerateRandomLoot(RANDOMLOOTTYPE lootType)
	{
		int countIndex = 0;
		int randomIndex = 0;
		switch(lootType)
		{
			case RANDOMLOOTTYPE.Any:
			print(1);
				randomIndex = Random.Range(0, lootPrefabs.Count - 1);
				foreach(GameObject lootPrf in lootPrefabs)
				{
					print(2);
					if(countIndex == randomIndex)
					{
						print(3);
						GameObject lootInstance = Instantiate(lootPrf, Vector3.zero, Quaternion.identity);
						lootInstance.SetActive(false);
						return lootInstance;
					}
					else countIndex++;
				}
				return null;
				
			case RANDOMLOOTTYPE.Weapon:
				List<GameObject> weaponTypes = new List<GameObject>();
				foreach(GameObject lootPrf in lootPrefabs)
				{
					if(lootPrf.GetComponent<A_Item>()._itemData.Type == SE_ItemData.TYPE.Weapon)
					{
						weaponTypes.Add(lootPrf);
					}
				}
				randomIndex = Random.Range(0, weaponTypes.Count - 1);
				foreach(GameObject weaponPrf in weaponTypes)
				{
					if(countIndex == randomIndex)
					{
						GameObject lootInstance = Instantiate(weaponPrf, Vector3.zero, Quaternion.identity);
						lootInstance.SetActive(false);
						return lootInstance;
					}
					else countIndex++;
				}
				return null;
				
			case RANDOMLOOTTYPE.Healing:
				List<GameObject> healingTypes = new List<GameObject>();
				foreach(GameObject lootPrf in lootPrefabs)
				{
					if(lootPrf.GetComponent<A_Item>()._itemData.Type == SE_ItemData.TYPE.Healing)
					{
						healingTypes.Add(lootPrf);
					}
				}
				randomIndex = Random.Range(0, healingTypes.Count - 1);
				foreach(GameObject healingPrf in healingTypes)
				{
					if(countIndex == randomIndex)
					{
						GameObject lootInstance = Instantiate(healingPrf, Vector3.zero, Quaternion.identity);
						lootInstance.SetActive(false);
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
		
		StartCoroutine(SpawnLevelChests(_levelSpawnData.MaxChestsInLevel));
	}
	
	IEnumerator SpawnLevelChests(int chestsToSpawn)
	{
		int chestCount = chestsToSpawn;
		int randomSpawnPoint = Random.Range(0, _chestSpawnPoints.Count - 1);
		int countIndex = 0;
		
		foreach(GameObject chestSpawn in _chestSpawnPoints)
		{
			if(countIndex == randomSpawnPoint)
			{
				if(!chestSpawn.GetComponent<ChestSpawnPoint>()._willSpawn)
				{
					chestSpawn.GetComponent<ChestSpawnPoint>()._willSpawn = true;
					chestCount -= 1
				}
				else break;
			}
			else countIndex++;
		}
		
		yield return new WaitForSeconds(0.01f);
	}
}
