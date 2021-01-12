using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animation _mainMenuAnimator;
    [SerializeField] AnimationClip _fadeOutAnimation;
    [SerializeField] AnimationClip _fadeInAnimation;
    [SerializeField] AnimationClip _titleTextAnimation;

    public Events.EventAnimationComplete OnFadeComplete;

    void Awake()
    {
        BeginTitleAnimation();
    }

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }
        if(previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }

    public void OnFadeOutComplete()
    {
        Debug.Log("Fade out complete...");
        OnFadeComplete.Invoke("MainMenuFadeOut");
    }

    public void OnFadeInComplete()
    {
        Debug.Log("Fade in complete...");
        UIManager.Instance.SetMenuCameraActive(true);

        OnFadeComplete.Invoke("MainMenuFadeIn");

    }

    public void FadeOut()
    {
        Debug.Log("fading Out...");

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeIn()
    {
        Debug.Log("fading In...");
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }

    public void BeginTitleAnimation()
    {
        //Begin animating the title text
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _titleTextAnimation;
        _mainMenuAnimator.Play();
    }
}
