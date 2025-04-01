using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	private static string GameSaveDataPath = Application.dataPath + "/SaveData.json";
	
	public GameSaveData GameData;

	public enum GameStates 
	{ 
		MainMenu,
		InGame,
		Paused
	};
	public GameStates CurrentGameState;

	public SO_Controls Controls;

	[SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI fullscreenButton;

    [HideInInspector] public bool IsFullscreen = true;
	
	private void Awake()
	{
		if(Instance == null || Instance == this) Instance = this;
		else
		{
            Destroy(this.gameObject);
			return;
        }

        if (File.Exists(GameSaveDataPath))
            LoadGame();
		
        Controls.LoadDefaults();

        DontDestroyOnLoad(this.gameObject);
	}

    private void Update()
    {
        if(Input.GetKeyDown(Controls.Escape))
        {
            if (CurrentGameState == GameStates.InGame)
            {
                PauseGame();
            }
        }
    }

    public void ClearGameData()
	{
		if(File.Exists(GameSaveDataPath)) File.Delete(GameSaveDataPath);
		GameData = null;
	}

	public void ResumeGame()
	{
		CurrentGameState = GameStates.InGame;
        PlayerController.Instance.UnFreezePlayer();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
		pauseMenu.SetActive(false);
    }

	public void PauseGame()
	{
		CurrentGameState = GameStates.Paused;
		PlayerController.Instance.FreezePlayer(true, true);
        Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
        Time.timeScale = 0;
		pauseMenu.SetActive(true);
    }

	public void ReturnToMenu()
	{
		CurrentGameState = GameStates.MainMenu;
        Time.timeScale = 1;
		pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void ChangeWindowMode()
    {
		IsFullscreen = !IsFullscreen;
        fullscreenButton.SetText(IsFullscreen ? "X" : "");
        Screen.SetResolution(Screen.width, Screen.height, IsFullscreen);
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
			GameData = loadedData;
			print("Game Data Loaded");
			return true;
		}
	}

	public void GivePlayerData()
	{
		if (GameData == null) LoadGame();

        PlayerController player = PlayerController.Instance;
        // Player inventory
        for (int i = 0; i < GameData.PlayerItems.Length; i++)
        {
            GameObject itemInstance = Instantiate(PrefabLibrary.ItemPrefabID[GameData.PlayerItems[i]], Vector3.zero, Quaternion.identity);
            player.InventoryMngr.AddItem(itemInstance, GameData.PlayerItemAmounts[i]);
        }

        // Player skill & level stats
        player.SkillsMngr.PlayerLevel = GameData.PlayerLevel;
        player.SkillsMngr.SkillPoints = GameData.PlayerSkillPoints;
        player.SkillsMngr.ExperienceGained = GameData.PlayerExperience;
        player.SkillsMngr.CurrentSkills.vitality = GameData.PlayerSkillLevels[0];
        player.SkillsMngr.CurrentSkills.strength = GameData.PlayerSkillLevels[1];
    }
	
	public void SaveGame()
	{
		GameData = new GameSaveData();
		
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
	public int[] PlayerItems = new int[0];
	public int[] PlayerItemAmounts = new int[0];
	
	public int PlayerLevel = 0;
	public int PlayerSkillPoints = 0;
	public float PlayerExperience = 0;
	public int[] PlayerSkillLevels = new int[2] { 1, 1 };
}
