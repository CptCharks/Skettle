using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainBossManager : MonoBehaviour
{
    public GameObject bossHealthUI;
    HealthBar bossHealth;
    [SerializeField] TrainBossPhase1 bossPhase1;
    [SerializeField] TrainBossPhase2 bossPhase2;
    [SerializeField] Transform centerOfArena;

    private void Awake()
    {
        bossHealth = bossHealthUI.GetComponent<HealthBar>();
        bossPhase1.movementDisabled = true;
    }

    public void Entered()
    {
        StartCoroutine(HealthApperance(StartBoss));
    }

    IEnumerator HealthApperance(UnityAction afterAction = null)
    {
        //Animations for the boss here



        bossHealthUI.SetActive(true);

        float temp = 0f;

        while (temp < 100)
        {
            bossHealth.UpdateHealthBarPercent(temp);
            temp++;
            yield return new WaitForSeconds(0.03f);
        }


        //Trigger the boss here

    }

    void StartBoss()
    {
        bossPhase1.movementDisabled = false;
    }


    public void OnBossPhase1Defeated()
    {
        //Flash effect to reset boss to the center of the arena
        bossPhase1.movementDisabled = true;
        bossPhase2.movementDisabled = true;

        //Remove any remaining bullets or make the player invincible during the transition
        StartCoroutine(BossPhase1DefeatRoutine());
    }

    IEnumerator BossPhase1DefeatRoutine()
    {
        bossPhase1.transform.SetPositionAndRotation(centerOfArena.position, centerOfArena.rotation);
        bossPhase2.transform.SetPositionAndRotation(centerOfArena.position, centerOfArena.rotation);

        yield return new WaitForSeconds(1f);

        bossPhase1.gameObject.SetActive(false);
        bossPhase2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        StartCoroutine(HealthApperance(StartBoss2));
    }

    void StartBoss2()
    {
        bossPhase2.movementDisabled = false;
    }


    public void OnBossPhase2Defeated()
    {
        bossPhase2.movementDisabled = true;

        StartCoroutine(BossPhase2DefeatRoutine());
    }

    IEnumerator BossPhase2DefeatRoutine()
    {
        bossPhase2.transform.SetPositionAndRotation(centerOfArena.position, centerOfArena.rotation);

        yield return new WaitForSeconds(2f);

        //Destory and transition to the town or something
    }
}
