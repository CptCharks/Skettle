using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneStartup : MonoBehaviour
{
    //High level managers


    //Scene objects
    public SceneStartPoint[] sceneStartPoints;
    public List<I_ProgressConditional> progressConditionals;
    public PlayerController player;

    //Add other details here for sceneStartup
    public void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        sceneStartPoints = FindObjectsOfType<SceneStartPoint>();

        progressConditionals = new List<I_ProgressConditional>();
        var pc = FindObjectsOfType<MonoBehaviour>().OfType<I_ProgressConditional>();
        foreach(I_ProgressConditional ipc in pc)
        {
            progressConditionals.Add(ipc);
        }
    }

    public void OnSceneStart(SceneStartID targetPoint, GameManager gm = null)
    {
        StartCoroutine(SceneStartRoutine(targetPoint, gm));

        FindObjectOfType<SceneSpecificsBase>()?.SceneStart();
    }

    IEnumerator SceneStartRoutine(SceneStartID targetPoint, GameManager gm = null)
    {
        if (gm != null)
        {
            foreach (I_ProgressConditional ipc in progressConditionals)
            {
                ipc.ShowOrHide(gm.progressContainer);
            }
        }

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
