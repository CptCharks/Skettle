using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallableTree : MonoBehaviour
{
    public GameObject topTree;
    public GameObject treeFallen;

    [SerializeField] Hittable healthController;
    [SerializeField] int health;

    [SerializeField] float fallAngle = 270;

    // Start is called before the first frame update
    void Start()
    {
        healthController.tempHit = health;
    }

    public void TriggerFall()
    {
        //Control using animation

        //Using this instead of creating animation controller for now
        StartCoroutine(TreeTimer());
    }

    IEnumerator TreeTimer()
    {
        yield return new WaitForSeconds(1f);
        HitGround();
        yield return new WaitForSeconds(0.2f);
        DisableDamageZones();
    }


    public void HitGround()
    {
        treeFallen.transform.SetPositionAndRotation(treeFallen.transform.position, Quaternion.AngleAxis(fallAngle, Vector3.forward));

        treeFallen.SetActive(true);

        topTree.SetActive(false);
    }

    public void DisableDamageZones()
    {
        var hittables = GetComponentsInChildren<Hittable>();

        foreach(Hittable ht in hittables)
        {
            ht.enabled = false;
        }
    }
}
