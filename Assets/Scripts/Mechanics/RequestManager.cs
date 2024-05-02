using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RequestManager : MonoBehaviour {
    Dictionary<string, Request> requestBook;
    Dictionary<string, int> completedRequests;

    public RequestManager() {
        requestBook = new Dictionary<string, Request>();
        completedRequests = new Dictionary<string, int>();
    }

    void DumpRequestBook() {
        var str = "Requests {\n";
        foreach (var entry in requestBook) {
            str += "    " + entry.Key + " {";
            foreach (var e in entry.Value.order) {
                str += "        " + e.Key + " x " + e.Value + "\n";
            }
        }
        Debug.Log(str + "}");
    }

    // Adds a request to the queue, returns false if the request was not possible to understand
    public void AddRequest(string client, string request, int days) {
        var newRequest = new Request();
        var elements = request.Split(';');
        if (elements.Length < 1) {
            Debug.Log($"Failed to process request {request}");
            return;
        }
        foreach (string ele in request.Split(';')) {
            var parts = ele.Split(':');
            if (parts.Length != 2) {
                Debug.Log($"Failed to process request {request}");
                return;
            }
            
            int count = int.Parse(parts[0]);
            SO_Produce produce = SO_Produce.FromString(parts[1]);
            if (produce == null) {
                Debug.Log($"Failed to process request {request}");
                return;
            }
            
            newRequest.AddItem(produce, count);
        }
        requestBook.Add(client.ToLower(), newRequest);
        DumpRequestBook();
    }

    public bool CanFill(string client) {
        client = client.ToLower();
        if (!requestBook.ContainsKey(client)) {
            return false;
        }
        return requestBook[client].CanFill(InventoryManagerSingleton.Instance);
    }

    public bool HasAnyDelivery() {
        foreach (var req in requestBook.Values) {
            if (req.CanFill(InventoryManagerSingleton.Instance)) {
                return true;
            }
        }
        return false;
    }

    public void FillRequest(string client) {
        client = client.ToLower();
        if (requestBook.ContainsKey(client)) {
            var rq = requestBook[client];
            rq.Fill(InventoryManagerSingleton.Instance);
        } else {
            Debug.Log("No pending request for client");
        }
    }

    public int CountRquestsFilled(string client) {
        client = client.ToLower();
        if (completedRequests.ContainsKey(client)) {
            return completedRequests[client];
        }
        return 0; 
    }
}

class Request {
    public Dictionary<SO_Produce, int> order;
    public void AddItem(SO_Produce produce, int count) {
        order.Add(produce, count);
    }

    public bool CanFill(InventoryManagerSingleton inv) {
        foreach (var e in order) {
            var produceType = e.Key;
            var needed = e.Value;
            // if (inv.Count(produceType) < needed) {
                // return false;
            // }
        }

        return true;
    }

    public void Fill(InventoryManagerSingleton inv) {
        foreach (var e in order) {
            // inv.Remove(e.Key, e.Value);
        }
    }
}