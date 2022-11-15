using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_StateSystem : GameplayComponent
{
    //Direction the player is headed in
    [SerializeField] private Vector3 v3_playerDirection;

    //Direction you want to go in
    [SerializeField] private Vector3 v3_controlDirection;

    //Direction of the mouse
    [SerializeField] private Vector3 v3_aimDirection;

    [SerializeField] private float f_currentSpeed = 0;
    [SerializeField] private float f_currentSpeedAbs = 0;

    [SerializeField] CharacterAttributes attributes;

    public ShootingController shootingController;
    public Hittable healthController;
    public Rigidbody2D rb_playerBody;

    public float f_runSpeed;
    public float f_sneakSpeed;
    public float f_rollSpeed;
    public float f_rollDistance; //Idk what unit this is in

    public float f_verticalSpeed;
    public float f_horizontalSpeed;


    //State bools
    [SerializeField] private bool b_isRolling;
    [SerializeField] private bool b_isStunned;
    [SerializeField] private bool b_isHit;
    [SerializeField] private bool b_isAlive;
    [SerializeField] private bool b_isCrouching;

    [SerializeField] public bool b_controlsDisabled;



    [SerializeField] private float timeSinceLastRoll = 0;
    [SerializeField] private float timeBetweenRolls = 1.5f;

    

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject detectionCollider;
    [SerializeField] private GameObject crouchText;
    [SerializeField] private GameObject damageController;
    [SerializeField] private Collider2D damageCollider;

    [SerializeField] private Collider2D movementCollider;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private GameObject playerCorpse_pf;

    public Sprite playerStand;
    public Sprite playerRoll;

    public Color flashColor;
    public float flashRate = 0.25f;


    public BasePlayerState currentPlayerState;


    // Start is called before the first frame update
    void Start()
    {
        shootingController = GetComponentInChildren<ShootingController>();

        playerAnimator = GetComponentInChildren<Animator>();

        damageCollider = damageController.GetComponent<Collider2D>();

        healthController = GetComponentInChildren<Hittable>();

        rb_playerBody = GetComponent<Rigidbody2D>();

        damageController = healthController.gameObject;

        movementCollider = GetComponent<Collider2D>();

        //Can add an update by automatically grabing a file called "player attributes"
        UpdateAttributes();

        b_controlsDisabled = false;

        v3_playerDirection = new Vector3(1, 0, 0);
        v3_controlDirection = new Vector3(0, 0, 0);
    }

    public void UpdateAttributes()
    {
        f_runSpeed = attributes.f_runSpeed;
        f_sneakSpeed = attributes.f_sneakSpeed;
        f_rollSpeed = attributes.f_rollSpeed;
        f_rollDistance = attributes.f_rollDistance;

        f_verticalSpeed = attributes.f_verticalSpeed;
        f_horizontalSpeed = attributes.f_horizontalSpeed;
    }

    public override void GameplayUpdate()
    {
        if (b_controlsDisabled)
            return;

        if (!b_isAlive)
            return;

        float f_hor = Input.GetAxis("Horizontal");
        float f_ver = Input.GetAxis("Vertical");
        bool roll = Input.GetButton("Fire3");

        b_isCrouching = Input.GetButton("Fire1");

        currentPlayerState?.Process(rb_playerBody, new Vector2(f_hor, f_ver), f_runSpeed, roll, b_isCrouching, b_isHit, b_isStunned);

    }
}
