using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and Controlls the Player
/// </summary>

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;
	
	public enum PLAYERSTATE { LockedInteract, LockedMove, FreeMove, FreezePlayer };
	public PLAYERSTATE PlayerState = PLAYERSTATE.LockedInteract;
	
	public float _health = 100;
	
	public Rigidbody PlayerRB {
		get {
			Rigidbody rb;
			if(gameObject.GetComponent<Rigidbody>() == null)
			{
				rb = gameObject.AddComponent<Rigidbody>();
				rb.constraints = RigidbodyConstraints.FreezeRotation;
				return rb;
			}
			else return gameObject.GetComponent<Rigidbody>(); 
		} 
	}
	
	public InteractionManager InteractionMngr { get { return gameObject.GetComponent<InteractionManager>(); } }
	
	[Space(10)]
	
	public Transform _camera;
	public Transform _body;
	
	[System.Serializable]
	public struct LockedMovementVariables
	{	
		public float lockedMovementSpeed;
		public float lockedRotationSpeed;
	}
	[Space(20)]
	[SerializeField] LockedMovementVariables lockedMovementVariables = new LockedMovementVariables();
	
	[System.Serializable]
	public struct FreeMoveVariables
	{
		public float mouseSensitivity;
		[Space(5)]
		public ForceMode forceMode;
		public float walkMoveSpeed;
		public float sprintMoveSpeed;
	}
	[Space(10)]
	[SerializeField] FreeMoveVariables freeMoveVariables = new FreeMoveVariables();
	
	[HideInInspector] public MovePoint _currentMovePoint;
	
	float mouseX;
	float mouseY;
	
	// Locked movement dependant
	Vector3 moveTargetPosition;
	Quaternion moveTargetRotation;
	Vector3 currentLockedPosition;
	Quaternion currentLockedRotation;
	
	// Free movement dependant
	Vector3 forceDirection;
	[HideInInspector] public bool _isSprinting;
	
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
	
	void FixedUpdate()
	{
		PlayerRB.AddForce(forceDirection, freeMoveVariables.forceMode);
	}
	
	void PlayerBehaviour()
	{
		CheckMouse();
		
		// Resetting variables
		forceDirection = Vector3.zero;
		
		switch(PlayerState)
		{
			case PLAYERSTATE.LockedInteract:
				LockedInteraction();
				break;
				
			case PLAYERSTATE.LockedMove:
				LockedMovement();
				break;
				
			case PLAYERSTATE.FreeMove:
				FreeMovement();
				break;
				
			case PLAYERSTATE.FreezePlayer:
				FrozenPlayer();
				break;
				
			default:
				break;
		}
	}
	
	void CheckMouse()
	{
		// Stores mouse rotation so it's not limited to mouse axis
		mouseX += Controls.MouseX * freeMoveVariables.mouseSensitivity;
		mouseY += Controls.MouseY * freeMoveVariables.mouseSensitivity;
		mouseY = Mathf.Clamp(mouseY, -80, 80);
		
		// Checks mouse visibility and if locked
		if(PlayerState == PLAYERSTATE.LockedInteract || PlayerState == PLAYERSTATE.LockedMove)
		{
			if(Cursor.lockState != CursorLockMode.None) Cursor.lockState = CursorLockMode.None;
			if(!Cursor.visible) Cursor.visible = true;
		}
		else if(PlayerState == PLAYERSTATE.FreeMove)
		{
			if(Cursor.lockState != CursorLockMode.Locked) Cursor.lockState = CursorLockMode.Locked;
			if(Cursor.visible) Cursor.visible = false;
		}
	}
	
	#region Public Functions
	
	public void FreezePlayer()
	{
		PlayerState = PLAYERSTATE.FreezePlayer;
	}
	
	public void UnFreezePlayer(PLAYERSTATE targetState)
	{
		PlayerState = targetState;
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
	
	#endregion
	
	#region Player State Functions
	
	void LockedInteraction()
	{
		if(Input.GetKeyDown(Controls.Right)) RotateRight();
		else if(Input.GetKeyDown(Controls.Left)) RotateLeft();
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
	
	void FreeMovement()
	{
		// Mouse
		_body.rotation = Quaternion.Euler(0, mouseX, 0);
		_camera.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
		
		// W A S D
		if(Input.GetKey(Controls.Sprint)) _isSprinting = true;
		else _isSprinting = false;
		
		float speed = _isSprinting? freeMoveVariables.sprintMoveSpeed : freeMoveVariables.walkMoveSpeed;
		if(Input.GetKey(Controls.Forward))
		{
			forceDirection += _body.forward * speed;
		}
		if(Input.GetKey(Controls.Backward))
		{
			forceDirection += -_body.forward * speed;
		}
		if(Input.GetKey(Controls.Left))
		{
			forceDirection += -_body.right * speed;
		}
		if(Input.GetKey(Controls.Right))
		{
			forceDirection += _body.right * speed;
		}
		
		// Interaction
		if(Input.GetKey(Controls.Interact))
		{
			InteractionManager.INTERACTIONOUTCOMES outcome = InteractionMngr.Interact();
			print(outcome);
		}
	}
	
	void FrozenPlayer()
	{
		// If empty by end of development: delete and remove from state machine
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
