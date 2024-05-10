using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour {
    public GameObject orderPrefab;
    public GameObject[] orderEntries;
    List<Request> clientOrders;

    RectTransform rect;

    private void Awake() {
        clientOrders = new List<Request>();
        rect = GetComponent<RectTransform>();
    }

    public void Tick() {
        for (var i = 0; i < clientOrders.Count; i++) {
            var go = orderEntries[i].GetComponent<OrderEntry>();
            go.UpdateProgress();
        }
    }

    public void SyncOrders(RequestManager reqMgr) {
        Debug.Log("SyncOrders");
        clientOrders.Clear();
        foreach (var req in reqMgr.Orders()) {
            // Debug.Log(req);
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
}
