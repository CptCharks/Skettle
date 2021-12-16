using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameplayPaused
    {
        get { return this.gameplayPaused; }
        set { this.gameplayPaused = _GameplayPaused; }
    }
    private bool _GameplayPaused;

    public bool gamePaused
    {
        get { return this.gamePaused; }
        set { this.gamePaused = _GamePaused; }
    }
    private bool _GamePaused;
    private bool _GamePauseBlocked = false;

    private bool _GameGatheringObjects = false;

    public List<GameplayComponent> allCurrentGameplay;
    List<GameplayComponent> addList;
    List<GameplayComponent> removeList;

    public PlayerController player;

    private void Start()
    {
        GetAllGameplayObjects();
        addList = new List<GameplayComponent>();
        removeList = new List<GameplayComponent>();

        var tempPlayer = GameObject.FindGameObjectWithTag("Player");
        if(tempPlayer != null)
        {
            player = tempPlayer.GetComponent<PlayerController>();
        }
    }

    public void PauseGame(bool pauseGame)
    {
        if (pauseGame && !_GamePauseBlocked)
        {
            //Pause game event
            _GamePaused = true;
        }
        else
        {
            _GamePaused = false;
        }
    }

    public void SetGameplayEnabled(bool enableGameplay)
    {
        if (!enableGameplay)
        {
            _GameplayPaused = true;
            //Pause gameplay event
        }
        else
        {
            _GameplayPaused = false;
        }
    }

    public void SetEnabledPlayerControls(bool enablePlayer)
    {
        player.b_controlsDisabled = !enablePlayer;
    }

    public void RegisterObject(GameplayComponent gc)
    {
        addList.Add(gc);
    }

    public void DeregisterObject(GameplayComponent gc)
    {
        removeList.Remove(gc);
    }

    public void GetAllGameplayObjects()
    {
        _GameGatheringObjects = true;

        allCurrentGameplay.Clear();
        allCurrentGameplay.AddRange(FindObjectsOfType<GameplayComponent>(true));

        _GameGatheringObjects = false;
    }

    public IEnumerator GetAllGameplayObjectsAsync()
    {
        _GameGatheringObjects = true;

        allCurrentGameplay.Clear();
        allCurrentGameplay.AddRange(FindObjectsOfType<GameplayComponent>());

        yield return new WaitForEndOfFrame();

        _GameGatheringObjects = false;
    }

    public void Update()
    {
        //Pre update checks
        if (_GamePaused)
            return;

        if (_GameGatheringObjects)
            return;

        if (!_GameplayPaused)
        {
            foreach(GameplayComponent gc in removeList)
            {
                allCurrentGameplay.Remove(gc);
            }

            removeList.Clear();

            foreach(GameplayComponent gc in addList)
            {
                allCurrentGameplay.Add(gc);
            }

            addList.Clear();

            foreach(GameplayComponent gc in allCurrentGameplay)
            {
                gc.GameplayUpdate();
            }
        }
    }
}
