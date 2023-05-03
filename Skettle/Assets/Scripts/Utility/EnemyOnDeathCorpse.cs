using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyLogic))]
public class EnemyOnDeathCorpse : MonoBehaviour
{
    public GameObject pf_corpse;

    EnemyLogic enemyParent;

    private void Awake()
    {
        enemyParent = GetComponent<EnemyLogic>();
    }


    private void OnEnable()
    {
        enemyParent.onDeath += SpawnCorpse;
    }

    private void OnDisable()
    {
        enemyParent.onDeath -= SpawnCorpse;
    }

    public void SpawnCorpse()
    {
        if (pf_corpse != null)
        {
            Instantiate(pf_corpse, null);
        }
    }
}
