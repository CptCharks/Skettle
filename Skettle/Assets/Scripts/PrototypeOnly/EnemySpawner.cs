using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        var enemy = Instantiate(enemyToSpawn);
        enemy.transform.SetPositionAndRotation(transform.position, enemy.transform.rotation);
        enemy.GetComponent<EnemyLogic_Generic>().EnableEnemy(true);
    }
}
