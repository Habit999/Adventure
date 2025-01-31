using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	void Start()
	{
		PlayerController.Instance.IsInDungeon = true;
		GameManager.Instance.LoadGame();
	}
}
