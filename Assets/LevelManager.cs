using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

    public SO_LevelSpawnData SpawnData;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    void Start()
	{
		PlayerController.Instance.IsInDungeon = true;
		GameManager.Instance.LoadGame();
	}
}
