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

    //Bools if you're hit staggered or rolling
    [SerializeField] private bool b_isRolling;
    [SerializeField] private bool b_isHit;
    [SerializeField] private bool b_isCrouched;
    [SerializeField] private bool b_isStunned;
    [SerializeField] private bool b_isKnockedBack;
    [SerializeField] private bool b_isAlive;

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



    // Start is called before the first frame update
    void Start()
    {
        shootingController = GetComponentInChildren<ShootingController>();

        playerAnimator = GetComponentInChildren<Animator>();

        damageCollider = damageController.GetComponent<Collider2D>();

        healthController = GetComponentInChildren<Hittable>();

        damageController = healthController.gameObject;

        movementCollider = GetComponent<Collider2D>();

        //Can add an update by automatically grabing a file called "player attributes"
        UpdateAttributes();

        b_controlsDisabled = false;
        b_isAlive = true;
        b_isStunned = false;


        v3_playerDirection = new Vector3(1, 0, 0);
        v3_controlDirection = new Vector3(0, 0, 0);
    }

    public void EnableDisableControls(bool enabled)
    {
        b_controlsDisabled = !enabled;
        shootingController.shootingEnabled = enabled;
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

        b_isCrouched = Input.GetButton("Fire1"); //I Think this is ctrl. Need to rename the inputs

        if(b_isRolling || b_isStunned || b_isHit)
        {
            b_isCrouched = false;
        }

        if(b_isCrouched)
        {
            crouchText.SetActive(true);
            detectionCollider.layer = 9;
        }
        else
        {
            crouchText.SetActive(false);
            detectionCollider.layer = 0;
        }

        

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

        Move(v3_controlDirection,roll);
        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        playerAnimator.SetBool("Crouch", b_isCrouched);

        playerAnimator.SetBool("Roll", b_isRolling);

        playerAnimator.SetBool("Knockback", b_isHit);

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

        if (b_isStunned)
            return;

        if (b_isKnockedBack)
            return;

        if (b_isRolling)
        {
            timeSinceLastRoll = 0;
            return;
        }


        if (b_roll && (timeSinceLastRoll > timeBetweenRolls))
        {
            b_isRolling = true;
            StartCoroutine(RollProcess(v3_playerDirection));
        }

        timeSinceLastRoll += Time.deltaTime;

        if (v3_controlDir.magnitude < 0.01 && v3_controlDir.magnitude > -0.01)
        {
            f_currentSpeed = 0.0f;
        }
        else
        {
            if (b_isCrouched)
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

    public void OnHit(bool disrupting, float force, Vector3 incomingDirection)
    {
        if (b_isStunned)
            b_isStunned = false;

        b_isHit = disrupting;
        if (b_isHit)
        {
            b_isCrouched = false;
            StartCoroutine(KnockBackprocess(force, incomingDirection));
        }
    }

    public void OnStun(float stunLength)
    {
        if (!b_isHit || !b_isStunned)
        {
            b_isStunned = true;
            StartCoroutine(StunProcess(stunLength));
        }
    }

    public void OnStun(float stunLength, bool disrupting, float distance, float knockbackSpeed, Vector3 incomingDirection)
    {
        //Change the way knockback works. Change the b_isHit boolon line 318
        b_isKnockedBack = disrupting;
        if (b_isKnockedBack)
        {
            StartCoroutine(KnockBackprocess(distance, knockbackSpeed, incomingDirection));
        }

        if (!b_isHit || !b_isStunned)
        {
            b_isStunned = true;
            StartCoroutine(StunProcess(stunLength));

            
        }
    }

    public void StartInvul(float time)
    {
        StartCoroutine(FlashPlayer(time));
    }

    IEnumerator FlashPlayer(float time)
    {
        float timer = 0;
        bool flash = true;


        Color startColor = playerSprite.color;

        while (timer < time)
        {
            if (flash)
            {
                playerSprite.color = flashColor;
            }
            else
            {
                playerSprite.color = startColor;
            }

            flash = !flash;

            yield return new WaitForSeconds(flashRate);

            timer += flashRate;
        }

        playerSprite.color = startColor;
    }

    //Look to add option for knockback speed
    IEnumerator KnockBackprocess(float force, Vector3 incomingDir)
    {
        Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //Timer to allow a player out of a stun. 2 second max
        float knockbackTimer = 0f;

        while(b_isKnockedBack)
        {

            transform.position += incomingDir.normalized * force * Time.deltaTime;

            if (((startingPos - transform.position).magnitude >= force * 5f) || (knockbackTimer >= 2f))
            {
                b_isKnockedBack = false;
            }

            knockbackTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator KnockBackprocess(float distance, float speed, Vector3 incomingDir)
    {
        Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //Timer to allow a player out of a stun. 2 second max
        float knockbackTimer = 0f;

        while (b_isKnockedBack)
        {

            transform.position += incomingDir.normalized * distance * speed * Time.deltaTime;

            if (((startingPos - transform.position).magnitude >= distance) || (knockbackTimer >= 2f))
            {
                b_isKnockedBack = false;
            }

            knockbackTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RollProcess(Vector3 v3_rollDirection)
    {
        //Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //This should all the player to roll into an object and not get stuck
        float distanceRolled = 0f;

        //Change collision layers to not collide with the tables
        //movementCollider.
        bool previousShootState = shootingController.shootingEnabled;

        shootingController.SetGunEnabled(false);

        while (b_isRolling)
        {
            transform.position += v3_rollDirection * f_rollSpeed * Time.deltaTime;

            distanceRolled += (v3_rollDirection * f_rollSpeed * Time.deltaTime).magnitude;

            if (/*(startingPos - transform.position).magnitude*/ distanceRolled >= f_rollDistance)
            {
                b_isRolling = false;
            }
            
            yield return new WaitForEndOfFrame(); 
        }

        shootingController.SetGunEnabled(previousShootState);
    }

    IEnumerator StunProcess(float stunDuration)
    {
        float stunTimer = stunDuration;

        while(b_isStunned)
        {
            if(stunTimer <= 0)
            {
                b_isStunned = false;
            }

            stunTimer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    public void PlayerDeath()
    {
        var corpse = Instantiate(playerCorpse_pf, transform.position, transform.rotation);

        //Destroy(corpse, 2.3f);
        FindObjectOfType<RespawnManager>().OnPlayerDeath();

        healthController.ResetHealth();
    }

    public void OnCollisionEnter(Collision collision)
    {
        /* Original solution to being stuck on a wall while rolling. Might include it as a bonk mechanic
        b_isRolling = false;
        v3_controlDirection = new Vector3(0, 0, 0);
        Move(v3_controlDirection, false);
        */
    }

    //Respawn points managed here
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Respawn")
        {
            FindObjectOfType<RespawnManager>().SetNewRespawnPoint(collision.gameObject);
        }
    }

    public void SetSpawnPoint(GameObject newPoint)
    {
        FindObjectOfType<RespawnManager>().SetNewRespawnPoint(newPoint);
    }

    public void SetPlayerPosition(Transform target)
    {
        transform.SetPositionAndRotation(target.position, transform.rotation);
    }
}
