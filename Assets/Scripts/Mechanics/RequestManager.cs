using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RequestManager : MonoBehaviour {
    Dictionary<string, Request> requestBook;
    Dictionary<string, int> completedRequests;
    private string lastFilled;

    public RequestManager() {
        requestBook = new Dictionary<string, Request>();
        completedRequests = new Dictionary<string, int>();
    }

    public void DumpRequestBook() {
        var str = "Requests {\n";
        foreach (var entry in requestBook) {
            str += "    " + entry.Key + " {\n";
            foreach (var e in entry.Value.order) {
                str += "        " + e.Key + " x " + e.Value + "\n";
            }
            str += "    }\n";
        }
        Debug.Log(str + "}");
    }

    // Adds a request to the queue, returns false if the request was not possible to understand
    public void AddRequest(string client, string request, int days) {
        var curCycle = 0; // TODO: get global cycle number
        var newRequest = new Request(curCycle + days);
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
            ProduceName? produce = SO_Produce.NameFromString(parts[1]);
            if (produce == null) {
                Debug.Log($"Failed to process request {request}");
                return;
            } else {
                newRequest.AddItem((ProduceName)produce, count);
            }

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
        Debug.Log("HasAnyDelivery()");
        foreach (var req in requestBook.Values) {
            Debug.Log("Examining request: " + req);
            if (req.CanFill(InventoryManagerSingleton.Instance)) {
                return true;
            }
        }
        return false;
    }

    public bool HasPending(string client) {
        client = client.ToLower();
        return requestBook.ContainsKey(client);
    }

    public void FillRequest(string client) {
        client = client.ToLower();
        if (requestBook.ContainsKey(client)) {
            var rq = requestBook[client];
            rq.Fill(InventoryManagerSingleton.Instance);
            requestBook.Remove(client);
            lastFilled = client;
            if (completedRequests.ContainsKey(client)) {
                completedRequests[client] = completedRequests[client] + 1;
            } else {
                completedRequests[client] = 1;
            }
        } else {
            Debug.Log("No pending request for client");
        }
    }

    public int CountCompletedRequests(string client) {
        client = client.ToLower();
        if (completedRequests.ContainsKey(client)) {
            Debug.Log($"completedRequests({client}): " + completedRequests[client]);
            return completedRequests[client];
        }
        Debug.Log($"completedRequests({client}): " + 0);
        return 0;
    }

    public int CountCompletedRequests() {
        var count = 0;
        foreach (var v in completedRequests.Values) {
            count += v;
        }
        return count;
    }

    public bool WorkAvailable(string client) {
        client = client.ToLower();
        if (requestBook.ContainsKey(client)) {
            return false;
        }
        // can't get work immediately after filling a request
        return !client.Equals(lastFilled);
    }

    public string GetLastClient() {
        return lastFilled == null ? "" : lastFilled;
    }
}

class Request {
    public Dictionary<ProduceName, int> order;
    public int neededByCycle;

    public Request(int tgtCycle) {
        order = new Dictionary<ProduceName, int>();
        neededByCycle = tgtCycle;
    }

    public void AddItem(ProduceName produce, int count) {
        order.Add(produce, count);
    }

    public bool CanFill(InventoryManagerSingleton inv) {
        if (inv == null) {
            Debug.Log("Can't fill any requests without inventory");
            return false;
        }
        foreach (var e in order) {
            var produceName = e.Key;
            var needed = e.Value;
            if (inv.CountItem(produceName) < needed) {
                return false;
            }
        }

        return true;
    }

    public void Fill(InventoryManagerSingleton inv) {
        if (inv == null) {
            Debug.LogError("Attempting to fill a request with no inventory.");
        }
        foreach (var e in order) {
            inv.RemoveItem(e.Key, e.Value);
        }
    }
}