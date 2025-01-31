using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
	public static StartPoint Instance;
	
	[SerializeField] GameObject playerPrefab;
	[Space(5)]
	[SerializeField] GameObject preGameCam;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else
		{
			print("More than one StartPoint detected - Destroying Extra");
			Destroy(this);
		}
	}
	
	void Start()
	{
		if(GetComponent<MovePoint>() != null)
		{
			// Destroying Pre-Game Preview Camera
			Destroy(preGameCam);
			
			PlayerController newPlayer = Instantiate(playerPrefab, GetComponent<MovePoint>()._cameraTarget.position, GetComponent<MovePoint>()._cameraTarget.rotation).GetComponent<PlayerController>();
		}
		else print("ERROR => StartPoint Doesn't Have A MovePoint Component!");
	}
}
