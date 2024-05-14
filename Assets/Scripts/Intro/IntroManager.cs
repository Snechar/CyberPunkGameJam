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
    private Sequence tweenSeq;

    private void Awake() {
        dlgBoxes = getDialogues();

        audioManager = Driver.GetInstance()?.GetAudioManager();
        if (audioManager != null) {
            audioManager.SetBGVolume(0f);
            audioManager.Play(JamTrack.NIGHT_CARRIER);
            audioManager.SetBGVolume(0.15f, 4);
        }

        DOTween.defaultEaseType = Ease.Linear;

        var seq = DOTween.Sequence();
        tweenSeq = seq;

        var bgImg = bgObj.GetComponent<Image>();
        var transparentColor = bgImg.color;
        transparentColor.a = 0;
        bgImg.color = transparentColor;

        var clearSpeed = 1.5f;

        #region story setup
        seq.Append(bgObj.GetComponent<Image>().DOFade(1, 4));
        seq.Append(dlgBoxes[0].FadeIn(2));
        seq.AppendInterval(3);
        seq.Append(dlgBoxes[1].FadeIn(.75f));
        seq.Append(dlgBoxes[2].FadeIn(.75f));
        seq.Append(dlgBoxes[3].FadeIn(.75f));
        seq.AppendInterval(1);
        seq.Append(dlgBoxes[4].FadeIn(2));

        seq.AppendCallback(() => continueBtn.SetActive(true));
        seq.AppendCallback(() => seq.Pause());
        seq.Append(dlgBoxes[0].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[1].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[2].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[3].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[4].FadeOut(clearSpeed));

        seq.Append(dlgBoxes[5].FadeIn(2));
        seq.AppendInterval(3);
        seq.Append(dlgBoxes[6].FadeIn(2));
        seq.AppendInterval(3);
        seq.Append(dlgBoxes[7].FadeIn(2));

        seq.AppendCallback(() => continueBtn.SetActive(true));
        seq.AppendCallback(() => seq.Pause());
        seq.Append(dlgBoxes[5].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[6].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[7].FadeOut(clearSpeed));

        seq.Append(dlgBoxes[8].FadeIn(2));
        seq.AppendInterval(1.5f);
        seq.Append(dlgBoxes[9].FadeIn(1.25f));
        seq.AppendInterval(.75f);
        seq.Append(dlgBoxes[10].FadeIn(1.25f));

        seq.AppendCallback(() => continueBtn.SetActive(true));
        seq.AppendCallback(() => seq.Pause());
        seq.Append(dlgBoxes[8].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[9].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[10].FadeOut(clearSpeed));

        seq.Append(dlgBoxes[11].FadeIn(2));
        seq.AppendInterval(2);
        seq.Append(dlgBoxes[12].FadeIn(2));

        seq.AppendCallback(() => continueBtn.SetActive(true));
        seq.AppendCallback(() => seq.Pause());

        seq.Append(dlgBoxes[11].FadeOut(clearSpeed));
        seq.Join(dlgBoxes[12].FadeOut(clearSpeed));
        #endregion

        var titleImg = titleObj.GetComponent<Image>();
        var teamNameImg = teamNameObj.GetComponent<TMP_Text>();
        var presentsImg = presentsObj.GetComponent<TMP_Text>();
        var jamImg = jamObj.GetComponent<TMP_Text>();

        seq.Append(teamNameImg.DOFade(1, 2));
        seq.Append(presentsImg.DOFade(1, 1.75f));
        seq.AppendInterval(1.5f);
        seq.Append(presentsImg.DOFade(0, 1.75f));
        seq.Append(jamImg.DOFade(1, 1.75f));
        seq.AppendInterval(1);
        seq.Append(jamImg.DOFade(0, 2));
        seq.Join(teamNameImg.DOFade(0, 2.5f * clearSpeed));
        seq.Append(titleImg.DOFade(1, 3).SetEase(Ease.InSine));
        seq.AppendCallback(() => { continueBtn.GetComponentInChildren<TMP_Text>().text = "Start"; });
        seq.AppendCallback(() => continueBtn.SetActive(true));
        seq.AppendCallback(() => seq.Pause());

        // seq.AppendCallback(() => Driver.GetInstance().mainCamera.backgroundColor = Color.black);
        seq.Append(bgObj.GetComponent<Image>().DOFade(0, 4));
        seq.Append(titleImg.DOFade(0, 2));
        seq.JoinCallback(() => audioManager.SetBGVolume(0, 4));
        seq.AppendInterval(2);
        seq.AppendCallback(() => Driver.GetInstance()?.SwitchScenes(JamScenes.BAR));
    }

    private List<IntroDlg> getVisibleDialogues() {
        return dlgBoxes.FindAll((IntroDlg dlg) => dlg.gameObject.activeSelf && dlg.alpha != 0);
    }

    private List<IntroDlg> getDialogues() {
        var list = new List<IntroDlg>();
        var scriptParent = GameObject.Find("Parent/Canvas/Script");
        list.AddRange(scriptParent.GetComponentsInChildren<IntroDlg>());
        foreach (Transform childT in scriptParent.transform) {
            childT.gameObject.SetActive(false);
        }
        list.ForEach((IntroDlg dlg) => dlg.alpha = 0);
        return list;
    }

    public void HitContinue() {
        var mode = continueBtn.GetComponentInChildren<TMP_Text>().text;
        Debug.Log("Continue mode: " + mode);

        continueBtn.SetActive(false);
        tweenSeq.Play();
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
