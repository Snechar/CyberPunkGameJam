using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour {
    public GameObject orderPrefab;
    public GameObject[] orderEntries;
    List<Request> clientOrders;

    public GameObject orderDetailsParent;
    public TMP_Text orderDetailsText;
    public RectTransform orderDetailsBG;

    private Vector2 originalPos = new Vector2(-100, 0);


    RectTransform rect;

    private void Awake() {
        clientOrders = new List<Request>();
        rect = GetComponent<RectTransform>();
        originalPos = orderDetailsBG.position;
    }

    public void Tick() {
        for (var i = 0; i < clientOrders.Count; i++) {
            var go = orderEntries[i].GetComponent<OrderEntry>();
            go.UpdateProgress();
        }
    }

    public void SyncOrders(RequestManager reqMgr) {
        clientOrders.Clear();
        foreach (var req in reqMgr.Orders()) {
            clientOrders.Add(req);
        }
        clientOrders.Sort((Request x, Request y) => x.RemainingCycles() - y.RemainingCycles());

        var i = 0;
        for (; i < clientOrders.Count; i++) {
            var req = clientOrders[i];
            orderEntries[i].SetActive(true);
            var entry = orderEntries[i].GetComponent<OrderEntry>();
            entry.SetRequest(req);
        }
        for (; i < orderEntries.Length; i++) {
            orderEntries[i].SetActive(false);
        }

        var sd = rect.sizeDelta;
        sd.y = 15 + (30 * clientOrders.Count);
        rect.sizeDelta = sd;
    }

    public void UpdateSizing(int idx, int lineCount) {
        var newHt = (lineCount - 1) * 20;
        var sd = orderDetailsBG.sizeDelta;
        sd.y = newHt;
        orderDetailsBG.sizeDelta = sd;

        var newPos = originalPos;
        orderDetailsBG.position = newPos;
        orderDetailsBG.Translate(0, idx * -23, 0);
    }

    public void EntryMouseEnter(OrderEntry entry) {
        var index = clientOrders.FindIndex((Request req) => req.client.Equals(entry.req.client));
        var str = "";
        var cnt = 0;
        foreach (var item in entry.req.order) {
            str += item.Value + " x " + item.Key + "\n";
            cnt++;
        }
        orderDetailsText.text = str;
        UpdateSizing(index, cnt);
        orderDetailsParent.SetActive(true);
    }
    public void EntryMouseExit(OrderEntry entry) {
        var index = clientOrders.FindIndex((Request req) => req.client.Equals(entry.req.client));
        var str = "";
        orderDetailsText.text = str;
        orderDetailsParent.SetActive(false);
    }
}
