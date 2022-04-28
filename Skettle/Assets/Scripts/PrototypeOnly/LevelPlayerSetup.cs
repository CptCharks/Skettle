using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerSetup : MonoBehaviour
{
    GameObject player;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.SetPositionAndRotation(transform.position,player.transform.rotation);
    }

}
