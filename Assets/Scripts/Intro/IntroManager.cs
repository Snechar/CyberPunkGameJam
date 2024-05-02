using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {
    public List<IIntroElement> scriptElements;

    public GameObject continueBtn;

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
        completed = true;
        var driver = Driver.GetInstance();
        driver.GetAudioManager().SetBGVolume(0, 1);
        driver.GetVariableStore().SetValue("$isFirstStart", true);
        driver.SwitchScenes(JamScenes.BAR);
    }
}
