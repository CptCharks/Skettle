using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAccess : MonoBehaviour
{
	public GameManager gameManager;
	
	
	public void Awake()
	{
		gameManager = FindObjectOfType<GameManager>();
	}
	
    public void PauseGame(bool pauseGame)
	{
		gameManager.PauseGame(pauseGame);
	}
	
	
	public void SetGameplayEnabled(bool enableGameplay)
    {
		gameManager.SetGameplayEnabled(enableGameplay);
	}
	
	public void SetEnabledPlayerControls(bool enablePlayer)
    {
		gameManager.SetEnabledPlayerControls(enablePlayer);
    }

    public void RegisterObject(GameplayComponent gc)
    {
		gameManager.RegisterObject(gc);
    }

    public void DeregisterObject(GameplayComponent gc)
    {
		gameManager.DeregisterObject(gc);
    }

    public void GetAllGameplayObjects()
    {
		gameManager.GetAllGameplayObjects();
    }

    /*public IEnumerator GetAllGameplayObjectsAsync()
    {

    }*/
}
