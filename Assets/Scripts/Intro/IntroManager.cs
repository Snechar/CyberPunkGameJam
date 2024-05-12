using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {
    public List<IIntroElement> scriptElements;

    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject bgObj;
    [SerializeField] private GameObject titleObj;
    [SerializeField] private GameObject teamNameObj;
    [SerializeField] private GameObject presentsObj;
    [SerializeField] private GameObject jamObj;

    private AudioManager audioManager;

    [SerializeField] private int initialFramePointer = 0;
    private int fp;

    private List<IntroStep> script;

    private void Awake() {
        audioManager = Driver.GetInstance()?.GetAudioManager();
        if (audioManager != null) {
            audioManager.SetBGVolume(0f);
            audioManager.Play(JamTrack.NIGHT_CARRIER);
            audioManager.SetBGVolume(0.15f, 4);
        }
        fp = initialFramePointer;

        script = getScript();

        var seq = DOTween.Sequence();

        seq.Append(bgObj.GetComponent<Image>().DOFade(1, 4));
        var id = GameObject.Find("Parent/Canvas/Script/Dialogue").GetComponent<IntroDlg>();
        seq.Append(id.FadeIn(4));

    }

    private List<IntroStep> getScript() {
        var scr = new List<IntroStep> {
            IntroStep.FadeIn(bgObj.GetComponent<Image>(), 3),
        };
        return scr;
    }

    /*

    private void Awake() {
        scriptElements = new List<IIntroElement>();

        GameObject script = GameObject.Find("Parent/Canvas/Script");
        foreach (var c in script.GetComponentsInChildren<IIntroElement>()) {
            scriptElements.Add(c);
            var mb = (MonoBehaviour)c;
            mb.gameObject.SetActive(false);
        }
    }

    int idx = -1;
    int hasWaitedMS = 0;
    bool isWaitingButton = false;
    bool completed = false;

    public void HitContinue() {
        isWaitingButton = false;
        continueBtn.SetActive(false);
    }

    private bool maybeAdvance() {
        if (isWaitingButton) {
            return false;
        }
        if (idx == -1) {
            return true;
        }
        if (idx == scriptElements.Count) {
            return false;
        }
        return scriptElements[idx].IsDone(hasWaitedMS);
    }

    // Update is called once per frame
    void Update() {
        IntroCompleted();
        if (completed) {
            return;
        }
        continueBtn.SetActive(isWaitingButton);
        if (maybeAdvance()) {
            idx++;
            if (idx == scriptElements.Count) {
                IntroCompleted();
            } else {
                var newIIE = scriptElements[idx];
                newIIE.Play();
                hasWaitedMS = 0;
                if (newIIE.ShouldDeactivatePrevious()) {
                    for (int i = 0; i < idx; i++) {
                        var ele = scriptElements[i];
                        if (ele is IntroDlg) {
                            ((MonoBehaviour)ele).gameObject.SetActive(false);
                        }
                    }
                }
                if (newIIE.WaitsForInput()) {
                    isWaitingButton = true;
                }
            }
        } else {
            hasWaitedMS += (int)(Time.deltaTime * 1000);
        }
    }

    private void IntroCompleted() {
        if (completed) {
            return;
        }
        completed = true;
        continueBtn.SetActive(false);
        var driver = Driver.GetInstance();
        driver.GetAudioManager().SetBGVolume(0, 1);
        driver.SwitchScenes(JamScenes.BAR);
    }
    */
}
