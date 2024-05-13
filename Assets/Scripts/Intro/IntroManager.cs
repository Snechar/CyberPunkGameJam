using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {
    public List<IIntroElement> scriptElements;

    public List<IntroDlg> dlgBoxes;

    [SerializeField] private GameObject continueBtn;
    [SerializeField] private GameObject bgObj;
    [SerializeField] private GameObject titleObj;
    [SerializeField] private GameObject teamNameObj;
    [SerializeField] private GameObject presentsObj;
    [SerializeField] private GameObject jamObj;

    private AudioManager audioManager;

    private void Awake() {
        dlgBoxes = getDialogues();

        audioManager = Driver.GetInstance()?.GetAudioManager();
        if (audioManager != null) {
            audioManager.SetBGVolume(0f);
            audioManager.Play(JamTrack.NIGHT_CARRIER);
            audioManager.SetBGVolume(0.15f, 4);
        }

        var seq = DOTween.Sequence();

        seq.Append(bgObj.GetComponent<Image>().DOFade(1, 4));
        seq.Append(dlgBoxes[0].FadeIn(2));
        seq.AppendInterval(3);
        seq.Append(dlgBoxes[1].FadeIn(.75f));
        seq.Append(dlgBoxes[2].FadeIn(.75f));
        seq.Append(dlgBoxes[3].FadeIn(.75f));
        seq.AppendInterval(2);
        seq.Append(dlgBoxes[4].FadeIn(2));

    }

    private List<IntroDlg> getDialogues() {
        var list = new List<IntroDlg>();
        var scriptParent = GameObject.Find("Parent/Canvas/Script");
        list.AddRange(scriptParent.GetComponentsInChildren<IntroDlg>());
        foreach (Transform childT in scriptParent.transform) {
            childT.gameObject.SetActive(false);
        }
        list.ForEach((IntroDlg dlg) => dlg.SetAlpha(0));
        return list;
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
