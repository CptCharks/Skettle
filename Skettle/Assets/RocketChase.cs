using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketChase : MonoBehaviour
{
    public GameObject player_go;

    public float speed;

    public float maxTurn;

    public float timeTillExplode = 3f;

    float timer = 0f;

    public GameObject explosion_pf;

    public Bullet.BulletAlligence alligence;

    private void Awake()
    {
        player_go = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Vector3 targetDirection = Vector3.RotateTowards(transform.up, ((player_go.transform.position + new Vector3(Random.Range(-1,-1), Random.Range(-1, -1),0)) - transform.position), maxTurn, 0f);

        float turn = Vector3.SignedAngle(transform.up, targetDirection, transform.forward);



        if (Mathf.Abs(turn) > Mathf.Abs(maxTurn))
        {
            if (turn > 0)
                turn = maxTurn;
            else
                turn = -maxTurn;
        }

        transform.RotateAround(transform.position,transform.forward, turn);

        transform.position += transform.up * speed * Time.deltaTime;



        if(timer > timeTillExplode)
        {
            var explode = Instantiate(explosion_pf, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Hittable hit = collision.GetComponent<Hittable>();
        if ((hit != null) && (hit.ba_colliderAlligence != alligence))
        {
            //Debug.Log("Hit");
            var explode = Instantiate(explosion_pf, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
