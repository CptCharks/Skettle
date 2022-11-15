using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkBossEntrance : MonoBehaviour
{

    public GameObject bossHealthUI;
    HealthBar bossHealth;
    [SerializeField] FireworkBossLogic bossLogic;

    private void Awake()
    {
        bossHealth = bossHealthUI.GetComponent<HealthBar>();
    }

    public void Entered()
    {
        StartCoroutine(HealthApperance());
    }

    IEnumerator HealthApperance()
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


        bossLogic.currentPhase = FireworkBossLogic.FireworkBoss_Phases.Phase1;

    }
}
