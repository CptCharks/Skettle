using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameplayComponent
{
    public enum BulletAlligence
    {
        Friendly,
        Dangerous,
        Neutral
    }

    public BulletAlligence alligence;
    public float damage;
    public float speed;
    private float f_distanceTillDestroyed = 100f;
    private float f_currentDistance = 0f;

    /*
    private void OnEnable()
    {
        FindObjectOfType<GameManager>().RegisterObject(this);
    }

    private void OnDisable()
    {
        FindObjectOfType<GameManager>().DeregisterObject(this);
    }

    private void OnDestroy()
    {
        //Currently having issue when object is destroyed. It gets added to the queue but then the bullet script is destroyed. We can probably disable the bullet then destroy it once the item is removed
        FindObjectOfType<GameManager>().DeregisterObject(this);
    }
    */
    //This is here until we can get the register/deregister issue solved
    public override void GameplayUpdate()
    {
    
    }
    public void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        f_currentDistance += (transform.right * speed * Time.deltaTime).magnitude;
        if(f_currentDistance >= f_distanceTillDestroyed)
        {
            Destroy(gameObject);
        }
    }
    
    public void SetDistance(float distance)
    {
        f_distanceTillDestroyed = distance;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Passed over something");

        Hittable hit = other.GetComponent<Hittable>();
        if((hit != null) && (hit.ba_colliderAlligence != alligence))
        {
            Debug.Log("Hit");
            hit.Hit();
            Destroy(gameObject);
        }

        
    }
}
