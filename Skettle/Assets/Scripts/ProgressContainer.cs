using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressContainer : MonoBehaviour
{
    public ProgressionData progress;
    public GameManager gameManager;

    public bool debug = false;


    private void Start()
    {
        DontDestroyOnLoad(this);

        gameManager = GetComponent<GameManager>();

        if(!debug)
            progress = new ProgressionData();

        //TODO: Fix the save system. Something done broke
        /*progress = SaveSystem.LoadProgress();
        if(progress == null)
        {
            //Create first time progress data
            progress = new ProgressionData();

            SaveSystem.SaveProgress(this);
        }*/
    }

    public void CompleteLevel(int levelNumber)
    {
        progress.lastCompletedLevel = levelNumber;
    }

    public void CurrentLevel(SceneStartID sceneID)
    {
        progress.currentScene = sceneID;
    }

    public void FoundTheEgg()
    {
        progress.foundTheEgg = true;
    }

    public bool AutoSave()
    {


        //Finished succesfully, otherwise return false and revert to previous data or warn player
        return true;
    }

    public void ClearSaveData()
    {
        progress = new ProgressionData();
        progress.ResetSceneID();

        //SaveSystem.SaveProgress(this);
    }

}
