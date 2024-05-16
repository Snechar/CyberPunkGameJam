using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour {
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
        seq.Append(dlgBoxes[9].FadeIn(1f));
        seq.AppendInterval(.75f);
        seq.Append(dlgBoxes[10].FadeIn(1f));

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

        seq.Append(bgObj.GetComponent<Image>().DOFade(0, 4));
        seq.Append(titleImg.DOFade(0, 2));
        seq.JoinCallback(() => audioManager.SetBGVolume(0, 4));
        seq.AppendInterval(2);
        seq.AppendCallback(() => audioManager.Stop());
        seq.AppendCallback(() => Driver.GetInstance()?.SwitchScenes(JamScenes.BAR));
    }

    private List<IntroDlg> getVisibleDialogues() {
        return dlgBoxes.FindAll((IntroDlg dlg) => dlg.gameObject.activeSelf && dlg.alpha != 0);
    }

    private List<IntroDlg> getDialogues() {
        var list = new List<IntroDlg>();
        var scriptParent = GameObject.Find("Parent/Canvas/Script");
        list.AddRange(scriptParent.GetComponentsInChildren<IntroDlg>());
        list.ForEach((IntroDlg dlg) => dlg.alpha = 0);
        foreach (Transform childT in scriptParent.transform) {
            childT.gameObject.SetActive(false);
        }
        return list;
    }

    public void HitContinue() {
        continueBtn.SetActive(false);
        tweenSeq.Play();
    }

    public void HitSkip() {
        float fadeSpeed = 1;
        
        tweenSeq.Kill();

        var seq = DOTween.Sequence();

        foreach (var dlg in dlgBoxes) {
            if (dlg.gameObject.activeSelf) {
                seq.Join(dlg.FadeOut(fadeSpeed));
            }
        }

        if (titleObj.activeSelf) {
            var titleImg = titleObj.GetComponent<Image>();
            seq.Join(titleImg.DOFade(0, fadeSpeed));
        }
        if (teamNameObj.activeSelf) {
            var teamNameImg = teamNameObj.GetComponent<TMP_Text>();
            seq.Join(teamNameImg.DOFade(0, fadeSpeed));
        }
        if (presentsObj.activeSelf) {
            var presentsImg = presentsObj.GetComponent<TMP_Text>();
            seq.Join(presentsImg.DOFade(0, fadeSpeed));
        }
        if (jamObj.activeSelf) {
            var jamImg = jamObj.GetComponent<TMP_Text>();
            seq.Join(jamImg.DOFade(0, fadeSpeed));
        }
        seq.JoinCallback(() => Driver.GetInstance()?.GetAudioManager().SetBGVolume(0, fadeSpeed));
        seq.AppendCallback(() => Driver.GetInstance()?.GetAudioManager().Stop());
        seq.AppendCallback(() => Driver.GetInstance()?.SwitchScenes(JamScenes.BAR));
    }
}
