using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBossPhase2 : GameplayComponent
{
    Animator anim;

    public bool movementDisabled = false;

    bool facingLeft = true;


    [SerializeField] private LuggageTower[] luggageTowers;
    [SerializeField] private BossRoomCorner[] corners;

    public float run;
    public float sneak;
    public float sneakAcce = 0.1f;
    EnemyNavMeshLogic enml;
    Vector3 targetLoc;

    public GameObject bullet_pf;

    //Using an explosion atm as a test
    public GameObject kick_pf;

    //Using a rocket barrage as a test object
    public GameObject fallingLuggage_pf;

    //Tower variables
    bool atTower = false;
    LuggageTower lastTowerUsed;
    LuggageTower currentTower;
    [SerializeField] float waitToPushTimer = 0f;
    //The animation of pushing should be synced to this somehow
    [SerializeField] float maxWaitToPushTime = 1f;
    [SerializeField] float gloatTimer = 0f;
    [SerializeField] float maxGloatTime = 2f;
    [SerializeField] float timeBetweenLuggageTower = 3f;

    //Running variables
    BossRoomCorner currentCorner;
    BossRoomCorner lastCornerUsed;
    [SerializeField] Vector2 changeDirectionTimeVariance = new Vector2(0.6f,2f);
    [SerializeField] float changeDirectionTimer = 0f;
    [SerializeField] float runAwayTimer = 0f;
    [SerializeField] float maxRunAwayTime = 10f;
    [SerializeField] float luggageTimer = 0f;
    [SerializeField] float timeBetweenLuggage = 2f;
    
    bool gloating = false;

    public GameObject player;

    //Hard points on the character
    public Transform barrelPoint;
    public ParticleSystem dustKickup;

    //Temp variables
    SpriteRenderer spriteRenderer;

    Hittable hitController;
    HealthBar healthUI; 

    public enum TrainBossConductorStates
    {
        RunAway,
        HideBehindTower,
        Retreat
    }

    [SerializeField] TrainBossConductorStates currentState = TrainBossConductorStates.RunAway;


    void Start()
    {
        hitController = GetComponentInChildren<Hittable>();
        healthUI = FindObjectOfType<HealthBar>();

        anim = GetComponent<Animator>();
        luggageTowers = FindObjectsOfType<LuggageTower>();
        corners = FindObjectsOfType<BossRoomCorner>();
        enml = GetComponent<EnemyNavMeshLogic>();
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();


        GetRunning();

    }

    public override void GameplayUpdate()
    {
        if (movementDisabled)
        {
            enml.Halt();
        }
        else
        {
            switch (currentState)
            {
                case TrainBossConductorStates.HideBehindTower:

                    if (!atTower)
                    {
                        if ((new Vector2(targetLoc.x, targetLoc.y) - new Vector2(transform.position.x, transform.position.y)).magnitude < 0.1f)
                        {
                            atTower = true;
                            //Insert thing here that reveals a week spot for the player to hit
                        }
                    }
                    else
                    {
                        TowerHideProcessing();
                    }

                    break;
                case TrainBossConductorStates.RunAway:
                    ProcessRunning();
                    break;
            }
        }
    }

    void GetRunning()
    {
        runAwayTimer = 0f;
        changeDirectionTimer = 0f;
        luggageTimer = 0f;

        currentCorner = corners[Random.Range(0, 3)];

        currentState = TrainBossConductorStates.RunAway;
    }

    void ProcessRunning()
    {
        runAwayTimer += Time.deltaTime;
        luggageTimer += Time.deltaTime;
        changeDirectionTimer += Time.deltaTime;

        //TODO: Add movement management here

        if (luggageTimer > timeBetweenLuggage)
        {
            var barrage = Instantiate(fallingLuggage_pf,player.transform.position,player.transform.rotation,null);
            luggageTimer = 0f;
        }

        if(runAwayTimer > maxRunAwayTime)
        {
            GoToTower();
        }
    }

    void GoToTower()
    {
        //Reset the count for tower hides. Assuming this was called to start a new cycle
        atTower = false;

        int tower = Random.Range(0, 3);

        currentTower = luggageTowers[tower];

        //Just grab the next tower if we can't push over the target one
        if (!currentTower.canPush)
        {
            tower++;
            if (tower > 3)
            {
                tower = 0;
            }

            currentTower = luggageTowers[tower];
        }

        targetLoc = currentTower.pushSpot.position;

        enml.SetPosition(targetLoc);
        enml.speed = run;

        waitToPushTimer = 0f;
        gloatTimer = 0f;
        luggageTimer = 0f;

        currentState = TrainBossConductorStates.HideBehindTower;
    }

    void TowerHideProcessing()
    {
        luggageTimer += Time.deltaTime;

        if (luggageTimer > timeBetweenLuggage)
        {
            var barrage = Instantiate(fallingLuggage_pf, player.transform.position, player.transform.rotation, null);
            luggageTimer = 0f;
        }


        if (luggageTimer > timeBetweenLuggageTower)
        {
            var barrage = Instantiate(fallingLuggage_pf, player.transform.position, player.transform.rotation, null);
            luggageTimer = 0f;
        }


        if (waitToPushTimer < maxWaitToPushTime)
        {
            waitToPushTimer += Time.deltaTime;
            return;
        }

        //Push the tower
        if(!gloating)
        {
            //Safety check in case we messed up when choosing the tower
            if (currentTower.canPush)
            {
                currentTower.PushTower();
            }
            //Start gloat animation
            //Probably make the boss vulnerable to bonus damage here
        }

        if(gloatTimer > maxGloatTime)
        {
            GetRunning();
        }

        gloatTimer += Time.deltaTime;
    }

    public void Dead()
    {
        //This function in the firework boss spawns a corpse
        movementDisabled = true;
        FindObjectOfType<GameManager>().DeregisterObject(this);
        gameObject.SetActive(false);
    }

    public void UpdateHealth()
    {
        healthUI.UpdateHealthValue(hitController.tempHit);
    }

}
