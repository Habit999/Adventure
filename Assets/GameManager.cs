using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	private static string GameSaveDataPath = Application.dataPath + "/SaveData.json";
	
	public GameSaveData GameData;

	public SO_Controls Controls;

	[Space(5)]

	public int CurrentLevel;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		if(LoadGameData() == null)
		{
            GameManager.Instance.Controls.LoadDefaults();
			
			CurrentLevel = 1;
		}
		else
		{
			GameManager.Instance.Controls.LoadDefaults();
		}
	}
	
	void Update()
	{
		// Dev inputs for testing (DELETE FOR BUILD VERSIONS)
		#region Developer Inputs
		if(Input.GetKeyDown(KeyCode.L)) SaveGame();
		#endregion
	}
	
	public void ReturnToMapRoom()
	{
		SceneManager.LoadScene(1);
	}
	
	public bool LoadGame()
	{
		GameSaveData loadedData = LoadGameData();
		
		if(loadedData == null)
		{
			GameData = new GameSaveData();
			print("No Game Data Loaded => Applying Defaults");
			return false;
		}
		else
		{
			CurrentLevel = loadedData.CurrentLevel;
			
			PlayerController player = PlayerController.Instance;
			// Player inventory
			for(int i = 0; i < loadedData.PlayerItems.Length; i++)
			{
				GameObject itemInstance = Instantiate(PrefabLibrary.ItemPrefabID[loadedData.PlayerItems[i]], Vector3.zero, Quaternion.identity);
				player.InventoryMngr.AddItem(itemInstance, loadedData.PlayerItemAmounts[i]);
			}
			
			// Player skill & level stats
			player.SkillsMngr.PlayerLevel = loadedData.PlayerLevel;
			player.SkillsMngr.SkillPoints = loadedData.PlayerSkillPoints;
			player.SkillsMngr.ExperienceGained = loadedData.PlayerExperience;
			player.SkillsMngr.CurrentSkills.vitality = loadedData.PlayerSkillLevels[0];
			player.SkillsMngr.CurrentSkills.strength = loadedData.PlayerSkillLevels[1];
			
			GameData = loadedData;
			print("Game Data Loaded");
			return true;
		}
	}
	
	public void SaveGame()
	{
		GameData = new GameSaveData();
		
		GameData.CurrentLevel = CurrentLevel;
		
		PlayerController player = PlayerController.Instance;
		// Player inventory
		GameData.PlayerItems = new int[player.InventoryMngr.CollectedItems.Count];
		GameData.PlayerItemAmounts = new int[player.InventoryMngr.CollectedItems.Count];
		int arrayIndex = 0;
		foreach(GameObject invItem in player.InventoryMngr.CollectedItems.Keys)
		{
			foreach(int itemID in PrefabLibrary.ItemPrefabID.Keys)
			{
				if(PrefabLibrary.ItemPrefabID[itemID].GetComponent<Item>().ItemData.Name == invItem.GetComponent<Item>().ItemData.Name)
				{
					GameData.PlayerItems[arrayIndex] = itemID;
					GameData.PlayerItemAmounts[arrayIndex] = player.InventoryMngr.CollectedItems[invItem];
					break;
				}
			}
			arrayIndex++;
		}
		
		// Player skill & level stats
		GameData.PlayerLevel = player.SkillsMngr.PlayerLevel;
		GameData.PlayerSkillPoints = player.SkillsMngr.SkillPoints;
		GameData.PlayerExperience = player.SkillsMngr.ExperienceGained;
		GameData.PlayerSkillLevels = new int[3];
		GameData.PlayerSkillLevels[0] = player.SkillsMngr.CurrentSkills.vitality;
		GameData.PlayerSkillLevels[1] = player.SkillsMngr.CurrentSkills.strength;
		
		// Save to json file
		SaveGameData(GameData);
		
		print("Game Data Saved");
	}
	
	GameSaveData LoadGameData()
	{
		if(File.Exists(GameSaveDataPath))
		{
			string jsonData = File.ReadAllText(GameSaveDataPath);
			return JsonUtility.FromJson<GameSaveData>(jsonData);
		}
		else return null;
	}
	
	void SaveGameData(GameSaveData newGameData)
	{
		if(File.Exists(GameSaveDataPath))
		{
			File.Delete(GameSaveDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(newGameData);
		File.WriteAllText(GameSaveDataPath, jsonData);
	}
}

public class GameSaveData
{
	public int CurrentLevel;
	
	public int[] PlayerItems;
	public int[] PlayerItemAmounts;
	
	public int PlayerLevel = 0;
	public int PlayerSkillPoints = 0;
	public float PlayerExperience = 0;
	public int[] PlayerSkillLevels = new int[2] { 1, 1 };
}
