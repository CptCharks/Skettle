using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
        get { return this._GamePaused; }
        set { this.gamePaused = _GamePaused; }
    }
    private bool _GamePaused;
    private bool _GamePauseBlocked = false;

    private bool _GameGatheringObjects = false;

    public List<GameplayComponent> allCurrentGameplay;
    List<GameplayComponent> addList;
    List<GameplayComponent> removeList;

    public PlayerController player;
    public GameObject playerInfo;
    PlayerGunManager gunManager;


    public ProgressContainer progressContainer;
    public MissionOrderInfo missionOrderInfo;

    int currentSaveFile = 0;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject optionsMenu;

    bool optionMenuOpen = false;
    bool controlsMenuOpen = false;
    

    private void Start()
    {
        DontDestroyOnLoad(this);

        pauseMenu.SetActive(false);

        progressContainer = GetComponent<ProgressContainer>();

        missionOrderInfo = (MissionOrderInfo)Resources.Load("MissionOrder");

        addList = new List<GameplayComponent>();
        removeList = new List<GameplayComponent>();
        GetAllGameplayObjects();

        //Get player variables. Might want to hard set the PlayerController and PlayerGO here too

        SearchForAndAssignPlayer();


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
                    startUp.OnSceneStart(currentTargetID, this);
                //progressContainer.CurrentLevel(sceneStartID.sceneName);
            }

            //Need to automatically assign the currentLevel
            //currentLoadedLevel = "";
        }
        else
        {
            playerInfo?.SetActive(false);

            //Prevent the game from being paused on the main menu
            _GamePauseBlocked = true;

            //Prevent player from accidently moving on main menu
            DisableControls();

            if (player != null)
                player.gameObject.SetActive(false);

            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);

        }
    }

    //Used to load up the player scene and the saved game. TODO: Add the option for multiple save slots
    public void LoadGame(bool newGame, int saveFile = 0)
    {
        currentSaveFile = saveFile;

        StartCoroutine(FadeOutUIStuff(Dummy));

        //Fade out
        //Unload mainMenu

        StartCoroutine(AsyncLoadGame(newGame, currentSaveFile));
    }

    public IEnumerator AsyncLoadGame(bool newGame, int saveGame = 0)
    {
        Debug.Log("Deload MainMenu");
        AsyncOperation asyncMenuDeload = SceneManager.UnloadSceneAsync("MainMenu");

        yield return new WaitUntil(()=> { return asyncMenuDeload.isDone; });
        
        if (newGame)
        {
            Debug.Log("New game save data created");
            progressContainer.ClearSaveData();
        }
        else
        {
            //Put necessary cleanup for loading a previous save
            progressContainer.LoadSaveFile(saveGame);
        }

        //Load the player weapons
        gunManager.LoadPlayerSavedGuns(progressContainer.playerData.weaponOwnership);
        gunManager.LoadPlayerSavedAmmo(progressContainer.playerData.extraAmmoCounts,progressContainer.playerData.currentAmmo);

        //Load player health
        player.healthController.tempHit = progressContainer.playerData.health;

        //Potentially could load the player position since the first load is different from the scene load.
        ////


        if (SceneManager.sceneCount < 2)
        {
            Debug.Log("Attempting to load: " + currentTargetID.sceneName.ToString() + " as starting scene");
            LoadFirstLevel(progressContainer.LoadLevel());
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

    public void SearchForAndAssignPlayer()
    {
        playerInfo = GameObject.FindGameObjectWithTag("PlayerUI");

        var playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null)
        {
            player = playerGo.GetComponent<PlayerController>();
            playerHit = playerGo.GetComponentInChildren<Hittable>();
            gunManager = playerGo.GetComponent<PlayerGunManager>();
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
        if(player != null)
            player.EnableDisableControls(enablePlayer);
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

    //All GameplayObjects are updated here and paused as needed without changing the timescale
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) /*Put any pause locks here*/ )
        {
            PauseUnpause();
        }

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
        currentLoadedLevel = sceneStartID.sceneName;

        //TODO: Probably should replace this with a cutscene activation
        StartCoroutine(FadeOutUIStuff(FirstActualLoad));
    }

    private void FirstActualLoad()
    {
        StartCoroutine(FirstLevelLoad(currentTargetID, FadeIntoLevel));

        _GamePauseBlocked = false;

        player.gameObject.SetActive(true);
        playerInfo?.SetActive(true);
        EnableControls();
    }

    void Dummy(){}

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
                var tempSceneStartup = new GameObject();
                startUp = tempSceneStartup.AddComponent<SceneStartup>();
                timesTried = 0;
                break;
            }
        }

        if (startUp != null)
        {
            //Tried to have it wait until it finished
            startUp.Awake();
            startUp.OnSceneStart(sceneStartID, this);
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
                var tempSceneStartup = new GameObject();
                startUp = tempSceneStartup.AddComponent<SceneStartup>();
                timesTried = 0;
                break;
            }
        }

        if (startUp != null)
        {
            //Tried to have it wait until it finished
            startUp.Awake();
            startUp.OnSceneStart(sceneStartID, this);

            //Save scene ID to progress
            progressContainer.SaveCurrentLevel(sceneStartID);
        }

        //Figure out better way to make sure this gets everything in the scene properly
        yield return new WaitForSecondsRealtime(0.5f);
        GetAllGameplayObjects();

        //Basically the autosave. TODO: Test and make sure we don't screw over the player ammo wise or health wise. We could make a save post in most areas of the game.
        progressContainer.SaveSaveFile(currentSaveFile);

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
        int lastCompleted = progressContainer.progress.currentLevel;

        foreach(MissionLoadInfo mli in missionOrderInfo.missions)
        {
            if(mli.order == lastCompleted+1)
            {
                return mli;
            }
        }

        return null;
    }

    public void PurchaseWeapon(int gunEnum)
    {
        var gun = gunManager.avaliableGuns[gunEnum];
        gun.avaliable = true;
        gunManager.avaliableGuns[gunEnum] = gun;
        gunManager.avaliableGuns[gunEnum].gun.extraAmmo = 6;

        SaveWeaponInfo();
    }

    public void PurchaseAmmo(int gunEnum)
    {
        //Probably need to fill up the current ammo then the extra ammo. Might want to move the logic to the weapon.
        gunManager.avaliableGuns[gunEnum].gun.GainAmmo(6);
        SaveWeaponInfo();
    }

    public void SaveWeaponInfo()
    {
        progressContainer.playerData.SaveGunOwnership(gunManager.avaliableGuns);
        progressContainer.playerData.SaveAmmoCounts(gunManager.avaliableGuns);

        progressContainer.SaveSaveFile(currentSaveFile);
    }

    public void SaveGame()
    {
        progressContainer.SaveSaveFile(currentSaveFile);
    }

    public void PauseUnpause()
    {
        _GamePaused = !_GamePaused;
        pauseMenu.SetActive(gamePaused);
        Time.timeScale = gamePaused ? 0 : 1;
    }

    public void OpenCloseOptions()
    {
        controlsMenuOpen = false;
        controlsMenu.SetActive(controlsMenuOpen);

        optionMenuOpen = !optionMenuOpen;
        optionsMenu.SetActive(optionMenuOpen);
    }

    public void OpenCloseControls()
    {
        optionMenuOpen = false;
        optionsMenu.SetActive(optionMenuOpen);

        controlsMenuOpen = !controlsMenuOpen;
        controlsMenu.SetActive(controlsMenuOpen);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
