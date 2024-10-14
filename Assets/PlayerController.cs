using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and Controlls the Player
/// </summary>

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;
	
	public enum PLAYERSTATE { LockedInteract, LockedMove, FreeMove };
	public static PLAYERSTATE PlayerState = PLAYERSTATE.LockedInteract;
	
	public static Transform PlayerCamera { get { return Instance.playerCamera; } private set { Instance.playerCamera = value; } }
	[SerializeField] Transform playerCamera;
	
	public float _health = 100;
	
	[System.Serializable]
	public struct LockedMovementVariables
	{
		public float lockedMovementSpeed;
		public float lockedRotationSpeed;
	}
	public LockedMovementVariables lockedMovementVariables = new LockedMovementVariables();
	
	[HideInInspector] public MovePoint _currentMovePoint;
	
	Vector3 moveTargetPosition;
	Quaternion moveTargetRotation;
	Vector3 currentLockedPosition;
	Quaternion currentLockedRotation;
	
	float distanceFromTarget;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	
	void Update()
	{
		PlayerBehaviour();
	}
	
	void PlayerBehaviour()
	{
		switch(PlayerState)
		{
			case PLAYERSTATE.LockedInteract:
				LockedInteraction();
				break;
				
			case PLAYERSTATE.LockedMove:
				LockedMovement();
				break;
				
			default:
				break;
		}
	}
	
	public void DamagePlayer(float damage)
	{
		_health -= damage;
		
		if(_health <= 0) Destroy(this.gameObject);
	}
	
	public void ClickMoveToPoint(Transform targetLocation, Transform orientation)
	{
		distanceFromTarget = 0;
		
		currentLockedPosition = transform.position;
		currentLockedRotation = transform.rotation;
		moveTargetPosition = targetLocation.position;
		moveTargetRotation = orientation.rotation;
		PlayerState = PLAYERSTATE.LockedMove;
	}
	
	#region Player State Machine
	
	void LockedInteraction()
	{
		if(Input.GetKeyDown(KeyCode.D)) RotateRight();
		else if(Input.GetKeyDown(InputManager.MoveLeft)) RotateLeft();
	}
	
	void LockedMovement()
	{
		distanceFromTarget += (lockedMovementVariables.lockedMovementSpeed / 100) * Time.deltaTime;
		distanceFromTarget = Mathf.Clamp(distanceFromTarget, 0, 1);
		
		transform.position = Vector3.Lerp(currentLockedPosition, moveTargetPosition, distanceFromTarget);
		transform.rotation = Quaternion.Lerp(currentLockedRotation, moveTargetRotation, distanceFromTarget);
		print("Locked Movement MOVING");
		if(transform.position == moveTargetPosition && Quaternion.Angle(transform.rotation, moveTargetRotation) < 0.1f)
		{
			_currentMovePoint._orientation.transform.rotation = transform.rotation;
			print("Locked Movement COMPLETE");
			PlayerState = PLAYERSTATE.LockedInteract;
			distanceFromTarget = 0;
			return;
		}
	}
	
	#endregion
	
	#region Movement & Rotation
	
	public void RotateRight()
	{
		distanceFromTarget = 0;
		
		currentLockedPosition = transform.position;
		currentLockedRotation = transform.rotation;
		moveTargetPosition = transform.position;
		Quaternion tempRotation = Quaternion.Euler(0, _currentMovePoint._orientation.transform.eulerAngles.y + 90, 0);
		moveTargetRotation = tempRotation;
		PlayerState = PLAYERSTATE.LockedMove;
	}
	
	public void RotateLeft()
	{
		distanceFromTarget = 0;
		
		currentLockedPosition = transform.position;
		currentLockedRotation = transform.rotation;
		moveTargetPosition = transform.position;
		Quaternion tempRotation = Quaternion.Euler(0, _currentMovePoint._orientation.transform.eulerAngles.y - 90, 0);
		moveTargetRotation = tempRotation;
		PlayerState = PLAYERSTATE.LockedMove;
	}
	
	#endregion
}
