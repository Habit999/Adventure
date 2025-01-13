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
	
	public GameData _gameData;
	
	public int _currentLevel;
	
	void Awake()
	{
		/*if(Instance == null)*/ Instance = this;
		/*else Destroy(this.gameObject);
		
		DontDestroyOnLoad(this.gameObject);*/
		
		if(LoadGameData() == null)
		{
			Controls.LoadDefaults();
			
			_currentLevel = 1;
		}
		else
		{
			Controls.LoadDefaults();
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
		GameData loadedData = LoadGameData();
		
		if(loadedData == null)
		{
			_gameData = new GameData();
			print("No Game Data Loaded => Applying Defaults");
			return false;
		}
		else
		{
			_currentLevel = loadedData.CurrentLevel;
			
			PlayerController player = PlayerController.Instance;
			// Player inventory
			for(int i = 0; i < loadedData.PlayerItems.Length; i++)
			{
				GameObject itemInstance = Instantiate(PrefabLibrary.ItemPrefabID[loadedData.PlayerItems[i]], Vector3.zero, Quaternion.identity);
				player.InventoryMngr.AddItem(itemInstance, loadedData.PlayerItemAmounts[i]);
			}
			
			// Player skill & level stats
			player.SkillsMngr._playerLevel = loadedData.PlayerLevel;
			player.SkillsMngr._skillPoints = loadedData.PlayerSkillPoints;
			player.SkillsMngr._experienceGained = loadedData.PlayerExperience;
			player.SkillsMngr._currentSkills.vitality = loadedData.PlayerSkillLevels[0];
			player.SkillsMngr._currentSkills.strength = loadedData.PlayerSkillLevels[1];
			player.SkillsMngr._currentSkills.intelligence = loadedData.PlayerSkillLevels[2];
			
			_gameData = loadedData;
			print("Game Data Loaded");
			return true;
		}
	}
	
	public void SaveGame()
	{
		_gameData = new GameData();
		
		_gameData.CurrentLevel = _currentLevel;
		
		PlayerController player = PlayerController.Instance;
		// Player inventory
		_gameData.PlayerItems = new int[player.InventoryMngr._collectedItems.Count];
		_gameData.PlayerItemAmounts = new int[player.InventoryMngr._collectedItems.Count];
		int arrayIndex = 0;
		foreach(GameObject invItem in player.InventoryMngr._collectedItems.Keys)
		{
			foreach(int itemID in PrefabLibrary.ItemPrefabID.Keys)
			{
				if(PrefabLibrary.ItemPrefabID[itemID].GetComponent<Item>()._itemData.Name == invItem.GetComponent<Item>()._itemData.Name)
				{
					_gameData.PlayerItems[arrayIndex] = itemID;
					_gameData.PlayerItemAmounts[arrayIndex] = player.InventoryMngr._collectedItems[invItem];
					break;
				}
			}
			arrayIndex++;
		}
		
		// Player skill & level stats
		_gameData.PlayerLevel = player.SkillsMngr._playerLevel;
		_gameData.PlayerSkillPoints = player.SkillsMngr._skillPoints;
		_gameData.PlayerExperience = player.SkillsMngr._experienceGained;
		_gameData.PlayerSkillLevels = new int[3];
		_gameData.PlayerSkillLevels[0] = player.SkillsMngr._currentSkills.vitality;
		_gameData.PlayerSkillLevels[1] = player.SkillsMngr._currentSkills.strength;
		_gameData.PlayerSkillLevels[2] = player.SkillsMngr._currentSkills.intelligence;
		
		// Save to json file
		SaveGameData(_gameData);
		
		print("Game Data Saved");
	}
	
	GameData LoadGameData()
	{
		if(File.Exists(GameSaveDataPath))
		{
			string jsonData = File.ReadAllText(GameSaveDataPath);
			return JsonUtility.FromJson<GameData>(jsonData);
		}
		else return null;
	}
	
	void SaveGameData(GameData newGameData)
	{
		if(File.Exists(GameSaveDataPath))
		{
			File.Delete(GameSaveDataPath);
		}
		
		string jsonData = JsonUtility.ToJson(newGameData);
		File.WriteAllText(GameSaveDataPath, jsonData);
	}
}

public class GameData
{
	public int CurrentLevel;
	
	public int[] PlayerItems;
	public int[] PlayerItemAmounts;
	
	public int PlayerLevel = 0;
	public int PlayerSkillPoints = 0;
	public float PlayerExperience = 0;
	public int[] PlayerSkillLevels = new int[3] { 1, 1, 1 };
}
