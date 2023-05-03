using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBossPhase1 : GameplayComponent
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

    

    public enum TrainBossCowboyStates
    {
        HideBehindTower,
        Retreat,
        Invisible,
        Kick
    }

    [SerializeField] TrainBossCowboyStates currentState = TrainBossCowboyStates.HideBehindTower;

    bool nextTargetLeft = true;
    bool atTower = false;
    public int numberOfTowerHides = 0;
    public int maxNumOfTowerHides = 3;
    LuggageTower lastTowerUsed;
    LuggageTower currentTower;

    public float maxTimeBetweenTowerShots = 2f;
    public float timerBetweenTowerShots = 0f;
    public int maxNumOfTowerShots = 10;
    public int numOfTowerShots = 0;

    //This should be used when hit
    public float maxTimeRunningAway = 5f;
    public float timerRunningAway = 0f;


    bool isIdle = true;
    public float maxTimeInvisIdle = 3f;
    public float timerInvisIdle = 0f;
    public float stepTimer = 0f;
    public float timeBetweenSteps = 2f;
    float maxTimeBetweenSteps = 2f;
    public float rangeToKick = 0.1f;
    public float kickRange = 0.2f;


    public GameObject player;

    //Hard points on the character
    public Transform barrelPoint;
    public ParticleSystem dustKickup;

    //Temp variables
    SpriteRenderer spriteRenderer;

    Hittable hitController;
    HealthBar healthUI;

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

        maxTimeBetweenSteps = timeBetweenSteps;

        GoToTower();
    }

    // Update is called once per frame
    public override void GameplayUpdate()
    {
        if(movementDisabled)
        {
            enml.Halt();
        }
        else
        {
            switch(currentState)
            {
                case TrainBossCowboyStates.HideBehindTower:

                    if(!atTower)
                    {
                        if((new Vector2(targetLoc.x,targetLoc.y) - new Vector2(transform.position.x, transform.position.y)).magnitude < 0.1f)
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
                case TrainBossCowboyStates.Invisible:
                    if(isIdle)
                    {
                        timerInvisIdle += Time.deltaTime;
                        if(timerInvisIdle > maxTimeInvisIdle)
                        {
                            isIdle = false;
                            enml.speed = 0.5f;
                        }
                    }
                    else
                    {
                        InvisibleProcessing();
                    }

                    break;

            }
        }
    }

    //Call this to trigger the tower mode
    void GoToTower()
    {
        //Reset the count for tower hides. Assuming this was called to start a new cycle
        atTower = false;
        if (numberOfTowerHides > maxNumOfTowerHides)
        {
            numberOfTowerHides = 0;
        }

        int tower = Random.Range(0, 3);

        currentTower = luggageTowers[tower];

        //Just grab the next tower if we called for the same one
        if(lastTowerUsed == currentTower)
        {
            tower++;
            if(tower>3)
            {
                tower = 0;
            }

            currentTower = luggageTowers[tower];
        }

        lastTowerUsed = currentTower;

        targetLoc = currentTower.hideSpotsLR[nextTargetLeft ? 0 : 1].position;
        facingLeft = nextTargetLeft;

        enml.SetPosition(targetLoc);
        enml.speed = run;

        nextTargetLeft = !nextTargetLeft;
        numOfTowerShots = 0;
        timerBetweenTowerShots = 0f;
        numberOfTowerHides++;
        currentState = TrainBossCowboyStates.HideBehindTower;
    }

    void TowerHideProcessing()
    {
        if(timerBetweenTowerShots >= maxTimeBetweenTowerShots)
        {
            if (numOfTowerShots >= maxNumOfTowerShots)
            {
                if (numberOfTowerHides < maxNumOfTowerHides)
                {
                    GoToTower();
                }
                else
                {
                    //GoToTower();
                    GoInvisible();
                }

                //Return before firing another volley
                return;
            }


            //Fire off a coroutine for shooting
            StartCoroutine(FireHiddenVolley(25));
            timerBetweenTowerShots = 0f;
            numOfTowerShots++;
        }

        timerBetweenTowerShots += Time.deltaTime;

        
    }

    void GoInvisible()
    {
        //Disable being hurt and visuals here
        SetBossInvisible(false, 1f);

        //enml.speed = 1f; //Moved to main update transition to processing invisible attack.

        timerInvisIdle = 0f;
        timeBetweenSteps = maxTimeBetweenSteps;
        isIdle = true;
        currentState = TrainBossCowboyStates.Invisible;

        targetLoc = corners[Random.Range(0, 3)].transform.position;
        enml.speed = run;
        enml.SetPosition(targetLoc);
    }

    void SetBossInvisible(bool visibility, float fadeSpeed)
    {
        StartCoroutine(InvisibleFade(visibility, fadeSpeed));
    }

    IEnumerator InvisibleFade(bool fadeIn, float fSpeed)
    {
        var rate = 255f / fSpeed;

        float value = spriteRenderer.color.a;

        if (!fadeIn)
        {
            while (value > 0f)
            {
                var tempColor = spriteRenderer.color;

                tempColor.a -= rate * Time.deltaTime;

                spriteRenderer.color = tempColor;

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (value < 255f)
            {
                var tempColor = spriteRenderer.color;

                tempColor.a += rate * Time.deltaTime;

                spriteRenderer.color = tempColor;

                yield return new WaitForEndOfFrame();
            }
        }
    }

    void InvisibleProcessing()
    {
        targetLoc = player.transform.position;

        enml.SetPosition(targetLoc);
        enml.speed += sneakAcce * Time.deltaTime;

        stepTimer += Time.deltaTime;
        if(stepTimer > timeBetweenSteps)
        {
            stepTimer = 0f;
            //Kick up a cloud of dust at the feet and make a thud in 3D space every couple seconds speeding up with the movement
            dustKickup.Play();
        }

        timeBetweenSteps -= sneakAcce * Time.deltaTime;

        if((new Vector2(targetLoc.x, targetLoc.y) - new Vector2(transform.position.x, transform.position.y)).magnitude < rangeToKick)
        {
            movementDisabled = true;
            StartCoroutine(KickFire());
            //Probably should start a coroutine and disable movement while doing the coroutine
            //Go visible, quickly windup and kick, retreat.
        }
    }

    IEnumerator KickFire()
    {
        SetBossInvisible(true, 0.2f);


        yield return new WaitForSeconds(0.5f);

        Vector3 direction = (player.transform.position - transform.position);
        direction.z = 0f;
        direction.Normalize();

        var kick = Instantiate(kick_pf,transform.position + (direction * kickRange), transform.rotation, null);

        yield return new WaitForSeconds(1f);

        movementDisabled = false;

        GoToTower();
    }

    //Call this when boss is injured via critical point or knocked out of a phase.
    void Retreat()
    {

    }

    IEnumerator FireHiddenVolley(float numberOfShots)
    {
        float movementAmount = 0f;

        if(facingLeft)
        {
            float angle = 90;
            float amount = 10f;

            for (int i = 0; i < numberOfShots; i++)
            {
                movementAmount += amount;
                angle += amount;
                if (movementAmount > 180 || movementAmount < 0)
                {
                    amount *= -1f;
                }

                //Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                barrelPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


                //Fire a shot at the player
                var bullet = Instantiate(bullet_pf, barrelPoint.position, barrelPoint.rotation);

                //yield return new WaitFor
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            float angle = -90;
            float amount = 10f;

            for (int i = 0; i < numberOfShots; i++)
            {
                angle += amount;
                movementAmount += amount;
                //This method of detecting the angle doesn't work well. The system reverts to 270 when going past -180
                if (movementAmount > 180 || movementAmount < 0)
                {
                    amount *= -1f;
                }

                //Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                barrelPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


                //Fire a shot at the player
                var bullet = Instantiate(bullet_pf, barrelPoint.position, barrelPoint.rotation);

                //yield return new WaitFor
                yield return new WaitForSeconds(0.05f);
            }
        }
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
