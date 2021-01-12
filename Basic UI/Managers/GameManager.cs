//Game Manager to be a Singleton, never destroyed to keep track of information 
// and logic that must be stored across game levels

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    //Create enum Game States
    public enum GameState {
        PREGAME,
        RUNNING
    }

    //Declare all relevant variables
    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChange;
    private List<GameObject> _instancedSystemPrefabs;
    private string _currentLevelName = string.Empty;
    List<AsyncOperation> _loadOperations;
    GameState _currentGameState = GameState.PREGAME;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set{ _currentGameState = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Make sure the GameManager object doesnt get destroyed every level
        DontDestroyOnLoad(gameObject);

        //Initialize lists
        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        //Instantiate System Prefabs
        InstantiateSystemPrefabs();

        //Subscribe to the MainMenuFadeComplete event from the UIManager
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }

    //public method to update the GameState
    public void UpdateState(GameState state)
    {
        //keep track of previous state
        GameState previousState = CurrentGameState;
        CurrentGameState = state;

        //Fire the event
        OnGameStateChange.Invoke(CurrentGameState, previousState);
    }

    //Instantiate all System Prefabs
    void InstantiateSystemPrefabs()
    {
        //Instantiate all System Prefabs and add them to the _instantiated prefabs list
        GameObject prefabInstance;
        for(int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    //Method to load levels
    public void LoadLevel(string levelName)
    {
        //Save the async operation for tracking
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        //Debugging in case of error
        if(ao==null)
        {
            Debug.LogError("Couldn't load level: " + levelName);
            return;
        }

        //Add listeners for Async Operation Completion
        ao.completed += OnLoadOperationComplete;

        //Add ao to the list of _loadOperations
        _loadOperations.Add(ao);

        //Set _currentLevelName to the loading level
        _currentLevelName = levelName;
    }

    //Method to Unload levels
    public void UnloadLevel(string levelName)
    {
        //Save the async operation for tracking
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);

        //Debug in case of error
        if(ao == null)
        {
            Debug.LogError("Could not unload level: " + levelName);
            return;
        }

        //Add listener for Async operation completion
        ao.completed += OnUnloadOperationComplete;
    }

    //public method to start the game
    public void StartGame()
    {
        LoadLevel("Main");
    }

    //Handle when the MainMenu finishes fading
    void HandleMainMenuFadeComplete(string type)
    {
        if(type == "MainMenuFadeIn" && _currentLevelName == "Main")
        {
            UnloadLevel(_currentLevelName);
            _currentLevelName = null;
        }
    }

    //Method to run after a level finishes loading
    void OnLoadOperationComplete(AsyncOperation ao)
    {
        UIManager.Instance.SetMenuCameraActive(false);
        //Make sure the load operation is in _loadOperations list
        if(_loadOperations.Contains(ao))
        {
            //Remove the operation
            _loadOperations.Remove(ao);

            //Update State to RUNNING once everything is loaded
            if(_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }

        Debug.Log("Load complete...");
    }

    //Method to run after a level finishes unloading
    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("Unload Complete...");
    }

    //Override methods
    protected override void OnDestroy()
    {
        base.OnDestroy();

        //Destroy all System Prefabs and remove them from the list of instancedprefabs
        for(int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }
}
