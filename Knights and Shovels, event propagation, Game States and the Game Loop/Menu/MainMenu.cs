//This is the script to be attached to the Main Menu in the editor in order to animate it
//This script must:
//  Track the Animation component
//  Track the Animation clips for Fade in/out
//  Function that can receive Animation events
//  Functions to play Fade in/out Animations

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainMenu : MonoBehaviour
{
    // Declare private but serializable fields for the animation components
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimation;
    [SerializeField] AnimationClip _fadeInAnimation;

    //Declare the event to be fired when the animation completes. For a manager to hande
    public Events.EventFadeComplete OnEventFadeComplete;

    //Start method to Run on instantiation and register for the State Changed event in the Game Manager
    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    //Method that will run when the Game Manager fires the event for State Change
    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        //Fade out if the state was changed from PREGAME to RUNNING
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }
        //Fade in if state changed back to PREGAME
        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

    //Method to run once the fade out has completed to fire the event
    public void OnFadeOutComplete()
    {
        Debug.Log("Fade out complete");
        OnEventFadeComplete.Invoke(true);
    }

    //Method to invoke the event on fade in completion and to set the dummy camera back to active
    public void OnFadeInComplete()
    {
        Debug.Log("Fade in complete");
        UIManager.Instance.SetDummyCameraActive(true);

        OnEventFadeComplete.Invoke(false);
    }

    //Method to fade out
   public void FadeOut()
    {
        //Turn off dummy camera
        UIManager.Instance.SetDummyCameraActive(false);

        //Stop any previous animation, set the correct animation, and play it
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }

    //Method to fade in
    public void FadeIn()
    {
        //Stop any previous animation, set the correct animation and play it
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }
}
