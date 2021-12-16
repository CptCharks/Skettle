using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoypeSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pauseMenu;
    [SerializeField] bool isPaused = false;

    int currentScene;

    void Start()
    {
        pauseMenu.SetActive(false);
        currentScene = 1;
        SceneManager.LoadScene(1,LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void LoadLevel(int level)
    {
        PauseUnpause();
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = level;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
