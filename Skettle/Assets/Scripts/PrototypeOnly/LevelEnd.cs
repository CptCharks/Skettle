using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class LevelEnd : MonoBehaviour
{
    public GameObject[] remainingEnemies;

    public UnityEvent allEnemiesDestroyed = new UnityEvent();
    public UnityEvent notAllEnemiesDestroyed = new UnityEvent();

    public GameObject endLevelMenu;
    GameObject parentCanvas;

    public TextMeshProUGUI numberOfEnemiesLeft;

    int enemiesRemaining = 0;

    GameManager gamemanager;

    private void Awake()
    {
        remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        gamemanager = FindObjectOfType<GameManager>();

        parentCanvas = GameObject.Find("PlayerMainCanvas");
        endLevelMenu = StaticHelperFunctions.FindUIObject(parentCanvas,"EndPanel");
        numberOfEnemiesLeft = StaticHelperFunctions.FindUIObject(endLevelMenu,"EnemyRemainingText").GetComponent<TextMeshProUGUI>();
        endLevelMenu.SetActive(false);
    }



    public void ExitingLevel()
    {
        Debug.Log("Trying to exit level");

        foreach(GameObject go in remainingEnemies)
        {
            if(go.activeSelf)
            {
                //Warn Player
                //notAllEnemiesDestroyed.Invoke();

                enemiesRemaining++;
            }
            
        }

        //End level
        //allEnemiesDestroyed.Invoke();
        numberOfEnemiesLeft.text = "Enemies Remaining:" + enemiesRemaining.ToString("#0");

        gamemanager.PauseGame(true);
        endLevelMenu.SetActive(true);
    }
}
