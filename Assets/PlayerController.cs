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

    public CombatManager CombatMngr;

    public event Action OnDeath;
    public event Action OnVanish;

    public event Action<float, float> OnHealthChange;

    [HideInInspector] public SO_Controls InputControls;

    [Space(5)]

    public bool IsInDungeon;

    [Space(5)]

    public Transform Camera;
    public Transform Body;

    [Space(5)]

    [SerializeField] private float maxHealth;
    private float startingMaxHealth;
    private float health;

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

    [ContextMenu("Test Health Change")]
    private void TestHealthChange()
    {
        DamagePlayer(10);
    }

    [ContextMenu("Test Experience Change")]
    private void TestExperienceChange()
    {
        SkillsMngr.AddExperience(10);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody>();

        startingMaxHealth = maxHealth;
    }

    private void OnDisable()
    {
        OnDeath = null;
        OnVanish = null;
        OnHealthChange = null;
    }

    private void Start()
    {
        health = maxHealth;
        if(IsInDungeon)
            OnHealthChange(health, maxHealth);

        InputControls = GameManager.Instance.Controls;
        mouseX = transform.eulerAngles.y;

        CheckBodyState();
    }

    private void Update()
    {
        PlayerBehaviour();
    }

    private void FixedUpdate()
    {
        rb.velocity =  moveDirection;
        moveDirection = Vector3.zero;
    }

    #region Publics

    public bool CheckInView(Transform target, float viewRange)
    {
        float product = Vector3.Dot(Body.forward, (target.position - Body.position).normalized);
        if (product > viewRange) return true;
        else return false;
    }

    public void ApplyExternalForce(Vector3 appliedForce)
    {
        moveDirection += appliedForce;
    }

    public void KillPlayer()
    {
        SwitchPlayerState(PLAYERSTATE.Dead);
        OnDeath();
    }

    public void VanishPlayer()
    {
        SwitchPlayerState(PLAYERSTATE.Dead);
        OnVanish();
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
        OnHealthChange(health, maxHealth);
    }

    public IEnumerator HealPlayer(Item item, float healing)
    {
        yield return new WaitWhile(() => CombatMngr.AnimationTimer > 0);

        health = Mathf.Clamp(health + healing, 0, maxHealth);
        OnHealthChange(health, maxHealth);

        foreach(var collectedItem in InventoryMngr.CollectedItems.Keys)
        {
            if (collectedItem.GetComponent<Item>().ItemData.Name == item.ItemData.Name)
            {
                InventoryMngr.RemoveItem(collectedItem, 1);
                break;
            }
        }
    }

    public void FreezePlayer(bool lockCamera, bool lockMovement)
    {
        SwitchPlayerState(PLAYERSTATE.Frozen);
        canLook = lockCamera;
        canMove = lockMovement;
    }

    public void UnFreezePlayer()
    {
        SwitchPlayerState(PLAYERSTATE.FreeLook);
        canLook = true;
        canMove = true;
    }

    #endregion

    private void SwitchPlayerState(PLAYERSTATE newState)
    {
        PlayerState = newState;
        CheckBodyState();
    }

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
        moveDirection.y = rb.velocity.y;

        // Moving
        if (Input.GetKey(InputControls.Sprint)) isSprinting = true;
        else isSprinting = false;

        isMoving = false;
        if (canMove)
        {
            bool nonStrafeMovement = false;
            if (Input.GetKey(InputControls.Forward))
            {
                Vector3 newDirection = Body.forward * (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime;
                newDirection.y = 0;
                moveDirection += newDirection;
                isMoving = true;
                nonStrafeMovement = true;
            }
            if (Input.GetKey(InputControls.Backward))
            {
                Vector3 newDirection = -Body.forward * (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime;
                newDirection.y = 0;
                moveDirection += newDirection;
                isMoving = true;
                nonStrafeMovement = true;
            }
            if (Input.GetKey(InputControls.Right))
            {
                Vector3 newDirection = Body.right * (isSprinting ? sprintSpeed : walkSpeed) * (nonStrafeMovement ? strafeModifier : 1) * Time.deltaTime;
                newDirection.y = 0;
                moveDirection += newDirection;
                isMoving = true;
            }
            if (Input.GetKey(InputControls.Left))
            {
                Vector3 newDirection = -Body.right * (isSprinting ? sprintSpeed : walkSpeed) * (nonStrafeMovement ? strafeModifier : 1) * Time.deltaTime;
                newDirection.y = 0;
                moveDirection += newDirection;
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
        if (Input.GetKeyDown(InputControls.ToggleUI))
            SwitchPlayerState(PLAYERSTATE.InMenu);
        else if(Input.GetKeyUp(InputControls.ToggleUI))
            SwitchPlayerState(PLAYERSTATE.FreeLook);
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

    public void UpdateMaxHealth()
    {
        float newHealth = startingMaxHealth + (10 * (SkillsMngr.CurrentSkills.vitality - 1));
        maxHealth = newHealth;
        OnHealthChange(health, maxHealth);
    }
}
