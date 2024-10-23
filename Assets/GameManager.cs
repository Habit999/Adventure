using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	
	public static GameSaveData gameSaveData = new GameSaveData();
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
		
		if(!LoadGameData())
		{
			Controls.LoadDefaults();
		}
	}
	
	public bool LoadGameData()
	{
		return false;
	}
}

public class GameSaveData
{
	public bool[] LevelsComplete;
}
