//This script to be attached to the UIManager prefab to communicate with the Game Manager
// and handle any changes to the UI
//This should be the bridge between the Game Manager and all the components of the UI Menus

using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //Get serializable references to the UI components
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private PauseMenu _pauseMenu;

    //Declare the event to fire after the main Menu finished its fade animation 
    // in order to chain it and have the Game Manager see the event without needing
    // to register for each individual component
    public Events.EventFadeComplete OnMainMenuFadeComplete;

    //Start method will run on the first frame
    void Start()
    {
        //Register for the State Change event from the game Manager, and Main Menu fade event
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        _mainMenu.OnEventFadeComplete.AddListener(HandleMainMenuFadeComplete);
    }

    //Update runs every frame
    void Update()
    {
        //Don't wait for any key presses if the GameState is not PREGAME
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }

        //Run the StartGame method in the GameManager if the user presses Space
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartGame();
        }
    }

    //public method to toggle the dummy camera
    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    //Method to make relevant changes to the UI based on State change
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        //Set the pause menu as active if the state was changed to PAUSED
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
    }

    //Make relevant changes on completion of Main Menu fade animations
    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        //Fire the same event but this time so the GameManager can see it
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }
}
