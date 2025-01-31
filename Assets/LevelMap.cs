using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
	[HideInInspector] public List<bool> LevelCompleted = new List<bool>();
	
	[SerializeField] GameObject mapCamera;
	[SerializeField] Transform mapUI;
	
	List<MapLevelIcon> levelIcons = new List<MapLevelIcon>();
	
	void Start()
	{
		mapCamera.SetActive(false);
		
		foreach(Transform element in mapUI)
		{
			if(element.gameObject.GetComponent<MapLevelIcon>() != null)
				levelIcons.Add(element.gameObject.GetComponent<MapLevelIcon>());
		}
		for(int i = 0; i < levelIcons.Count; i++)
		{
			if(levelIcons[i]._levelDetails.LevelNumber < GameManager.Instance.CurrentLevel)
				levelIcons[i]._progressionStatus = "Completed";
			else if(levelIcons[i]._levelDetails.LevelNumber == GameManager.Instance.CurrentLevel)
				levelIcons[i]._progressionStatus = "Incompleted";
			else levelIcons[i]._progressionStatus = "???";
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown(GameManager.Instance.Controls.Back)) CloseMap();
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
		PlayerController.Instance.UnFreezePlayer();
	}
}
