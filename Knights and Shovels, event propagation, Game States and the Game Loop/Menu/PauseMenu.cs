//Class to attach to the Pause menu to be set to active when the ESC key is pressed
// and to handle the button actions

using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Declare the private but serializable fields to keep track of the buttons in the menu
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    //Start method will run on first method after instantiation
    void Start()
    {
        //Add methods to be invoked on each button press
        resumeButton.onClick.AddListener(HandleResumeClicked);
        restartButton.onClick.AddListener(HandleRestartClicked);
        quitButton.onClick.AddListener(HandleQuitClicked);
    }

    //Method to be invoked if the player presses Resume from the pause menu
    void HandleResumeClicked()
    {
        //Run the toggle pause method written in the Game Manager script
        GameManager.Instance.TogglePause();
    }

    //Method to be invoked if the player presses Restart
    void HandleRestartClicked()
    {
        //Run the RestartGame method in the Game Manager script
        GameManager.Instance.RestartGame();
    }

    //Method to be invoked if the player presses Quit
    void HandleQuitClicked()
    {
        //Run the QuitGame method in the Game Manager script
        GameManager.Instance.QuitGame();
    }
}
