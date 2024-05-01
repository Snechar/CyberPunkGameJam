 using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class BarManager : MonoBehaviour {
    private Driver driver;

    public DialogueRunner dlgRunner;
    public AudioManager audioManager;
    public Image bgImg;
    public Image leftChar;
    public Image rightChar;
    public FadeLayer curtain;

    private void Awake() {
        driver = Driver.GetInstance();

        if (dlgRunner == null) {
            dlgRunner = FindObjectOfType<DialogueRunner>();
            dlgRunner.VariableStorage = driver.GetVariableStore();
        }
        if (audioManager == null) {
            audioManager = driver.GetAudioManager();
        }

        dlgRunner.AddCommandHandler("reset", Reset);
        dlgRunner.AddCommandHandler<string>("loadBG", LoadBG);
        dlgRunner.AddCommandHandler<string>("loadLeft", LoadCharacterLeft);
        dlgRunner.AddCommandHandler<string>("loadRight", LoadCharacterRight);
        dlgRunner.AddCommandHandler("unloadLeft", UnloadLeft);
        dlgRunner.AddCommandHandler("unloadRight", UnloadRight);
        dlgRunner.AddCommandHandler<float>("fadeIn", FadeIn);
        dlgRunner.AddCommandHandler<float>("fadeOut", FadeOut);
        dlgRunner.AddCommandHandler<string>("playBG", audioManager.Play);
        dlgRunner.AddCommandHandler<float, float>("volumeBG", audioManager.SetBGVolume);
        dlgRunner.AddCommandHandler<string, float>("crossfadeBGTo", audioManager.CrossfadeTo);
        dlgRunner.AddCommandHandler<string>("switchScene", SwitchScene);

        //dlgRunner.AddCommandHandler("test", Test);
    }

    // hides left and right characters
    // unsets background
    // does not impact the audio
    private void Reset() {
        UnloadLeft();
        UnloadRight();
        bgImg.sprite = getSprite("no-signal");
    }

    private void Test() {
        Debug.Log("BarManager.Test");
        GameObject chrName = GameObject.Find("Dialogue System/Canvas/Line View/Character Name");
        Debug.Log("chrName: " + chrName);

        TextMeshPro tmp = chrName.GetComponent<TextMeshPro>();
        Debug.Log("tmp: " + tmp);

        for (int i = 0; i < chrName.GetComponentCount(); i++) {
            Debug.Log($"{i}: " + chrName.GetComponentAtIndex(i));
        }

        TMPro.TextMeshProUGUI label = chrName.GetComponent<TMPro.TextMeshProUGUI>();
        label.margin = new Vector4(300, 0, 0, 0);
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

    private void SwitchScene(string next) {
        JamScenes nextScene = JamScenes.NONE;
        switch (next.ToLower()) {
            case "tycoon":
                nextScene = JamScenes.TYCOON;
                break;
        }
        driver.SwitchScenes(nextScene);
    }
}