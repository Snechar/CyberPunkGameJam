using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class Driver : MonoBehaviour {
    private static Driver instance;
    public const string BAR_NAME = "Bar";
    public const string TYCOON_NAME = "Tycoon";

    public AudioManager audioManager;

    private bool requestedBar = false;
    private Scene barScene;
    private GameObject barRoot;
    private bool requestedTycoon = false;
    private Scene tycoonScene;
    private GameObject tycoonRoot;

    private InMemoryVariableStorage variableStore;

    private JamScenes curScene;
    private JamScenes nextScene;

    public static Driver GetInstance() {
        return Driver.instance;
    }

    private void Awake() {
        Driver.instance = this;
        variableStore = GetComponent<InMemoryVariableStorage>();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private Button startVNButton;

    void Update() {
        if (nextScene != curScene) {
            if (!HasLoaded(nextScene)) {
                Debug.Log($"{nextScene} has not yet loaded, waiting");
                QueueLoad(nextScene);
                return;
            }

            var oldRoot = GetRoot(curScene);
            var newRoot = GetRoot(nextScene);

            if (oldRoot != null) {
                oldRoot.SetActive(false);
            }
            if (newRoot != null) {
                newRoot.SetActive(true);
            }

            curScene = nextScene;
        }

        if (startVNButton == null) {
            startVNButton = GameObject.Find("StartDlg").GetComponent<Button>();
        }
        startVNButton.interactable = curScene == JamScenes.BAR;
    }

    public AudioManager GetAudioManager() {
        return audioManager;
    }

    public InMemoryVariableStorage GetVariableStore() {
        return variableStore;
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log($"SceneLoaded: {scene.name}");
        switch (scene.name) {
            case BAR_NAME:
                barRoot = barScene.GetRootGameObjects()[0];
                break;
            case TYCOON_NAME:
                tycoonRoot = tycoonScene.GetRootGameObjects()[0];
                break;
        }
    }

    public void LoadBar() {
        if (requestedBar) {
            return;
        }
        SceneManager.LoadScene(BAR_NAME, LoadSceneMode.Additive);
        barScene = SceneManager.GetSceneByName(BAR_NAME);
    }

    public void LoadTycoon() {
        if (requestedTycoon) {
            return;
        }
        SceneManager.LoadScene(TYCOON_NAME, LoadSceneMode.Additive);
        tycoonScene = SceneManager.GetSceneByName(TYCOON_NAME);
    }

    public void Button1() {
        barRoot.SetActive(!barRoot.activeSelf);
    }

    public void Button2() {
        tycoonRoot.SetActive(!tycoonRoot.activeSelf);
    }

    public void TriggerVN() {
        var dlgRunner = barRoot.GetComponentInChildren<DialogueRunner>();
        var txt = GameObject.Find("StartDlg/NodeField").GetComponent<TMP_InputField>();
        var nodeName = txt.text.Trim();
        Debug.Log(nodeName);
        if (nodeName.Trim().Length == 0) {
            nodeName = "Begin";
        }
        Debug.Log($"Starting dialog at node {nodeName}");
        dlgRunner.StartDialogue(nodeName);
    }

    public void SwitchScenes(JamScenes toScene) {
        if (curScene == toScene) {
            return;
        }
        nextScene = toScene;
    }

    private void QueueLoad(JamScenes tgt) {
        switch (tgt) {
            case JamScenes.BAR:
                LoadBar();
                break;
            case JamScenes.TYCOON:
                LoadTycoon();
                break;
        }
    }

    private bool HasLoaded(JamScenes tgt) {
        switch (tgt) {
            case JamScenes.TYCOON:
                return tycoonRoot != null;
            case JamScenes.BAR:
                return barRoot != null;
            case JamScenes.NONE:
                return true;
        }
        return false;
    }

    private GameObject GetRoot(JamScenes tgt) {
        switch (tgt) {
            case JamScenes.BAR:
                return barRoot;
            case JamScenes.TYCOON:
                return tycoonRoot;
        }
        return null;
    }

    public void Debug_SwitchToBar() {
        SwitchScenes(JamScenes.BAR);
    }

    public void Debug_SwitchToTycoon() {
        SwitchScenes(JamScenes.TYCOON);
    }
    
    public void Debug_SwitchToNone() {
        SwitchScenes(JamScenes.NONE);
    }
}

public enum JamScenes {
    NONE,
    BAR,
    TYCOON,
}