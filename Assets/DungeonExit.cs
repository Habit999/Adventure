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
	
	bool isLeaving;
	
	void Awake()
	{
		exitCam.gameObject.SetActive(false);
		
		startPoint = exitCam.localPosition;
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
		exitCam.localPosition = (camTarget.localPosition - startPoint).normalized * Time.deltaTime * camTransitionSpeed;
		
		if(Vector3.Distance(exitCam.localPosition, camTarget.localPosition) < 0.1f)
		{
			ExitDungeon();
		}
	}
	
	void ToggleExitMessage()
	{
		if(PlayerController.Instance.InteractionMngr._objectPresent)
		{
			if(PlayerController.Instance.InteractionMngr._objectInView == this.gameObject)
				exitMsg.SetActive(true);
			else if(PlayerController.Instance.InteractionMngr._objectInView == null || PlayerController.Instance.InteractionMngr._objectInView != this.gameObject) 
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
