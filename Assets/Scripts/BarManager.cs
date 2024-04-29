using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class BarManager : MonoBehaviour {
    public DialogueRunner dlgRunner;
    public Image bgImg;
    public Image leftChar;
    public Image rightChar;
    public FadeLayer curtain;

    private void Awake() {
        if (dlgRunner == null) {
            this.dlgRunner = FindObjectOfType<DialogueRunner>();
        }
        dlgRunner.AddCommandHandler("reset", Reset);
        dlgRunner.AddCommandHandler<string>("loadBG", LoadBG);
        dlgRunner.AddCommandHandler<string>("loadLeft", LoadCharacterLeft);
        dlgRunner.AddCommandHandler<string>("loadRight", LoadCharacterRight);
        dlgRunner.AddCommandHandler("unloadLeft", UnloadLeft);
        dlgRunner.AddCommandHandler("unloadRight", UnloadRight);
        dlgRunner.AddCommandHandler<float>("fadeIn", FadeIn);
        dlgRunner.AddCommandHandler<float>("fadeOut", FadeOut);
    }

    private void Reset() {
        UnloadLeft();
        UnloadRight();
        bgImg.sprite = getSprite("no-signal");
    }

    private Sprite getSprite(string path) {
        Sprite img = Resources.Load<Sprite>(path);
        if (img == null) {
            Debug.Log($"Failed to load sprite at {path}");
            img = Resources.Load<Sprite>("no-signal");
        }
        return img;
    }

    private void LoadBG(string path) {
        bgImg.sprite = getSprite(path);
    }

    private void LoadCharacterLeft(string path) {
        leftChar.sprite = getSprite(path);
        leftChar.enabled = true;
    }

    private void UnloadLeft() {
        leftChar.enabled = false;
        leftChar.sprite = null;
    }

    private void LoadCharacterRight(string path) {
        rightChar.sprite = getSprite(path);
        rightChar.enabled = true;
    }

    private void UnloadRight() {
        rightChar.enabled = false;
        rightChar.sprite = null;
    }

    private Coroutine FadeIn(float time = 1f) {
        return StartCoroutine(curtain.ChangeAlphaOverTime(0, time));
    }

    private Coroutine FadeOut(float time = 1f) {
        return StartCoroutine(curtain.ChangeAlphaOverTime(1, time));
    }
}