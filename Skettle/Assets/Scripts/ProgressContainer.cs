using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressContainer : MonoBehaviour
{
    public PlayerData playerData;
    public ProgressionData progress;
    public SceneStartID currentSceneID;
    public GameManager gameManager;

    public bool debug = false;

    [SerializeField] GameObject gameSavedNotice;

    private void Start()
    {
        DontDestroyOnLoad(this);

        gameManager = GetComponent<GameManager>();

        if (!debug)
        {
            progress = new ProgressionData();
            playerData = new PlayerData();
        }
        //TODO: Fix the save system. Something done broke
        /*progress = SaveSystem.LoadProgress();
        if(progress == null)
        {
            //Create first time progress data
            progress = new ProgressionData();

            SaveSystem.SaveProgress(this);
        }*/
    }

    public void LoadSaveFile(int file = 0)
    {
        if (file == 0)
        {
            progress = SaveSystem.LoadProgress();
            playerData = SaveSystem.LoadPlayer();
        }
        else
        {
            progress = SaveSystem.LoadProgress(file);
            playerData = SaveSystem.LoadPlayer(file);
        }
    }

    public void SaveSaveFile(int file = 0)
    {
        if (file == 0)
        {
            SaveSystem.SaveProgress(this);
            SaveSystem.SavePlayer(gameManager.player);
        }
        else
        {
            SaveSystem.SaveProgress(this,file);
            SaveSystem.SavePlayer(gameManager.player,file);
        }

        gameSavedNotice.SetActive(true);
        StartCoroutine(timeDelay(1.5f,()=>{ gameSavedNotice.SetActive(false); }));
    }

    public void CompleteLevel(int levelNumber)
    {
        progress.currentLevel = levelNumber;
    }

    public void SaveCurrentLevel(SceneStartID sceneID)
    {
        currentSceneID = sceneID;
        progress.currentScene.sceneName = sceneID.sceneName;
        progress.currentScene.startID = sceneID.startID;
    }

    public SceneStartID LoadLevel()
    {
        currentSceneID = (SceneStartID)Resources.Load("SceneEntryPointIDs/"+progress.currentScene.sceneName+"/"+ progress.currentScene.startID, typeof(SceneStartID));
        return currentSceneID;
    }

    public void FoundTheEgg()
    {
        progress.foundTheEgg = true;
    }

    public void ClearSaveData()
    {
        playerData = new PlayerData();

        progress = new ProgressionData();
        progress.ResetSceneID();

        //SaveSystem.SaveProgress(this);
    }

    IEnumerator timeDelay(float maxTime, UnityEngine.Events.UnityAction toDo)
    {
        float timer = 0;

        while(timer < maxTime)
        {
            timer += 0.1f;

            yield return new WaitForSeconds(0.1f);
        }

        toDo?.Invoke();
    }

}
