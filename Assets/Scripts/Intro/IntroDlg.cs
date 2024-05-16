using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroDlg : MonoBehaviour {
    [SerializeField] private Image bgImg;
    [SerializeField] private TMP_Text text;

    private bool hasPlayed = false;

    public float alpha {
        get { return bgImg.color.a; }
        set {
            var curColorBG = bgImg.color;
            curColorBG.a = value;
            bgImg.color = curColorBG;
            var curColorText = text.color;
            curColorText.a = value;
            text.color = curColorText;
        }
    }

    void Awake() {
        bgImg = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
    }

    public void Play() {
        gameObject.SetActive(true);
        hasPlayed = true;
    }

    public TweenerCore<float, float, FloatOptions> Fade(bool fadeIn, float duration) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        float stop = fadeIn ? 1 : 0;

        var tween = DOTween.To(
            () => bgImg.color.a,
            (float newA) => alpha = newA,
            stop,
            duration
        ); ;
        tween.SetEase(Ease.InQuart);

        return tween;
    }

    public TweenerCore<float, float, FloatOptions> FadeIn(float duration) {
        return Fade(true, duration);
    }

    public TweenerCore<float, float, FloatOptions> FadeOut(float duration) {
        return Fade(false, duration);
    }
}