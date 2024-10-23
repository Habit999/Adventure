using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
	[SerializeField] GameObject mapCamera;
	
	[HideInInspector] public bool[] levelCompleted;
	
	void Start()
	{
		mapCamera.SetActive(false);
	}
	
	public void OpenMap()
	{
		mapCamera.SetActive(true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	
	void CloseMap()
	{
		mapCamera.SetActive(false);
		PlayerController.Instance.UnFreezePlayer(PlayerController.PLAYERSTATE.FreeMove);
	}
}
