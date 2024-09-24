using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
	[SerializeField] GameObject playerPrefab;
	[Space(5)]
	[SerializeField] GameObject preGameCam;
	
	void Start()
	{
		if(GetComponent<MovePoint>() != null)
		{
			// Destroying Pre-Game Preview Camera
			Destroy(preGameCam);
			
			PlayerController newPlayer = Instantiate(playerPrefab, GetComponent<MovePoint>()._cameraTarget.position, GetComponent<MovePoint>()._cameraTarget.rotation).GetComponent<PlayerController>();
			newPlayer._currentMovePoint = GetComponent<MovePoint>();
		}
		else print("ERROR => StartPoint Doesn't Have A MovePoint Component!");
	}
}
