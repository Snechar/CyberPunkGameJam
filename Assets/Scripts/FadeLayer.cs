using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeLayer : MonoBehaviour {
    [SerializeField] bool startFadedOut = false;
    private Image fadeTarget;

    private void Awake() {
        fadeTarget = GetComponent<Image>();
        var initialColor = fadeTarget.color;
        initialColor.a = startFadedOut ? 0 : 1;
        fadeTarget.color = initialColor;
    }

    public IEnumerator ChangeAlphaOverTime(float alpha, float time) {
        var curAlpha = fadeTarget.color.a;
        float elapsed = 0f;

        var newColor = fadeTarget.color;
        
        while (elapsed < time) {
            var factor = elapsed / time;
            var newAlpha = Mathf.Lerp(curAlpha, alpha, factor);
            newColor.a = newAlpha;
            fadeTarget.color = newColor;
            yield return null;
            elapsed += Time.deltaTime;
        }
        newColor.a = alpha;
        fadeTarget.color = newColor;
    }
}
