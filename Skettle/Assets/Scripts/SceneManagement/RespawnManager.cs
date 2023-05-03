using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RespawnManager : MonoBehaviour
{
    public UnityEvent onPlayerDeath;

    [SerializeField] GameObject currentRespawnPoint;
    [SerializeField] PlayerController player;

    public GameManager gameManager;

    bool callback = false;

    DemoEnd demoEndManager;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
       
        gameManager = FindObjectOfType<GameManager>();

        demoEndManager = FindObjectOfType<DemoEnd>();
    }

    public void SetNewRespawnPoint(GameObject point_go)
    {
        currentRespawnPoint = point_go;
    }

    public void OnPlayerDeath()
    {
        /*if(demoEndManager == null)
        {
            demoEndManager = FindObjectOfType<DemoEnd>();
        }

        demoEndManager.AddDeath();*/

        if(currentRespawnPoint == null)
        {
            currentRespawnPoint = GameObject.FindWithTag("LevelStart");
        }

        player.gameObject.SetActive(false);
        gameManager.SetEnabledPlayerControls(false);
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        gameManager.FadeOutUI(CallBackFuncDone);

        yield return new WaitUntil(() => callback);
        callback = false;

        player.SetPlayerPosition(currentRespawnPoint.transform);
        player.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        gameManager.FadeInUI(CallBackFuncDone);
        yield return new WaitUntil(() => callback);
        callback = false;

        gameManager.SetEnabledPlayerControls(true);
    }

    public void CallBackFuncDone()
    {
        callback = true;
    }

}
