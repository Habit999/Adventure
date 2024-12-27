using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and Controlls the Player
/// </summary>

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance;
	
	public enum PLAYERSTATE { LockedInteract, LockedMove, FreeMove, Frozen, Dead };
	public PLAYERSTATE PlayerState = PLAYERSTATE.LockedInteract;
	
	[HideInInspector] public bool _isInDungeon;
	
	[Space(10)]
	
	public CustomGrid.ORIENTAION PlayerOrientation;
	
	public float _maxHealth = 100;
	public float _maxMana = 100;
	[HideInInspector] public float _health;
	[HideInInspector] public float _mana;
	
	public Rigidbody PlayerRB {
		get {
			if(playerRB != null) return playerRB;
			else
			{
				if(gameObject.GetComponent<Rigidbody>() == null)
				{
					playerRB = gameObject.AddComponent<Rigidbody>();
					playerRB.constraints = RigidbodyConstraints.FreezeRotation;
				}
				else
				{
					playerRB = gameObject.GetComponent<Rigidbody>(); 
				}
				return playerRB;
			}
		} 
	}
	Rigidbody playerRB;
	
	public InteractionManager InteractionMngr { get { return gameObject.GetComponent<InteractionManager>(); } }
	
	public InventoryManager InventoryMngr { get { return gameObject.GetComponent<InventoryManager>(); } }
	
	public SkillsManager SkillsMngr { get { return gameObject.GetComponent<SkillsManager>(); } }
	
	public CombatManager CombatMngr { get { return damageArea.GetComponent<CombatManager>(); } }
	
	[Space(10)]
	
	public Transform _camera;
	public Transform _body;
	
	[Space(10)]
	
	[SerializeField] GameObject damageArea;
	
	[System.Serializable]
	public struct LockedMovementVariables
	{	
		public float lockedMovementSpeed;
		public float lockedRotationSpeed;
		
		[HideInInspector] public CustomGrid.ORIENTAION targetOrientation;
		
		[HideInInspector] public Vector3 moveTargetPosition;
		[HideInInspector] public Quaternion moveTargetRotation;
		[HideInInspector] public Vector3 currentLockedPosition;
		[HideInInspector] public Quaternion currentLockedRotation;
		
		[HideInInspector] public float positionDistance;
		[HideInInspector] public float rotationDistance;
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
		[Space(5)]
		public float strafeDampening;
		public float movingDrag;
		
		[HideInInspector] public Vector3 forceDirection;
		[HideInInspector] public bool isToggledUI;
		
		[HideInInspector] public float startingDrag;
	}
	[Space(10)]
	public FreeMoveVariables freeMoveVariables = new FreeMoveVariables();
	
	[HideInInspector] public MovePoint _currentMovePoint;
	
	[HideInInspector] public bool _isSprinting;
	
	[HideInInspector] public bool _canLook;
	[HideInInspector] public bool _canMove;
	
	float mouseX;
	float mouseY;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		_health = _maxHealth;
		_mana = _maxMana;
		
		freeMoveVariables.startingDrag = PlayerRB.drag;
		
		_canLook = true;
		_canMove = true;
		
		mouseX = transform.eulerAngles.y;
		mouseY = 0;
	}
	
	void Update()
	{
		PlayerBehaviour();
	}
	
	void FixedUpdate()
	{
		PlayerRB.AddForce(freeMoveVariables.forceDirection);
	}
	
	void PlayerBehaviour()
	{
		CheckOrientation();
		CheckMouse();
		CheckRigidbody();
		
		// Resetting variables
		freeMoveVariables.forceDirection = Vector3.zero;
		
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
				
			case PLAYERSTATE.Frozen:
				FrozenPlayer();
				break;
				
			default:
				break;
		}
	}
	
	#region Routine Checks
	
	void CheckOrientation()
	{
		Vector3 playerForwardDirection = transform.TransformDirection(transform.forward);
		Vector3 worldNorth = transform.TransformDirection(Vector3.forward);
		Vector3 worldEast = transform.TransformDirection(Vector3.right);
		
		float dotProduct = Vector3.Dot(playerForwardDirection, worldNorth);
		
		if(dotProduct > 0.9f) PlayerOrientation = CustomGrid.ORIENTAION.North;
		else if(dotProduct < 0.1 && dotProduct > -0.1)
		{
			dotProduct = Vector3.Dot(playerForwardDirection, worldEast);
		
			if(dotProduct > 0.9f) PlayerOrientation = CustomGrid.ORIENTAION.East;
			else if(dotProduct < -0.9f) PlayerOrientation = CustomGrid.ORIENTAION.West;
		}
		else if(dotProduct < -0.9f) PlayerOrientation = CustomGrid.ORIENTAION.South;
	}
	
	void CheckMouse()
	{
		// Checks mouse visibility and if locked
		if(PlayerState == PLAYERSTATE.LockedInteract || PlayerState == PLAYERSTATE.LockedMove)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else if(PlayerState == PLAYERSTATE.FreeMove)
		{
			if(freeMoveVariables.isToggledUI)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				
				_canLook = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				
				_canLook = true;
			}
		}
		
		// Stores mouse rotation so it's not limited to mouse axis
		if(_canLook)
		{
			mouseX += Controls.MouseX * freeMoveVariables.mouseSensitivity;
			mouseY += Controls.MouseY * freeMoveVariables.mouseSensitivity;
			mouseY = Mathf.Clamp(mouseY, -80, 80);
		}
	}
	
	void CheckRigidbody()
	{
		if(PlayerState == PLAYERSTATE.Dead)
		{
			PlayerRB.constraints = RigidbodyConstraints.None;
		}
		else if(PlayerState == PLAYERSTATE.LockedInteract || PlayerState == PLAYERSTATE.LockedMove || PlayerState == PLAYERSTATE.Frozen)
		{
			PlayerRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
		}
		else if(PlayerState == PLAYERSTATE.FreeMove)
		{
			PlayerRB.constraints = RigidbodyConstraints.FreezeRotation;
			
			if(Input.GetKey(Controls.Forward) || Input.GetKey(Controls.Backward) || Input.GetKey(Controls.Left) || Input.GetKey(Controls.Right))
			{
				PlayerRB.drag = freeMoveVariables.movingDrag;
			}
			else
			{
				PlayerRB.drag = freeMoveVariables.startingDrag;
			}
		}
	}
	
	#endregion
	
	#region Public Functions
	
	public void FreezePlayer(bool hideBody, bool disableCamera)
	{
		PlayerState = PLAYERSTATE.Frozen;
		
		if(hideBody) _body.gameObject.SetActive(false);
		else _body.gameObject.SetActive(true);
		
		if(disableCamera) _camera.gameObject.SetActive(false);
		else _camera.gameObject.SetActive(true);
	}
	
	public void UnFreezePlayer(PLAYERSTATE targetState)
	{
		PlayerState = targetState;
		
		_body.gameObject.SetActive(true);
		_camera.gameObject.SetActive(true);
	}
	
	public void HealPlayer(float health, float mana)
	{
		_health += health;
		_health = Mathf.Clamp(_health, 0, _maxHealth);
		
		_mana += mana;
		_mana = Mathf.Clamp(_mana, 0, _maxMana);
	}
	
	public void DamagePlayer(float damage)
	{
		_health -= damage;
		
		if(_health <= 0) PlayerState = PLAYERSTATE.Dead;
	}
	
	public void ClickMoveToPoint(Transform targetLocation, Transform orientation)
	{
		lockedMovementVariables.rotationDistance = 0;
		
		lockedMovementVariables.currentLockedPosition = transform.position;
		lockedMovementVariables.currentLockedRotation = transform.rotation;
		lockedMovementVariables.moveTargetPosition = targetLocation.position;
		lockedMovementVariables.moveTargetRotation = orientation.rotation;
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
		/*(lockedMovementVariables.rotationDistance += (lockedMovementVariables.lockedMovementSpeed / 100) * Time.deltaTime;
		lockedMovementVariables.rotationDistance = Mathf.Clamp(lockedMovementVariables.rotationDistance, 0, 1);*/
		lockedMovementVariables.positionDistance += lockedMovementVariables.lockedMovementSpeed * Time.deltaTime;
		lockedMovementVariables.positionDistance += lockedMovementVariables.lockedRotationSpeed * Time.deltaTime;
		
		transform.position = Vector3.Lerp(lockedMovementVariables.currentLockedPosition, lockedMovementVariables.moveTargetPosition, lockedMovementVariables.positionDistance);
		transform.rotation = Quaternion.Lerp(lockedMovementVariables.currentLockedRotation, lockedMovementVariables.moveTargetRotation, lockedMovementVariables.rotationDistance);
		
		print("Locked Movement MOVING");
		if(transform.position == lockedMovementVariables.moveTargetPosition && transform.rotation == lockedMovementVariables.moveTargetRotation)
		{
			//_currentMovePoint._orientation.transform.rotation = transform.rotation;
			print("Locked Movement COMPLETE");
			PlayerState = PLAYERSTATE.LockedInteract;
			lockedMovementVariables.rotationDistance = 0;
			return;
		}
	}
	
	void FreeMovement()
	{
		// Mouse
		if(_canLook)
		{
			_body.rotation = Quaternion.Euler(0, mouseX, 0);
			_camera.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
		}
		
		// W A S D
		if(_canMove)
		{
			if(Input.GetKey(Controls.Sprint)) _isSprinting = true;
			else _isSprinting = false;
			
			float speed = _isSprinting? freeMoveVariables.sprintMoveSpeed : freeMoveVariables.walkMoveSpeed;
			
			bool dampenStrafe = false;
			if(Input.GetKey(Controls.Forward))
			{
				freeMoveVariables.forceDirection += _body.forward * speed;
				dampenStrafe = true;
			}
			if(Input.GetKey(Controls.Backward))
			{
				freeMoveVariables.forceDirection += -_body.forward * speed;
				dampenStrafe = true;
			}
			if(Input.GetKey(Controls.Left))
			{
				if(dampenStrafe) freeMoveVariables.forceDirection += (-_body.right * speed) / freeMoveVariables.strafeDampening;
				else freeMoveVariables.forceDirection += -_body.right * speed;
			}
			if(Input.GetKey(Controls.Right))
			{
				if(dampenStrafe) freeMoveVariables.forceDirection += (_body.right * speed) / freeMoveVariables.strafeDampening;
				else freeMoveVariables.forceDirection += _body.right * speed;
			}
			
			// Interaction
			if(Input.GetKey(Controls.Interact))
			{
				InteractionMngr.Interact();
			}
		}
		
		//Toggle UI Interaction
		if(_isInDungeon)
		{
			if(Input.GetKey(Controls.ToggleUI))
			{
				freeMoveVariables.isToggledUI = true;
			}
			else
			{
				// Close UI menu's if open
				UserInterfaceController controllerUI = UserInterfaceController.Instance;
				if(controllerUI._isInvOpen) controllerUI.ToggleInventory();
				if(controllerUI._isSkillsOpen) controllerUI.ToggleSkills();
				
				freeMoveVariables.isToggledUI = false;
			}
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
		lockedMovementVariables.positionDistance = 1;
		lockedMovementVariables.rotationDistance = 0;
		
		lockedMovementVariables.currentLockedPosition = transform.position;
		lockedMovementVariables.currentLockedRotation = transform.rotation;
		lockedMovementVariables.moveTargetPosition = transform.position;
		lockedMovementVariables.moveTargetRotation = transform.rotation;
		lockedMovementVariables.moveTargetRotation.y += 90;
		PlayerState = PLAYERSTATE.LockedMove;
	}
	
	public void RotateLeft()
	{
		lockedMovementVariables.positionDistance = 1;
		lockedMovementVariables.rotationDistance = 0;
		
		lockedMovementVariables.currentLockedPosition = transform.position;
		lockedMovementVariables.currentLockedRotation = transform.rotation;
		lockedMovementVariables.moveTargetPosition = transform.position;
		lockedMovementVariables.moveTargetRotation = transform.rotation;
		lockedMovementVariables.moveTargetRotation.y += -90;
		PlayerState = PLAYERSTATE.LockedMove;
	}
	
	#endregion
}
