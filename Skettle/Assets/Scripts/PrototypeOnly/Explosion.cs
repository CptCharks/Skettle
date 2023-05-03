using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float timeActive = 1f;

    public Bullet.BulletAlligence alligence;

    [SerializeField] AudioClip explosionClip;
    [SerializeField] UnityEngine.Audio.AudioMixerGroup explosionMixer;
    [SerializeField] float explosionLoudness = 0.8f;
    float audioVariance = 0.1f;


    void Start()
	{
		Destroy(gameObject, timeActive);
        AudioFunctions.PlayClipAtPoint(explosionClip, transform.position, 1f + Random.Range(-audioVariance, audioVariance), explosionLoudness, explosionMixer);
	}
	
	public void OnTriggerEnter2D(Collider2D other)
	{
		var hittable = other.GetComponent<Hittable>();
		if(hittable != null)
		{
            if (hittable.ba_colliderAlligence != alligence)
            {
                hittable.Hit();
            }
		}
	}
}
