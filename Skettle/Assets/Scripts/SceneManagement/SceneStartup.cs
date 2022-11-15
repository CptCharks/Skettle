using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartup : MonoBehaviour
{
    //High level managers


    //Scene objects
    public SceneStartPoint[] sceneStartPoints;
    public PlayerController player;

    //Add other details here for sceneStartup
    public void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        sceneStartPoints = FindObjectsOfType<SceneStartPoint>();
    }

    public void OnSceneStart(SceneStartID targetPoint)
    {
        StartCoroutine(SceneStartRoutine(targetPoint));

        FindObjectOfType<SceneSpecificsBase>()?.SceneStart();
    }

    IEnumerator SceneStartRoutine(SceneStartID targetPoint)
    {

        foreach (SceneStartPoint ssp in sceneStartPoints)
        {
            ssp.alreadyTriggered = true;
        }

        foreach (SceneStartPoint ssp in sceneStartPoints)
        {
            if ((ssp.sceneStartID != null) && (ssp.sceneStartID.startID == targetPoint.startID))
            {
                Debug.Log("Tried to set player position with " + player.name + " and " + ssp.playerExitPoint.name);
                
                yield return new WaitUntil(() => player = FindObjectOfType<PlayerController>());

                player.SetPlayerPosition(ssp.playerExitPoint);
            }

            ssp.alreadyTriggered = false;
        }
    }


}
