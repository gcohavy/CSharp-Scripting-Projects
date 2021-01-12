using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Animation _optionsMenuAnimator;
    [SerializeField] AnimationClip _fadeInAnimation;
    [SerializeField] AnimationClip _fadeOutAnimation;

    public Events.EventAnimationComplete OptionsMenuFadeComplete;

    public void FadeIn()
    {
        Debug.Log("Fading in...");
        gameObject.SetActive(true);
        _optionsMenuAnimator.Stop();
        _optionsMenuAnimator.clip = _fadeInAnimation;
        _optionsMenuAnimator.Play();
    }
    
    public void FadeOut()
    {
        Debug.Log("Fading out...");
        _optionsMenuAnimator.Stop();
        _optionsMenuAnimator.clip = _fadeOutAnimation;
        _optionsMenuAnimator.Play();
    }

    public void OnFadeInComplete()
    {
        Debug.Log("Fade in complete...");
        OptionsMenuFadeComplete.Invoke("FadeIn");
    }

    public void OnFadeOutComplete()
    {
        Debug.Log("Fade in complete...");
        OptionsMenuFadeComplete.Invoke("FadeOut");
    }
}
