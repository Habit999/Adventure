using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonExit : MonoBehaviour
{
	public bool _canExit;
	
	[Space(5)]
	
	[SerializeField] Transform exitCam;
	[SerializeField] Transform camTarget;
	[SerializeField] GameObject exitMsg;
	[SerializeField] float camTransitionSpeed;
	
	Vector3 startPoint;
	
	Vector3 transitionDirection;
	
	bool isLeaving;
	
	void Awake()
	{
		exitCam.gameObject.SetActive(false);
		
		startPoint = exitCam.position;
		
		transitionDirection = (camTarget.position - startPoint).normalized * camTransitionSpeed * Time.deltaTime;
		
		isLeaving = false;
	}
	
	void Update()
	{
		if(isLeaving) TransitionCam();
		
		ToggleExitMessage();
	}
	
	public void Exit()
	{
		if(_canExit)
		{
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
		GameManager.Instance.ReturnToMapRoom();
	}
}
