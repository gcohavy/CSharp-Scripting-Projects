//The Game Manager script is to track the Game State and facilitate communication 
// between the different systems

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Declare the different Game States as enums
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    //Declare the variables needed in this class
    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;
    private List<GameObject> _instancedSystemPrefabs;
    public string _currentLevelName = string.Empty;
    List<AsyncOperation> _loadOperations;

    //Create the GameState defaulted to PREGAME
    GameState _currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get{ return _currentGameState; }
        private set{ _currentGameState = value; }
    }

    //Start method executes on the first frame
    void Start()
    {
        //Built in method to make sure this object doesn't get destroyed when different levels
        // are loaded
        DontDestroyOnLoad(gameObject);

        //Initialize lists
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        //Instantiate system prefabs once the game starts
        InstantiateSystemPrefabs();

        //Register for the Main Menu fade completed event from the UI Manager
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }

    //Update method will execute every frame
    void Update()
    {
        //Do not listen for the ESC key for the Pause menu if it is a PREGAME state
        if(_currentGameState == GameState.PREGAME)
        {
            return;
        }

        //Otherwise, toggle the Pause state if the user presses ESC
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    //Method to run once a level is finished loading
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        //remove the async operation from the list of operations we are keeping
        // track of
        if(_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            //If there are no more load operations to wait for, change the GameState to RUNNING
            if(_loadOperations.Count == 0)
                UpdateState(GameState.RUNNING);
                //Add messages, transition Scene, etc etc
        }
        Debug.Log("Load Complete...");
    }

    //Method to run once the Unload operation has completed
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        //currently just debugging
        Debug.Log("Unload Complete...");
    }

    //Method to instantiate all system prefabs
    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    //Method to load a level after receiving an input of the level's name
    public void LoadLevel(string levelName)
    {
        //Save the async operation to be referenced later
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        
        //Debugging in case of error
        if(ao == null)
        {
            Debug.LogError("Unable to load level: " + levelName);
            return;
        }

        //Add listener to the operation completed built-in event
        ao.completed += OnLoadOperationComplete;
        //Add the operation to the collection of load operations
        _loadOperations.Add(ao);
        //Set the current level name to keep track of later
        _currentLevelName = levelName;
    }

    //Make a method to unload levels based on their name
    public void UnloadLevel(string levelName)
    {
        //Unload the levels asynchronously and keep track of the operation
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);

        //Debug in case of an error
        if(ao == null)
        {
            Debug.LogError("Unable to unload level: " + levelName);
            return;
        }

        //Register for the completed event
        ao.completed += OnUnloadOperationComplete;
    }

    //Create a method to update and handle a State change
    void UpdateState(GameState state)
    {
        //keep track of the previous Game State, and then set the current state
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        //Handle any changes that need to be made based on Game State
        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0;
                break;

            default:
                break;
        }

        //Fire the method for the UI manager to know that the Game State has changed
        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    //override the OnDestroy method from the Singleton class
    protected override void OnDestroy()
    {
        //first call the original method so we get the same functionality
        base.OnDestroy();

        //Destroy all the system prefabs if the game Manager is destroyed
        for(int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }

        //Clear the list of System Prefabs
        _instancedSystemPrefabs.Clear();
    }

    //Method to Load the Main level if the user starts the game
    public void StartGame()
    {
        LoadLevel("Main");
    }

    //Method to update the Game State to either PAUSED or RUNNING 
    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    //Method to restart the game by updating the Game State to PREGAME
    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    //Method to Quit the Game
    public void QuitGame()
    {
        //Implement features that you want to happen before quitting
        Application.Quit();
    }

    //Create the method that will handle the main menu fading
    // if the main menu finishes fading in, Unload the level
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if(!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
    }
}
