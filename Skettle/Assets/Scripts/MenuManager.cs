using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    {
        LoadGame,
        NewGame,
        None
    }

    [SerializeField] MenuState menuState;

    [SerializeField] GameManager gamemanager;

    [SerializeField] TextMeshProUGUI newGamePrompt;
    [SerializeField] GameObject confirmButton;

    bool loading = false;

    private void Start()
    {
        loading = false;
        gamemanager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            menuState = MenuState.None;
            newGamePrompt.gameObject.SetActive(false);
            confirmButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        menuState = MenuState.NewGame;
        newGamePrompt.gameObject.SetActive(true);
        newGamePrompt.text = "Override save?";
        confirmButton.SetActive(true);
    }

    public void LoadGame()
    {
        menuState = MenuState.LoadGame;
        newGamePrompt.gameObject.SetActive(true);
        newGamePrompt.text = "Load save?";
        confirmButton.SetActive(true);
    }
    public void Confirm()
    {
        if (loading)
            return;

        switch (menuState)
        {
            case MenuState.NewGame:
                loading = true;
                gamemanager.LoadGame(true);
                break;
            case MenuState.LoadGame:
                loading = true;
                gamemanager.LoadGame(false);
                break;
            default:
                break;
        }
    }

    public void QuitGame()
    {
        loading = true;
        Application.Quit();
    }
}
