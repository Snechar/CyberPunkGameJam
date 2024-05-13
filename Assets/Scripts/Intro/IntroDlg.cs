using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroDlg : MonoBehaviour, IIntroElement {
    public int DelayPostShowMS = 1500;
    public bool WaitForContinue = false;
    public bool ClearPrevious = false;
    public bool StartFadedOut = false;

    private Image bgImg;
    private TMP_Text text;
    private Color defaultColor;

    private bool hasPlayed = false;

    void Awake() {
        bgImg = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
        defaultColor = text.color;
        if (StartFadedOut) {
            var x = bgImg.color;
            x.a = 0;
            bgImg.color = x;
            x = text.color;
            x.a = 0;
            text.color = x;
        }
    }

    public void Play() {
        gameObject.SetActive(true);
        hasPlayed = true;
    }

    public bool IsDone(int hasWaitedMS) {
        return hasPlayed && hasWaitedMS >= DelayPostShowMS;
    }

    public bool WaitsForInput() {
        return WaitForContinue;
    }

    public bool ShouldDeactivatePrevious() {
        return ClearPrevious;
    }

    public TweenerCore<float, float, FloatOptions> Fade(bool fadeIn, float duration) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        float stop = fadeIn ? 1 : 0;

        var tween = DOTween.To(
            () => bgImg.color.a,
            (float newA) => SetAlpha(newA),
            stop,
            duration
        );
        tween.SetEase(Ease.InQuart);

        return tween;
    }

    public void SetAlpha(float alpha) {
        var curColorBG = bgImg.color;
        curColorBG.a = alpha;
        bgImg.color = curColorBG;
        var curColorText = text.color;
        curColorText.a = alpha;
        text.color = curColorText;

    }

    public TweenerCore<float, float, FloatOptions> FadeIn(float duration) {
        return Fade(true, duration);
    }

    public TweenerCore<float, float, FloatOptions> FadeOut(float duration) {
        return Fade(false, duration);
    }
}

public interface IIntroElement {
    // makes the thing take its action
    public void Play();
    
    // is the thing done playing
    public bool IsDone(int hasWaitedMS);

    public bool WaitsForInput();

    public bool ShouldDeactivatePrevious();
}