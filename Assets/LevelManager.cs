using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager Instance;

    public SO_LevelSpawnData SpawnData;

    [SerializeField] private Transform levelTorches;

    [HideInInspector] public List<Torch> Torches = new List<Torch>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        foreach(var torch in levelTorches.gameObject.GetComponentsInChildren<Torch>())
        {
            Torches.Add(torch);
        }
    }

    void Start()
	{
		PlayerController.Instance.IsInDungeon = true;
        GameManager.Instance.GivePlayerData();
    }
}
