using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject endOfLevel;
    [SerializeField] bool isPaused = false;

    int currentScene;

    void Start()
    {
        pauseMenu.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (endOfLevel.activeInHierarchy)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused?0:1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
