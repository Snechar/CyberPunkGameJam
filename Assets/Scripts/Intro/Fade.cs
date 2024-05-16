using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
    public bool FadeIn;
    public Image target;
    public float time;

    public bool WaitTilDone;
    private bool done = false;

    private int getTargetAlpha() {
        return FadeIn ? 1 : 0;
    }

    private IEnumerator AlphaOverTime() {
        float elapsed = 0;
        var initialColor = target.color;
        var tgtAlpha = getTargetAlpha();

        while (elapsed < time) {
            var newAlpha = Mathf.Lerp(initialColor.a, tgtAlpha, elapsed / time);
            var newColor = initialColor;
            newColor.a = newAlpha;
            target.color = newColor;
            yield return null;
            elapsed += Time.deltaTime;
        }
        initialColor.a = tgtAlpha;
        done = true;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Play() {
        gameObject.SetActive(true);
        StartCoroutine(AlphaOverTime());
    }

    public bool IsDone(int hasWaitedMS) {
        return !WaitTilDone || done;
    }

    public bool WaitsForInput() {
        return false;
    }

    public bool ShouldDeactivatePrevious() {
        return false;
    }
}
