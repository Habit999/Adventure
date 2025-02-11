using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public enum PLAYERSTATE { FreeLook, InMenu, Frozen, Dead };
    public PLAYERSTATE PlayerState;

    public InteractionManager InteractionMngr { get { return gameObject.GetComponent<InteractionManager>(); } }

    public InventoryManager InventoryMngr { get { return gameObject.GetComponent<InventoryManager>(); } }

    public SkillsManager SkillsMngr { get { return gameObject.GetComponent<SkillsManager>(); } }

    public CombatManager CombatMngr { get { return damageArea.GetComponent<CombatManager>(); } }

    public event Action OnDeath;
    public event Action OnVanish;

    [HideInInspector] public bool IsInDungeon;

    public Transform Camera;
    public Transform Body;
    [SerializeField] GameObject damageArea;

    [Space(5)]

    public float MaxHealth;
    public float MaxMana;
    [HideInInspector] public float Health;
    [HideInInspector] public float Mana;

    private float mouseX;
    private float mouseY;

    private Vector3 moveDirection;
    private Rigidbody rb;

    [Space(5)]

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] [Range(0, 1)] private float strafeModifier;

    [HideInInspector] public bool MouseToggled;

    private bool canLook = true;
    private bool canMove = true;

    private bool isMoving;
    private bool isSprinting;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Health = MaxHealth;
        Mana = MaxMana;

        mouseY = 0;
    }

    private void Update()
    {
        PlayerBehaviour();
        CheckBodyState();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection);
        moveDirection = Vector3.zero;
    }

    #region Publics

    public void ApplyExternalForce(Vector3 appliedForce)
    {
        moveDirection += appliedForce;
    }

    public void KillPlayer()
    {
        PlayerState = PLAYERSTATE.Dead;
        OnDeath();
    }

    public void VanishPlayer()
    {
        PlayerState = PLAYERSTATE.Dead;
        OnVanish();
    }

    public void DamagePlayer(float damage)
    {
        Health -= damage;
    }

    public void HealPlayer(float healing, float manaRecovery)
    {
        Health += healing;
        Mana += manaRecovery;
    }

    public void FreezePlayer(bool lockCamera, bool lockMovement)
    {
        PlayerState = PLAYERSTATE.Frozen;
        canLook = lockCamera;
        canMove = lockMovement;
    }

    public void UnFreezePlayer()
    {
        PlayerState = PLAYERSTATE.FreeLook;
        canLook = true;
        canMove = true;
    }

    #endregion

    private void PlayerBehaviour()
    {
        switch (PlayerState)
        {
            case PLAYERSTATE.FreeLook:
                FreeMovement();
                break;

            case PLAYERSTATE.InMenu:
                FreeMovement();
                break;

            case PLAYERSTATE.Frozen:
                break;

            case PLAYERSTATE.Dead:
                break;

            default:
                break;
        }
    }

    private void FreeMovement()
    {
        // Moving
        if (Input.GetKey(KeyCode.LeftShift)) isSprinting = true;
        else isSprinting = false;

        isMoving = false;
        if (canMove)
        {
            bool nonStrafeMovement = false;
            if (Input.GetKey(GameManager.Instance.Controls.Forward))
            {
                moveDirection += Body.forward * (isSprinting ? sprintSpeed : walkSpeed);
                isMoving = true;
                nonStrafeMovement = true;
            }
            if (Input.GetKey(GameManager.Instance.Controls.Backward))
            {
                moveDirection += -Body.forward * (isSprinting ? sprintSpeed : walkSpeed);
                isMoving = true;
                nonStrafeMovement = true;
            }
            if (Input.GetKey(GameManager.Instance.Controls.Right))
            {
                moveDirection += Body.right * (isSprinting ? sprintSpeed : walkSpeed) * (nonStrafeMovement ? strafeModifier : 1);
                isMoving = true;
            }
            if (Input.GetKey(GameManager.Instance.Controls.Left))
            {
                moveDirection += -Body.right * (isSprinting ? sprintSpeed : walkSpeed) * (nonStrafeMovement ? strafeModifier : 1);
                isMoving = true;
            }
        }

        // Looking & Rotation
        if (canLook)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
            mouseY = Mathf.Clamp(mouseY, -80, 80);

            Camera.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
            Body.rotation = Quaternion.Euler(0, mouseX, 0);
        }

        // Switch State
        if (Input.GetKey(GameManager.Instance.Controls.ToggleUI))
            PlayerState = PLAYERSTATE.InMenu;
        else PlayerState = PLAYERSTATE.FreeLook;
    }

    private void CheckBodyState()
    {
        switch (PlayerState)
        {
            case PLAYERSTATE.FreeLook:
                canLook = true;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

            case PLAYERSTATE.InMenu:
                canLook = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;

            case PLAYERSTATE.Frozen:
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                Cursor.lockState = canLook ? CursorLockMode.Confined : CursorLockMode.Locked;
                Cursor.visible = !canLook;
                break;

            case PLAYERSTATE.Dead:
                canLook = false;
                canMove = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                rb.constraints = RigidbodyConstraints.None;
                break;

            default:
                break;
        }
    }
}
