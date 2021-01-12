using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //Get references to all the menu instances relevant objects
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private Camera _menuCamera;

    //Declare event variable for fading animations
    public Events.EventAnimationComplete OnMainMenuFadeComplete;

    //Get references to menu text items and buttons
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Slider fontSizeSlider;
    [SerializeField] private Slider volumeSlider;

    //Start will run before the first frame
    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
        _mainMenu.OnFadeComplete.AddListener(HandleMainMenuFadeComplete);
        _optionsMenu.OptionsMenuFadeComplete.AddListener(HandleOptionsMenuFadeComplete);

        fontSizeSlider.value = 14;
    }

    //What to do if the GameState changes
    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {

    }

    //Handle the end of the main menu fade
    void HandleMainMenuFadeComplete(string type)
    {
        if(type == "MainMenuFadeIn")
        {
            _mainMenu.BeginTitleAnimation();
        }
        
        //Pass the event onwards
        OnMainMenuFadeComplete.Invoke(type);
    }

    //Handle the options menu fade completion
    void HandleOptionsMenuFadeComplete(string type)
    {
        if (type != "FadeIn")
        {
            _mainMenu.gameObject.SetActive(true);
        }
    }

    //public method to activate or deactivate the menu camera
    public void SetMenuCameraActive(bool active)
    {
        _menuCamera.gameObject.SetActive(active);
    }

    //------Button Actions section-------

    //Main Menu
    //Start Button
    public void StartButton()
    {
        GameManager.Instance.StartGame();
    }

    //Options Button
    public void OptionsButton()
    {
        //Fadeout the Main Menu
        //Fade in the options menu
        _mainMenu.FadeOut();
        _optionsMenu.FadeIn();
    }

    //Options Menu
    //Back Botton
    public void BackButton()
    {
        //Add fading animations
        _optionsMenu.FadeOut();
        _mainMenu.FadeIn();
    }

    //Font Size slider
    public void ChangeFontSize()
    {
        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].fontSize = fontSizeSlider.value;
        }
    }

    //Create color picker

    //Create volume Slider

    //Create mode toggle

}
