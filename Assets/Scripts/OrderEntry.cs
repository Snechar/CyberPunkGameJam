using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderEntry : MonoBehaviour {
    public TMP_Text orderLabel;
    public RectTransform progressRemain;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private Request req;
    public void SetRequest(Request req) {
        this.req = req;
        UpdateProgress();
    }

    public void UpdateProgress() {
        var endCycle = req.neededByCycle;
        orderLabel.text = req.client;

        Debug.Log("req: " + req);
        Debug.Log("  neededByCycle: " + req.neededByCycle);
        Debug.Log("  totalCycles: " + req.totalCycles);

        var cyclesLeft = endCycle - Driver.GetInstance().CurrentCycleNumber();
        Debug.Log("  remainingCycles: " + cyclesLeft);

        if (cyclesLeft < 0) {
            orderLabel.color = Color.red;
            var scale = progressRemain.localScale;
            scale.x = 0;
            progressRemain.localScale = scale;
        } else {
            orderLabel.color = Color.white;
            var scale = progressRemain.localScale;
            scale.x = ((float)cyclesLeft) / req.totalCycles;
            progressRemain.localScale = scale;
        }
    }
}
