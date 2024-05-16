using TMPro;
using UnityEngine;

public class OrderEntry : MonoBehaviour {
    public OrderManager orderManager;
    public TMP_Text orderLabel;
    public RectTransform progressRemain;
    public Request req {
        get; set;
    }

    public void SetRequest(Request req) {
        this.req = req;
        UpdateProgress();
    }

    public void UpdateProgress() {
        var endCycle = req.neededByCycle;
        orderLabel.text = req.client;
        if (orderLabel.text.Equals("sif")) {
            orderLabel.text = "Tychus";
        }
        if (orderLabel.text.Equals("rose")) {
            orderLabel.text = "Caitlyn";
        }
        var cyclesLeft = endCycle - Driver.GetInstance().CurrentCycleNumber();

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
    public void EntryMouseEnter() {
        orderManager?.EntryMouseEnter(this);
    }
    public void EntryMouseExit() {
        orderManager?.EntryMouseExit(this);
    }
}
