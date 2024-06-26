using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class Driver : MonoBehaviour {
    private static Driver instance;
    public const string BAR_NAME = "Bar";
    public const string TYCOON_NAME = "SampleScene";
    public const string INTRO_NAME = "Intro";

    public AudioManager audioManager;
    public Camera mainCamera;
    public OrderManager orderManager;

    private bool requestedBar = false;
    private Scene barScene;
    private GameObject barRoot;
    
    private bool requestedTycoon = false;
    private Scene tycoonScene;
    private GameObject tycoonRoot;

    private bool requestedIntro = false;
    private Scene introScene;
    private GameObject introRoot;

    private InMemoryVariableStorage variableStore;
    private RequestManager requestManager;

    private Button startVNButton;

    private JamScenes curScene;
    private JamScenes nextScene;

    public GameObject cycleDisplay;
    public TMP_Text cycleDisplayText;
    public GameObject orderDisplay;

    public static Driver GetInstance() {
        return Driver.instance;
    }

    private void Awake() {
        Driver.instance = this;
        requestManager = GetComponent<RequestManager>();
        SceneManager.sceneLoaded += SceneLoaded;

        var startButton = GameObject.Find("StartDlg");
        if (startButton != null) {
            startVNButton.GetComponent<Button>();
        }

        SwitchScenes(JamScenes.INTRO);
        // SwitchScenes(JamScenes.BAR);
    }

    void Update() {
        var hudActive = curScene == JamScenes.BAR || curScene == JamScenes.TYCOON;
        cycleDisplay.SetActive(hudActive);
        orderDisplay.SetActive(hudActive);
        if (cycleDisplay.activeSelf) {
            cycleDisplayText.text = "Day: " + CurrentCycleNumber();
        }
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

        if (startVNButton != null) {
            startVNButton.interactable = curScene == JamScenes.BAR;
        }
    }

    public AudioManager GetAudioManager() {
        return audioManager;
    }

    public RequestManager GetRequestManager() {
        return requestManager;
    }

    public InMemoryVariableStorage GetVariableStore() {
        return variableStore;
    }

    public void SetVariableStore(InMemoryVariableStorage storeage) {
        variableStore = storeage;
    }

    public void SyncOrders(RequestManager reqMgr) {
        orderManager?.SyncOrders(reqMgr);
    }

    public void Tick() {
        orderManager?.Tick();
    }

    public void SceneLoaded(Scene scene, LoadSceneMode mode) {
        // Debug.Log($"SceneLoaded: {scene.name}");
        switch (scene.name) {
            case BAR_NAME:
                barRoot = barScene.GetRootGameObjects()[0];
                break;
            case TYCOON_NAME:
                tycoonRoot = tycoonScene.GetRootGameObjects()[0];
                var canvas = tycoonRoot.GetComponentInChildren<Canvas>();
                canvas.worldCamera = mainCamera;
                break;
            case INTRO_NAME:
                introRoot = introScene.GetRootGameObjects()[0];
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

    public void LoadIntro() {
        if (requestedIntro) {
            return;
        }
        SceneManager.LoadScene(INTRO_NAME, LoadSceneMode.Additive);
        introScene = SceneManager.GetSceneByName(INTRO_NAME);
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
        // Debug.Log(nodeName);
        if (nodeName.Trim().Length == 0) {
            nodeName = "Begin";
        }
        // Debug.Log($"Starting dialog at node {nodeName}");
        dlgRunner.StartDialogue(nodeName);
    }

    public void SwitchScenes(JamScenes toScene) {
        Debug.Log($"SwitchScenes {curScene} -> {toScene} ");
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
            case JamScenes.INTRO:
                LoadIntro();
                break;
        }
    }

    private bool HasLoaded(JamScenes tgt) {
        switch (tgt) {
            case JamScenes.TYCOON:
                return tycoonRoot != null;
            case JamScenes.BAR:
                return barRoot != null;
            case JamScenes.INTRO:
                return introRoot != null;
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
            case JamScenes.INTRO:
                return introRoot;
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

    public int CurrentCycleNumber() {
        if (CycleManagerSingleton.Instance == null) {
            return 0;
        }
        return CycleManagerSingleton.Instance.GetCurrentCycle();
    }
}

public enum JamScenes {
    NONE,
    INTRO,
    BAR,
    TYCOON,
}