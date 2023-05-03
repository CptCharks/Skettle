using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnEnemyDeath : MonoBehaviour
{
    public List<EnemyLogic> enemies;

    [SerializeField] int numberDead = 0;
    public int numberNeeded = 2;
    bool triggeredOnce = false;

    public UnityEvent onEnemiesDead;

    void OnEnable()
    {
        foreach(EnemyLogic el in enemies)
        {
            el.onDeath += EnemyDied;
        }
    }

    void OnDisable()
    {
        foreach (EnemyLogic el in enemies)
        {
            el.onDeath -= EnemyDied;
        }
    }


    public void EnemyDied()
    {
        numberDead++;
        if(!triggeredOnce && (numberDead >= numberNeeded))
        {
            triggeredOnce = true;
            onEnemiesDead.Invoke();
        }
    }

}
