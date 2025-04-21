using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls regula dungeon exits
/// </summary>

public class DungeonExit : MonoBehaviour
{
	public bool _canExit;
	
	[Space(5)]
	
	[SerializeField] private Transform exitCam;
	[SerializeField] private Transform camTarget;
	[SerializeField] private GameObject exitMsg;
	[SerializeField] private float camTransitionSpeed;

    private Vector3 startPoint;

    private Vector3 transitionDirection;

    private bool isLeaving;
	
	void Awake()
	{
		exitCam.gameObject.SetActive(false);
		
		startPoint = exitCam.position;
		
		transitionDirection = (camTarget.position - startPoint).normalized * camTransitionSpeed;
		
		isLeaving = false;
	}

    private void Update()
	{
		if(isLeaving) TransitionCam();
		
		ToggleExitMessage();
	}
	
	public void Exit()
	{
		if(_canExit)
		{
			GameManager.Instance.CurrentGameState = GameManager.GameStates.Paused;
            PlayerController.Instance.gameObject.SetActive(false);
			exitCam.gameObject.SetActive(true);
			isLeaving = true;
		}
	}
	
	void TransitionCam()
	{
		exitCam.position += transitionDirection;
		
		if(Vector3.Distance(exitCam.position, camTarget.position) < 0.1f)
		{
			ExitDungeon();
		}
	}
	
	void ToggleExitMessage()
	{
		if(PlayerController.Instance.InteractionMngr.ObjectPresent)
		{
			if(PlayerController.Instance.InteractionMngr.ObjectInView == this.gameObject)
				exitMsg.SetActive(true);
			else if(PlayerController.Instance.InteractionMngr.ObjectInView == null || PlayerController.Instance.InteractionMngr.ObjectInView != this.gameObject) 
				exitMsg.SetActive(false);
		}
		else exitMsg.SetActive(false);
	}
	
	void ExitDungeon()
	{
		GameManager.Instance.SaveGame();
		GameManager.Instance.CurrentGameState = GameManager.GameStates.InGame;
        GameManager.Instance.ReturnToMapRoom();
	}
}
