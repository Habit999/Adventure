using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
	[SerializeField] private GameObject mapCamera;

	private PlayerController playerController;
	
	void Start()
	{
		mapCamera.SetActive(false);
	}
	
	void Update()
	{
		if(playerController != null && Input.GetKeyDown(playerController.InputControls.Escape)) CloseMap();
	}
	
	public void OpenMap(PlayerController player)
	{
		playerController = player;
		player.Camera.gameObject.SetActive(false);
		mapCamera.SetActive(true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	
	void CloseMap()
	{
		mapCamera.SetActive(false);
		playerController.Camera.gameObject.SetActive(true);
		playerController.UnFreezePlayer();
	}
}
