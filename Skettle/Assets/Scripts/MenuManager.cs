using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    GameManager gamemanager;

    private void Start()
    {
        gamemanager = FindObjectOfType<GameManager>();
    }

    public void NewGame()
    {
        gamemanager.LoadGame(true);
    }

    public void LoadGame()
    {
        gamemanager.LoadGame(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
