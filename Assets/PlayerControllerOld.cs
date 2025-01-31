using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages and Controlls the Player
/// </summary>

public class PlayerControllerOld : MonoBehaviour
{
	/*public static PlayerController Instance;
	
	public enum PLAYERSTATE { FreeLook, InMenu, Dead };
	public PLAYERSTATE PlayerState = PLAYERSTATE.FreeLook;
	
	[HideInInspector] public bool _isInDungeon;
	
	public float _maxHealth = 100;
	public float _maxMana = 100;
	[HideInInspector] public float _health;
	[HideInInspector] public float _mana;
	
	public InteractionManager InteractionMngr { get { return gameObject.GetComponent<InteractionManager>(); } }
	
	public InventoryManager InventoryMngr { get { return gameObject.GetComponent<InventoryManager>(); } }
	
	public SkillsManager SkillsMngr { get { return gameObject.GetComponent<SkillsManager>(); } }
	
	public CombatManager CombatMngr { get { return damageArea.GetComponent<CombatManager>(); } }
	
	[Space(10)]
	
	[HideInInspector] public Rigidbody _playerRB;
	
	public Transform _camera;
	public Transform _body;
	
	[Space(10)]
	
	[SerializeField] GameObject damageArea;
	
	[System.Serializable]
	public struct MovementVariables
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
		
		[HideInInspector] public float startingDrag;
	}
	[Space(10)]
	public MovementVariables freeMoveVariables = new MovementVariables();
	
	[HideInInspector] public bool _isSprinting;
	
	[HideInInspector] public bool _canLook;
	[HideInInspector] public bool _canMove;

    [HideInInspector] public bool isToggledUI;

    float mouseX;
	float mouseY;
	
	void Awake()
	{
		if(Instance == null) Instance = this;
		else Destroy(this.gameObject);
	}
	
	void Start()
	{
		_playerRB = GetComponent<Rigidbody>();
		
		_health = _maxHealth;
		_mana = _maxMana;
		
		freeMoveVariables.startingDrag = _playerRB.drag;
		
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
		_playerRB.AddForce(freeMoveVariables.forceDirection);
	}
	
	void PlayerBehaviour()
	{
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
			_playerRB.constraints = RigidbodyConstraints.None;
		}
		else if(PlayerState == PLAYERSTATE.LockedInteract || PlayerState == PLAYERSTATE.LockedMove || PlayerState == PLAYERSTATE.Frozen)
		{
			_playerRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
		}
		else if(PlayerState == PLAYERSTATE.FreeMove)
		{
			_playerRB.constraints = RigidbodyConstraints.FreezeRotation;
			
			if(Input.GetKey(Controls.Forward) || Input.GetKey(Controls.Backward) || Input.GetKey(Controls.Left) || Input.GetKey(Controls.Right))
			{
				_playerRB.drag = freeMoveVariables.movingDrag;
			}
			else
			{
				_playerRB.drag = freeMoveVariables.startingDrag;
			}
		}
	}
	
	#endregion
	
	#region Public Functions
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
	#endregion
	
	#region Player State Functions
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
	#endregion*/
}
