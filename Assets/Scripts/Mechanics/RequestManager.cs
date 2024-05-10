using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour {
    Dictionary<string, Request> requestBook;
    Dictionary<string, int> completedRequests;
    private string lastFilled;

    public RequestManager() {
        requestBook = new Dictionary<string, Request>();
        completedRequests = new Dictionary<string, int>();
    }

    public IEnumerable<Request> Orders() {
        return requestBook.Values;
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

    public int Count() {
        return requestBook.Count;
    }

    // Adds a request to the queue, returns false if the request was not possible to understand
    public void AddRequest(string client, string request, int days) {
        var curCycle = Driver.GetInstance().CurrentCycleNumber();
        var newRequest = new Request(client, days, curCycle + days);
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
        Driver.GetInstance()?.SyncOrders(this);
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
        Driver.GetInstance()?.SyncOrders(this);
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

public class Request {
    public string client { get; }
    public Dictionary<ProduceName, int> order;
    // how many days did we have at the beginning of this
    public int totalCycles;
    // what is the day that we need this by?
    public int neededByCycle;

    public Request(string forClient, int totalCycles, int tgtCycle) {
        this.client = forClient;
        order = new Dictionary<ProduceName, int>();
        this.totalCycles = totalCycles;
        neededByCycle = tgtCycle;
    }

    public override bool Equals(object obj) {
        if (obj is Request) {
            return ((Request)obj)?.client?.Equals(client) ?? false;
        }
        return false;
    }

    public override string ToString() {
        return $"Request({client} by {neededByCycle})";
    }

    public override int GetHashCode() {
        return client.GetHashCode();
    }

    public int RemainingCycles() {
        return neededByCycle - (Driver.GetInstance()?.CurrentCycleNumber() ?? 0);
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