using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySpawnPoint : MonoBehaviour
{
    public GameObject enemyPrefab;

    public virtual void Awake()
    {
        SpawnEnemyType();
    }

    public abstract void SpawnEnemyType();
}
