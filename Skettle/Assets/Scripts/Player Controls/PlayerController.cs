using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameplayComponent
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


    public float f_runSpeed;
    public float f_sneakSpeed;
    public float f_rollSpeed;
    public float f_rollDistance; //Idk what unit this is in

    public float f_verticalSpeed;
    public float f_horizontalSpeed;

    [SerializeField] public bool b_controlsDisabled;

    //Bools if you're hit staggered or rolling
    [SerializeField] private bool b_isRolling;
    [SerializeField] private bool b_isHit;

    [SerializeField] private float timeSinceLastRoll = 0;
    [SerializeField] private float timeBetweenRolls = 1.5f;

    //Add a listener for when isSneaking is changed
    [SerializeField] private bool b_isSneaking;

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject detectionCollider;
    [SerializeField] private GameObject crouchText;
    [SerializeField] private GameObject damageController;
    [SerializeField] private Collider2D damageCollider;

    [SerializeField] private Collider2D movementCollider;

    [SerializeField] private Animator playerAnimator;

    public Sprite playerStand;
    public Sprite playerRoll;

    // Start is called before the first frame update
    void Start()
    {
        shootingController = GetComponentInChildren<ShootingController>();

        playerAnimator = GetComponentInChildren<Animator>();

        damageCollider = damageController.GetComponent<Collider2D>();

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

    // Update is called once per frame
    public override void GameplayUpdate()
    {
        if (b_controlsDisabled)
            return;

        float f_hor = Input.GetAxis("Horizontal");
        float f_ver = Input.GetAxis("Vertical");
        bool roll = Input.GetButton("Fire3");
        b_isSneaking = Input.GetButton("Fire1"); //I Think this is ctrl. Need to rename the inputs

        if(b_isSneaking)
        {
            
            crouchText.SetActive(true);
            detectionCollider.layer = 9;
        }
        else
        {
            crouchText.SetActive(false);
            detectionCollider.layer = 0;
        }

        playerAnimator.SetBool("Crouch", b_isSneaking);

        if (b_isRolling)
        {
            //playerSprite.sprite = playerRoll;
            playerSprite.sortingLayerName = "AboveBullets";
            damageCollider.enabled = false;
            damageController.layer = 10;
            this.gameObject.layer = 10;
        }
        else
        {
            //playerSprite.sprite = playerStand;
            playerSprite.sortingLayerName = "Default";
            damageCollider.enabled = true;
            damageController.layer = 8;
            this.gameObject.layer = 0;
        }
       

        v3_controlDirection = new Vector3(f_hor, f_ver).normalized;

        Move(v3_controlDirection, roll);

        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        if (shootingController.shootingEnabled)
        {
            v3_playerDirection = (shootingController.mousePos - gameObject.transform.position).normalized;
            playerAnimator.SetFloat("Vertical_Movement", v3_playerDirection.y);
            playerAnimator.SetFloat("Horizontal_Movement", v3_playerDirection.x);

        }
        else
        {
            //Get the last direction the player moved in. Used to calculate direction facing
            v3_playerDirection = v3_controlDirection;
            playerAnimator.SetFloat("Vertical_Movement", v3_playerDirection.y);
            playerAnimator.SetFloat("Horizontal_Movement", v3_playerDirection.x);
        }

        if(Vector3.Dot(v3_controlDirection,v3_playerDirection) <= 0)
        {
            f_currentSpeedAbs = -f_currentSpeed;
        }
        else
        {
            f_currentSpeedAbs = f_currentSpeed;
        }

        playerAnimator.SetFloat("Speed", f_currentSpeedAbs);
    }

    public void Move(Vector3 v3_controlDir, bool b_roll)
    {
        if (b_isHit)
            return;

        if (b_isRolling)
        {
            timeSinceLastRoll = 0;
            return;
        }


        if (b_roll && (timeSinceLastRoll > timeBetweenRolls))
        {
            b_isRolling = true;
            StartCoroutine(RollProcess());

            playerAnimator.SetTrigger("Roll");

            //Start roll animation
            //Start "physics" calculations
        }

        timeSinceLastRoll += Time.deltaTime;

        if (v3_controlDir.magnitude < 0.01 && v3_controlDir.magnitude > -0.01)
        {
            f_currentSpeed = 0.0f;
        }
        else
        {
            if (b_isSneaking)
            {
                f_currentSpeed = f_sneakSpeed;
            }
            else
            {
                f_currentSpeed = f_runSpeed;
            }
        }

        //Probably need to change to a rigidbody system
        transform.position += v3_controlDir * f_currentSpeed * Time.deltaTime;
    }

    //TODO: Test the OnHit Disruption system. Currently not implemented

    public void OnHit(bool disrupting, float severity, Vector3 incomingDirection)
    {
        if (disrupting)
        {
            playerAnimator.SetTrigger("Knockback");
        }

        b_isHit = disrupting;
        b_isSneaking = !b_isHit;
        StartCoroutine(KnockBackprocess(severity, incomingDirection));
    }

    IEnumerator KnockBackprocess(float severity, Vector3 incomingDir)
    {
        Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        while(b_isHit)
        {
            //Figure out a good either piecewise function for severity or something. 
            //Small shots shouldn't effect the player, rough shots should quickly stop the player, severe shots should send the player flying back.
            transform.position += v3_playerDirection * severity * Time.deltaTime;

            if ((startingPos - transform.position).magnitude >= severity*5f)
            {
                b_isRolling = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RollProcess()
    {
        //Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //This should all the player to roll into an object and not get stuck
        float distanceRolled = 0f;


        //Change collision layers to not collide with the tables
        //movementCollider.

        while (b_isRolling)
        {
            transform.position += v3_playerDirection * f_rollSpeed * Time.deltaTime;

            distanceRolled += (v3_playerDirection * f_rollSpeed * Time.deltaTime).magnitude;

            if (/*(startingPos - transform.position).magnitude*/ distanceRolled >= f_rollDistance)
            {
                ExitRoll();
            }
            
            yield return new WaitForEndOfFrame(); 
        }
    }

    public void ExitRoll()
    {
        b_isRolling = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        /* Original solution to being stuck on a wall while rolling. Might include it as a bonk mechanic
        b_isRolling = false;
        v3_controlDirection = new Vector3(0, 0, 0);
        Move(v3_controlDirection, false);
        */
    }
}
