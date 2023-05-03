using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OnLevelFinished : MonoBehaviour
{
    public PlayableAsset onEndCutscene;
    public PlayableDirector director;

    SceneStartID townSquareEntry;
    GameManager gameManager;

    private void Awake()
    {
        townSquareEntry = (SceneStartID)Resources.Load("SceneEntryPointIDs/TownSquare/TownSquare_ToCrossroads");
        gameManager = FindObjectOfType<GameManager>();

        director = GetComponent<PlayableDirector>();
        if(director != null)
        {
            director.stopped += OnTimelineFinished;
        }
    }

    public void EndLevel()
    {
        if(onEndCutscene != null && director != null)
        {
            //TODO: Figure out if you can just call the cutScene manager from here and have it work
            director.Play(onEndCutscene);
        }
        else
        {
            GoToTown();
        }
    }


    public void OnTimelineFinished(PlayableDirector callingDirector)
    {
        GoToTown();
    }

    void GoToTown()
    {
        gameManager.progressContainer.progress.currentLevel++;
        gameManager.progressContainer.progress.isMorning = false;
        //v This might be useless since it saves during level loads
        gameManager.SaveGame();
        gameManager.LoadLevel(townSquareEntry);
    }
}
