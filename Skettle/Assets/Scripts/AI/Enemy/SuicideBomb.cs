using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class SuicideBomb : MonoBehaviour
{
	public VisionController visionController;
	public EnemyNavMeshLogic navMeshController;
	public bool targetAcquired;
	public bool visionActivated;
	
	public GameObject explosion_pf;
	
	public float timeTillExplode = 5f;
	float timerTillBoom = 0f;

    [SerializeField] Vector3 decisionZone = Vector3.zero;
    [SerializeField] Vector3 explodeZone = Vector3.zero;
	bool setToGo = false;
    bool fuseLit = false;

    [SerializeField] GameObject botSprite;
    Animator anim;
    [SerializeField] ParticleSystem fuseLitParticles;

	GameObject playerRef;
    AudioSource audioSource;
    public AudioClip jumpTowardsClip;
    public AudioClip explosionClip;
    public AudioClip fuseClip;
    public AudioMixerGroup explosionMixer;

	void Awake()
	{
		navMeshController = GetComponent<EnemyNavMeshLogic>();
        visionController = GetComponent<VisionController>();
        audioSource = GetComponent<AudioSource>();
        anim = botSprite.GetComponent<Animator>();

    }
	
    // Start is called before the first frame update
    void Start()
    {
		playerRef = visionController.go_player;
    }

    // Update is called once per frame
    void Update()
    {
		if(visionController.b_canSeePlayer && visionActivated)
		{
			targetAcquired = true;
            LightFuse();
        }
		
        if(targetAcquired)
		{
			timerTillBoom += Time.deltaTime;
			
			
			
			if(timerTillBoom < timeTillExplode)
			{
				navMeshController.SetPosition(playerRef.transform.position);
			}
            else if (((timerTillBoom >= timeTillExplode) || (visionController.f_distanceToPlayer < 0.8f)) && !setToGo)
            {
                //TODO: Change the bot to make a mad dash for the player. If they don't make it, have them trip towards the player and blow up after some time. If they make it, trip and explode.
                //TODO: This line makes the bot go wander off to die. Not good. I just need it to make a sprint then basically trip towards the player when trying to explode.
                decisionZone = transform.position;
                explodeZone = ((playerRef.transform.position - transform.position).normalized*1.2f) + transform.position;
                navMeshController.SetPosition(explodeZone);
                navMeshController.UpdateSpeed(2f);
                setToGo = true;
                AudioFunctions.PlayClipAtPoint(jumpTowardsClip, transform.position, 2f, audioSource.outputAudioMixerGroup);
            }

            if (setToGo)
			{
				if((explodeZone - transform.position).magnitude < 0.3f)
				{
					Explode();

					Destroy(gameObject);
				}
			}
            else
            {
                float xValue = (playerRef.transform.position - transform.position).x;
                if(xValue > 0)
                {
                    Vector3 newScale = botSprite.transform.localScale;
                    newScale.x = -1;
                    botSprite.transform.localScale = newScale;
                }
                else
                {
                    Vector3 newScale = botSprite.transform.localScale;
                    newScale.x = 1;
                    botSprite.transform.localScale = newScale;
                }
            }
			
		}
    }
	
    void LightFuse()
    {
        if (!fuseLit)
        {
            audioSource.Play();
            fuseLit = true;
            anim?.SetTrigger("Lit");
            fuseLitParticles?.Play();
        }
    }

	public void Explode()
	{
		Instantiate(explosion_pf, transform.position, transform.rotation, transform.parent);
        //AudioFunctions.PlayClipAtPoint(explosionClip, transform.position, 1f, explosionMixer);
    }



    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(decisionZone, 0.5f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explodeZone, 0.5f);
    }

}
