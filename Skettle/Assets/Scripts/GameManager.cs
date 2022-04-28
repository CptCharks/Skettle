using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

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
        addList = new List<GameplayComponent>();
        removeList = new List<GameplayComponent>();
        GetAllGameplayObjects();

        var tempPlayer = GameObject.FindGameObjectWithTag("Player");
        if(tempPlayer != null)
        {
            player = tempPlayer.GetComponent<PlayerController>();
        }


        playerGo = GameObject.FindGameObjectWithTag("Player");

        playerHit = playerGo.GetComponentInChildren<Hittable>();

        SceneManager.LoadScene(1, LoadSceneMode.Additive);
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

    public void DumpAllGameobjects()
    {
        _GameGatheringObjects = true;
        removeList.AddRange(allCurrentGameplay);
        _GameGatheringObjects = false;
    }


    public void GetAllGameplayObjects()
    {
        _GameGatheringObjects = true;

        removeList.AddRange(allCurrentGameplay);
        addList.AddRange(FindObjectsOfType<GameplayComponent>(true));

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
                if(gc.gameObject.activeInHierarchy)
                    gc.GameplayUpdate();
            }
        }
    }


    //Demo specific functions. May not stick around for the full game

    public GameObject playerGo;
    Hittable playerHit;
    public int defaultMaxHealth = 6;

    int currentlyLoadedLevel = 1;

    [SerializeField] public int currentLevel = 2; //First level. Bedroom is 1. Player is 0

    [SerializeField] Image blackEnd;

    [SerializeField] bool retryingStage = false;

    public void FinishStage()
    {
        //Do not reset health

        StartCoroutine(FadeOutUIStuff(null));

        //Wake up in bedroom
        LoadLevel(1);

        //Setup rewards


        //Set next level to next level

        //If last level was completed, lock door to next level.
        if (retryingStage)
        {
            retryingStage = false;
        }
        else
        {
            currentLevel++;
        }
    }

    public void FailStage()
    {
        //Reset health
        playerHit.tempHit = 6;

        StartCoroutine(FadeOutUIStuff(null));

        //Wake up in bedroom
        LoadLevel(1);

        //Make sure the next level stays the same
    }

    void DisableControls()
    {
        SetEnabledPlayerControls(false);
    }

    void EnableControls()
    {
        SetEnabledPlayerControls(true);
    }

    public void LoadNextLevel()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int i)
    {
        if (i < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(AsyncLevelLoad(i, FadeIntoLevel));
    }

    public void LoadLevelRetry(int i)
    {
        retryingStage = true;
        LoadLevel(i);
    }

    public void FadeIntoLevel()
    {
        StartCoroutine(FadeInUIStuff(EnableControls));
    }

    IEnumerator AsyncLevelLoad(int i, UnityAction funcToRun)
    {
        DumpAllGameobjects();

        yield return new WaitForEndOfFrame();


        AsyncOperation deloadSceneProgress = SceneManager.UnloadSceneAsync(currentlyLoadedLevel);
        AsyncOperation sceneProgress = SceneManager.LoadSceneAsync(i,LoadSceneMode.Additive);

        while(!sceneProgress.isDone && !deloadSceneProgress.isDone)
        {
            yield return null;
        }

        currentlyLoadedLevel = i;
        
        //Figure out better way to make sure this gets everything in the scene properly
        yield return new WaitForSecondsRealtime(0.5f);
        GetAllGameplayObjects();

        funcToRun.Invoke();
    }


    IEnumerator FadeOutUIStuff(UnityAction funcToRun)
    {
        Color current = blackEnd.color;

        while (blackEnd.color.a < 1)
        {
            current.a += 0.02f; //= new Color(current.r, current.g, current.b, op);

            blackEnd.color = current;

            yield return new WaitForFixedUpdate();
        }

        funcToRun.Invoke();
    }

    IEnumerator FadeInUIStuff(UnityAction funcToRun)
    {
        Color current = blackEnd.color;

        while (blackEnd.color.a > 0)
        {
            current.a -= 0.02f; //= new Color(current.r, current.g, current.b, op);

            blackEnd.color = current;

            yield return new WaitForFixedUpdate();
        }

        funcToRun.Invoke();
    }
}
