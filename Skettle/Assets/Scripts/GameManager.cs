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

    public ProgressContainer progressContainer;
    public MissionOrderInfo missionOrderInfo;


    private void Start()
    {
        DontDestroyOnLoad(this);

        progressContainer = GetComponent<ProgressContainer>();

        missionOrderInfo = (MissionOrderInfo)Resources.Load("MissionOrder");

        addList = new List<GameplayComponent>();
        removeList = new List<GameplayComponent>();
        GetAllGameplayObjects();

        //Get player variables. Might want to hard set the PlayerController and PlayerGO here too

        /*var tempPlayer = GameObject.FindGameObjectWithTag("Player");
        if(tempPlayer != null)
        {
            player = tempPlayer.GetComponent<PlayerController>();
        }

        playerGo = GameObject.FindGameObjectWithTag("Player");
        */
        playerHit = playerGo.GetComponentInChildren<Hittable>();


        if (SceneManager.sceneCount >= 2)
        {
            if (startUp == null)
            {
                startUp = FindObjectOfType<SceneStartup>();
            }

            if (startUp != null)
            {
                //Tried to have it wait until it finished
                startUp.Awake();
                if(currentTargetID != null)
                    startUp.OnSceneStart(currentTargetID);
                //progressContainer.CurrentLevel(sceneStartID.sceneName);
            }


        }
        else
        {

            //Prevent the game from being paused on the main menu
            _GamePauseBlocked = true;

            //Prevent player from accidently moving on main menu
            DisableControls();

            player.gameObject.SetActive(false);

            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

        }
    }

    //Used to load up the player scene and the saved game. TODO: Add the option for multiple save slots
    public void LoadGame(bool newGame)
    {
        StartCoroutine(FadeOutUIStuff(Dummy));

        //Fade out
        //Unload mainMenu

        StartCoroutine(AsyncLoadGame(newGame));
    }

    public IEnumerator AsyncLoadGame(bool newGame)
    {
        //AsyncOperation playerSceneLoad = SceneManager.LoadSceneAsync("PlayerScene2");

        //SceneManager.LoadScene(currentLoadedLevel, LoadSceneMode.Additive);
        //LoadLevel(currentTargetID);

        Debug.Log("Deload MainMenu");
        AsyncOperation asyncMenuDeload = SceneManager.UnloadSceneAsync("MainMenu");

        yield return new WaitUntil(()=> { return asyncMenuDeload.isDone; });

        //TODO: Provide prompt to make sure players don't accidently clear save data
        if (newGame)
        {
            Debug.Log("New game save data created");
            progressContainer.ClearSaveData();
        }

        //I don't actually know why the sceneCount check is here
        if (SceneManager.sceneCount < 2)
        {
            Debug.Log("Attempting to load: " + currentTargetID.sceneName.ToString() + " as starting scene");
            LoadFirstLevel(progressContainer.progress.currentScene);
        }
        else
        {
            if (startUp == null)
            {
                startUp = FindObjectOfType<SceneStartup>();
            }

            if (startUp != null)
            {
                //Tried to have it wait until it finished
                startUp.Awake();
                //startUp.OnSceneStart(sceneStartID);
                //progressContainer.CurrentLevel(sceneStartID.sceneName);
            }
        }

        //Let the player pause whenever now
       

        yield return null;
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

    [SerializeField] Image blackEnd;

    [SerializeField] bool retryingStage = false;

    [SerializeField] int currentlyLoadedLevel = 0;
    [SerializeField] string currentLoadedLevel = "";

    [SerializeField] SceneStartID currentTargetID;
    [SerializeField] SceneStartup startUp;


    void DisableControls()
    {
        SetEnabledPlayerControls(false);
    }

    void EnableControls()
    {
        SetEnabledPlayerControls(true);
    }

    public void LoadLevel(int i)
    {
        if (i < SceneManager.sceneCountInBuildSettings)
            StartCoroutine(AsyncLevelLoad(i, FadeIntoLevel));
    }

    public void LoadLevel(SceneStartID sceneStartID)
    {
        //TODO: Add a proper check to see if scene is in build

        StartCoroutine(FadeOutUIStuff(Dummy));

        DisableControls();
        currentTargetID = sceneStartID;

        StartCoroutine(FadeOutUIStuff(LoadLevelActualLoad));
    }

    public void LoadFirstLevel(SceneStartID sceneStartID)
    {
        

        DisableControls();
        currentTargetID = sceneStartID;

        StartCoroutine(FadeOutUIStuff(FirstActualLoad));
    }

    private void FirstActualLoad()
    {
        StartCoroutine(FirstLevelLoad(currentTargetID, FadeIntoLevel));

        _GamePauseBlocked = false;

        player.gameObject.SetActive(true);
        EnableControls();
    }

    void Dummy()
    {

    }

    private void LoadLevelActualLoad()
    {
        StartCoroutine(AsyncLevelLoad(currentTargetID, FadeIntoLevel));
    }

    public void FadeIntoLevel()
    {
        StartCoroutine(FadeInUIStuff(EnableControls));
    }

    IEnumerator FirstLevelLoad(SceneStartID sceneStartID, UnityAction funcToRun)
    {
        DumpAllGameobjects();

        yield return new WaitForEndOfFrame();

        AsyncOperation sceneProgress = SceneManager.LoadSceneAsync(sceneStartID.sceneName, LoadSceneMode.Additive);

        yield return new WaitUntil(() => sceneProgress.isDone);

        currentLoadedLevel = sceneStartID.sceneName;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLoadedLevel));

        int timesTried = 0;

        while (startUp == null)
        {
            startUp = FindObjectOfType<SceneStartup>();
            yield return new WaitForSeconds(0.5f);
            timesTried++;
            if (timesTried > 6)
            {
                Debug.LogError("Failed to find sceneStartup for " + currentLoadedLevel.ToString());
                timesTried = 0;
                break;
            }
        }

        if (startUp != null)
        {
            //Tried to have it wait until it finished
            startUp.Awake();
            startUp.OnSceneStart(sceneStartID);
        }

        //Figure out better way to make sure this gets everything in the scene properly
        yield return new WaitForSecondsRealtime(0.5f);
        GetAllGameplayObjects();

        funcToRun.Invoke();
    }

    IEnumerator AsyncLevelLoad(SceneStartID sceneStartID, UnityAction funcToRun)
    {
        DumpAllGameobjects();

        yield return new WaitForEndOfFrame();


        AsyncOperation deloadSceneProgress = SceneManager.UnloadSceneAsync(currentLoadedLevel);

        yield return new WaitUntil(() => deloadSceneProgress.isDone);


        AsyncOperation sceneProgress = SceneManager.LoadSceneAsync(sceneStartID.sceneName, LoadSceneMode.Additive);

        yield return new WaitUntil(() => sceneProgress.isDone);

        currentLoadedLevel = sceneStartID.sceneName;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLoadedLevel));

        int timesTried = 0;

        while (startUp == null)
        {
            startUp = FindObjectOfType<SceneStartup>();
            yield return new WaitForSeconds(0.5f);
            timesTried++;
            if(timesTried > 6)
            {
                Debug.LogError("Failed to find sceneStartup for " + currentLoadedLevel.ToString());
                timesTried = 0;
                break;
            }
        }

        if (startUp != null)
        {
            //Tried to have it wait until it finished
            startUp.Awake();
            startUp.OnSceneStart(sceneStartID);
            progressContainer.CurrentLevel(sceneStartID);
        }

        //Figure out better way to make sure this gets everything in the scene properly
        yield return new WaitForSecondsRealtime(0.5f);
        GetAllGameplayObjects();

        funcToRun.Invoke();
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
        
        //Figure out better way to make sure this gets everything in the scene properly
        yield return new WaitForSecondsRealtime(0.5f);
        GetAllGameplayObjects();

        funcToRun.Invoke();
    }

    public void FadeOutUI(UnityAction funcToRun)
    {
        StartCoroutine(FadeOutUIStuff(funcToRun));
    }

    public void FadeInUI(UnityAction funcToRun)
    {
        StartCoroutine(FadeInUIStuff(funcToRun));
    }

    IEnumerator FadeOutUIStuff(UnityAction funcToRun)
    {
        Color current = blackEnd.color;

        while (blackEnd.color.a < 1)
        {
            current.a += 0.06f; //= new Color(current.r, current.g, current.b, op);

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
            current.a -= 0.06f; //= new Color(current.r, current.g, current.b, op);

            blackEnd.color = current;

            yield return new WaitForFixedUpdate();
        }

        funcToRun.Invoke();
    }

    public MissionLoadInfo GetNextMissionLevelName()
    {
        int lastCompleted = progressContainer.progress.lastCompletedLevel;

        foreach(MissionLoadInfo mli in missionOrderInfo.missions)
        {
            if(mli.order == lastCompleted+1)
            {
                return mli;
            }
        }

        return null;
    }
}
