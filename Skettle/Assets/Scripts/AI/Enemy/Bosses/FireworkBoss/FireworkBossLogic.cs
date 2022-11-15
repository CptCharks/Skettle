using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkBossLogic : GameplayComponent
{
    //Two phases
    public enum FireworkBoss_Phases
    {
        Idle,
        Phase1,
        Phase2
    }

    public enum FireworkBoss_Subphases
    {
        P1_Circle,
        P1_Kneel,
        P1_Hide,
        P1_Shower,
        P2_Charging,
        P2_Swinging,
        P2_Recovery
    }

    public FireworkBoss_Phases currentPhase;
    public FireworkBoss_Subphases currentSubPhase
    {
        get { return _currentSubPhase; }
        set {ChangeState(value); }
    }

    private FireworkBoss_Subphases _currentSubPhase;

    VisionController vision;
    EnemyNavMeshLogic navMeshLogic;

    public GameObject player;

    new SpriteRenderer renderer;

    bool goingLeft = true;
    float circleTimer = 0;
    Vector2 circleTarget;
    [SerializeField] float circleDistance;

    [SerializeField] float mortarShotTime = 5f;
    int mortarShots = 0;
    [SerializeField] float timeBetweenShots = 1.5f;
    float nextShotTime = 0f;

    Animator anim;

    [SerializeField] Transform pistolPoint;
    //[SerializeField] float distanceFromBoss = 0.2f;

    [SerializeField] GameObject bullet_pf;

    [SerializeField] ParticleSystem smoke;

    [SerializeField] float barrageDisplace = 3f;
    [SerializeField] GameObject barrage_pf;

    [SerializeField] Transform[] corners;
    [SerializeField] Transform cornerTarget;

    [SerializeField] int health_Phase2;

    public float chargeTimeLimit = 4f;
    float chargeTimer = 0f;

    [SerializeField] GameObject chaseRocket_pf;
    [SerializeField] int rockets = 4;


    public float timeToRecover = 2f;
    float recoverTimer = 0f;

    [SerializeField] GameObject bossCorpse_pf;


    Hittable hitController;
    [SerializeField] HealthBar healthUI;

    //First phase
    //Roam around
    //Rotate in a circle around the player, left, right repeating.
    //After rotating to one side for a bit, shooting at the player with small side arm, stoop down and launch the fireworks
    //Go the other way, repeat
    //After two cylces, run to an opposite corner of the arena and rain fireworks across the arean

    //Second phase
    //Charges the player with the firework totem and side arm.
    //Once close or after a certain amount of time, swings totem firing off a volley towards the player.
    //Repeats.
    private void Awake()
    {
        vision = GetComponent<VisionController>();
        navMeshLogic = GetComponent<EnemyNavMeshLogic>();
        hitController = GetComponentInChildren<Hittable>();

        player = GameObject.FindGameObjectWithTag("Player");

        currentPhase = FireworkBoss_Phases.Idle;
        currentSubPhase = FireworkBoss_Subphases.P1_Circle;

        renderer = GetComponent<SpriteRenderer>();

        

        nextShotTime = timeBetweenShots;

        circleTarget = Vector2.zero;

        anim = GetComponent<Animator>();
    }

    public void Start()
    {
        health_Phase2 = (int)(hitController.tempHit / 3);

        navMeshLogic.SetPosition(transform.position);
    }

    public void ActivateBoss()
    {
        //Animations and name
        currentPhase = FireworkBoss_Phases.Phase1;
    }

    public override void GameplayUpdate()
    {
        if(currentPhase == FireworkBoss_Phases.Phase1)
        {
            switch(_currentSubPhase)
            {
                case FireworkBoss_Subphases.P1_Circle:
                    CircleProcesses();
                    break;
                case FireworkBoss_Subphases.P1_Kneel:
                    break;
                case FireworkBoss_Subphases.P1_Hide:
                    RainOfFire();
                    break;
                case FireworkBoss_Subphases.P1_Shower:
                    
                    break;
            }
        }
        else
        {
            switch (_currentSubPhase)
            {
                case FireworkBoss_Subphases.P2_Charging:
                    Charge();
                    break;
                case FireworkBoss_Subphases.P2_Swinging:
                   
                    break;
                case FireworkBoss_Subphases.P2_Recovery:
                    Recovery();
                    break;
            }
        }

        if(hitController.tempHit < health_Phase2)
        {
            StartCoroutine(TransitionToPhase2());
            currentPhase = FireworkBoss_Phases.Idle;
        }

        AnimationControl();
    }

    public void ChangeState(FireworkBoss_Subphases newState)
    {
        _currentSubPhase = newState;

        switch (_currentSubPhase)
        {
            case FireworkBoss_Subphases.P1_Circle:
                goingLeft = !goingLeft;
                //currentPhase = FireworkBoss_Phases.Phase1;
                break;
            case FireworkBoss_Subphases.P1_Kneel:
                MortarShot();
                //currentPhase = FireworkBoss_Phases.Phase1;
                break;
            case FireworkBoss_Subphases.P1_Hide:
                RunToCorner();
                //currentPhase = FireworkBoss_Phases.Phase1;
                break;
            case FireworkBoss_Subphases.P1_Shower:
                StartCoroutine(RainOfFireCR());
                //currentPhase = FireworkBoss_Phases.Phase1;
                break;
            case FireworkBoss_Subphases.P2_Charging:
                currentPhase = FireworkBoss_Phases.Phase2;
                break;
            case FireworkBoss_Subphases.P2_Recovery:
                currentPhase = FireworkBoss_Phases.Phase2;
                break;
            case FireworkBoss_Subphases.P2_Swinging:
                StartCoroutine(SwingAtPlayer());
                currentPhase = FireworkBoss_Phases.Phase2;
                break;
        }
    }

    public void AnimationControl()
    {

        //Use movement or use gun direction

        //Need to change this to scaling the gameObject

        if((player.transform.position - transform.position).x > 0)
        {
            //renderer.flipX = true;
        }
        else
        {
            //renderer.flipX = false;
        }


    }

    IEnumerator TransitionToPhase2()
    {
        yield return new WaitForFixedUpdate();

        //Put animations for getting enraged


        currentSubPhase = FireworkBoss_Subphases.P2_Charging;

        Debug.Log("Hit phase 2");
    }


    public void CircleProcesses()
    {
        if(mortarShots > 4)
        {
            mortarShots = 0;
            currentSubPhase = FireworkBoss_Subphases.P1_Hide;
        }

        if(circleTarget == Vector2.zero || circleTimer < 0)
        {
            if(goingLeft)
            {
                circleTarget = player.transform.position + new Vector3(-circleDistance,0f,0f);
            }
            else
            {
                circleTarget = player.transform.position + new Vector3(circleDistance, 0f, 0f);
            }

            navMeshLogic.SetPosition(circleTarget);
        }

        //If circle target is too close to the player, consider moving the other way
        //TODO

        if(circleTimer > mortarShotTime)
        {
            navMeshLogic.SetPosition(transform.position);

            currentSubPhase = FireworkBoss_Subphases.P1_Kneel;
            circleTimer = -0.1f;
            nextShotTime = timeBetweenShots;

            mortarShots++;

            return;
        }


        if(circleTimer > nextShotTime)
        {

            Vector3 direction = player.transform.position - pistolPoint.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pistolPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            //Fire a shot at the player
            var bullet = Instantiate(bullet_pf, pistolPoint.position, pistolPoint.rotation);

            nextShotTime = circleTimer + timeBetweenShots + Random.Range(0,1f);
        }

        circleTimer += Time.deltaTime;
    }

    public void MortarShot()
    {
        //Make it a coroutine

        //Make a wave of bullets around the boss while he's launching like it's the sparks

        //Spawn a game object on the player that warns them of the incoming fireworks then blow up after the animation
        StartCoroutine(MortarShotCR());
    }

    IEnumerator MortarShotCR()
    {
        //Start the animation for launching here

        anim.SetTrigger("Kneel");

        yield return new WaitForSeconds(0.8f);

        smoke.Play();

        Vector3 direction = player.transform.position - pistolPoint.position;

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 25f;

        for (int i = 0; i < 52; i++)
        {
            

            angle += 10f;

              //Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pistolPoint.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            //Fire a shot at the player
            var bullet = Instantiate(bullet_pf, pistolPoint.position, pistolPoint.rotation);

            //yield return new WaitFor
            yield return new WaitForSeconds(0.05f);
        }

        var barrage = Instantiate(barrage_pf, player.transform.position, player.transform.rotation);
        //Destroy(barrage, 4f);

        yield return new WaitForSeconds(0.5f);

        anim.SetTrigger("Stand");

        _currentSubPhase = FireworkBoss_Subphases.P1_Circle;
    }

    public void RunToCorner()
    {
        Debug.Log("Running to corner");

        int target = 0;
        float length = 0;

        for(int i = 0; i < 4; i++)
        {
            float dist = (player.transform.position - corners[i].position).magnitude;
            if(dist > length)
            {
                length = dist;
                target = i;
            }
        }

        cornerTarget = corners[target];

        navMeshLogic.SetPosition(cornerTarget.position);

        Debug.Log(cornerTarget.position);

        navMeshLogic.UpdateSpeed(2.5f);
    }

    public void RainOfFire()
    {
        //Debug.Log("Checking for corner");

        //Run to a corner away from the player
        navMeshLogic.SetPosition(cornerTarget.position);

        //Once there
        if (Mathf.Abs((cornerTarget.position - transform.position).magnitude) < 0.2f)
        {
            navMeshLogic.UpdateSpeed(1.2f);
            currentSubPhase = FireworkBoss_Subphases.P1_Shower;
        }
    }

    IEnumerator RainOfFireCR()
    {
        anim.SetTrigger("Kneel");

        yield return new WaitForSeconds(0.8f);

        smoke.Play();

        yield return new WaitForSeconds(2f);

        Vector3[] displacements = new Vector3[4];
        displacements[0] = new Vector3(barrageDisplace,0,0);
        displacements[1] = new Vector3(-barrageDisplace,0,0);
        displacements[2] = new Vector3(0,barrageDisplace,0);
        displacements[3] = new Vector3(0, -barrageDisplace, 0);

        var firstBarrage = Instantiate(barrage_pf, player.transform.position, player.transform.rotation);

        for (int i = 0; i < 4; i++)
        {
            var barrage = Instantiate(barrage_pf, player.transform.position + displacements[i], player.transform.rotation);
        }

        yield return new WaitForSeconds(2f);

        anim.SetTrigger("Stand");

        currentSubPhase = FireworkBoss_Subphases.P1_Circle;
    }

    public void Charge()
    {
        if(chargeTimer > chargeTimeLimit)
        {
            chargeTimer = 0f;
            currentSubPhase = FireworkBoss_Subphases.P2_Swinging;
            return;
        }

        navMeshLogic.UpdateSpeed(1.5f);
        navMeshLogic.SetPosition(player.transform.position);

        chargeTimer += Time.deltaTime;
    }

    IEnumerator SwingAtPlayer()
    {
        //animation to swing at player
        anim.SetTrigger("Swing");

        yield return new WaitForSeconds(0.3f);

        for(int i = 0; i < rockets; i++)
        {
            var rocket = Instantiate(chaseRocket_pf, pistolPoint.position, pistolPoint.rotation);
            yield return new WaitForSeconds(0.5f);
        }

        //Spawn damaging object and use that to spawn rockets

        currentSubPhase = FireworkBoss_Subphases.P2_Recovery;
    }

    public void Recovery()
    {
        if(recoverTimer > timeToRecover)
        {
            anim.SetTrigger("Stand");

            recoverTimer = 0f;
            currentSubPhase = FireworkBoss_Subphases.P2_Charging;
            return;
        }

        recoverTimer += Time.deltaTime;

        //Basically a timer and set the animation
    }

    public void Dead()
    {
        Instantiate(bossCorpse_pf, transform.position, transform.rotation);
        FindObjectOfType<GameManager>().DeregisterObject(this);
        gameObject.SetActive(false);
    }

    public void UpdateHealth()
    {
        healthUI.UpdateHealthValue(hitController.tempHit);
    }
}
