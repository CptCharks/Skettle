using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBarrage : MonoBehaviour
{
    public GameObject rocketStrike_pf;

    public float numberOfRockets;

    public float randomness = 1f;
    public float perRocket = 0.3f;

    public bool isExtremelyRandom = true;

    Transform[] rocketPoints;

    private void Start()
    {
        rocketPoints = GetComponentsInChildren<Transform>();


        if (isExtremelyRandom)
        {
            StartCoroutine(SpawnRockets());
        }
        else
        {
            StartCoroutine(SpawnTidyRockets());
        }
    }

    IEnumerator SpawnRockets()
    {
        yield return new WaitForEndOfFrame();

        int leftOrRight = 1;
        int upOrDown = 1;

        for(int i = 0; i < numberOfRockets; i++)
        {
            var newStrike = Instantiate(rocketStrike_pf, transform.position + new Vector3(Random.Range(1f, 1f + randomness + perRocket*numberOfRockets) * leftOrRight, Random.Range(1f, 1f + randomness * numberOfRockets),0f) * upOrDown,transform.rotation);

            leftOrRight = leftOrRight * (int)Mathf.Sign(Random.Range(-1f, 1f));
            upOrDown = upOrDown * (int)Mathf.Sign(Random.Range(-1f, 1f));

            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
        }

        Destroy(gameObject, 6f);
    }

    IEnumerator SpawnTidyRockets()
    {
        var rng = new System.Random();
        rng.Shuffle(rocketPoints);

        int num = 0;

        for (int i = 0; i < numberOfRockets; i++)
        {
            if(num >= numberOfRockets)
            {
                num = 0;
            }

            var newStrike = Instantiate(rocketStrike_pf, rocketPoints[num].position, transform.rotation);

            num++;

            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
        }
    }
}
