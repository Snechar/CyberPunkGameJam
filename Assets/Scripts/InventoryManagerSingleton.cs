using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerSingleton : MonoBehaviour {
    public InventoryDisplay display;

    public static InventoryManagerSingleton Instance {
        get; private set;
    }
    [SerializeField]
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();
    public List<SO_Produce> sO_Produces = new List<SO_Produce>();

    public void DebugLog() {
        var str = "Inventory:\n";
        foreach (var slot in InventorySlots) {
            str += "  " + str.ToString() + "\n";
        }
        Debug.Log(str);
    }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void AddItem(SO_Produce produce) {
        if (InventorySlots.Count < 1) {
            InventorySlots.Add(new InventorySlot(produce, 1));
            updateDisplay();
            return;
        }
        foreach (InventorySlot slot in InventorySlots) {
            if (slot.produce == produce) {
                slot.numberOfProduce++;
                updateDisplay();
                return;
            }
        }
        InventorySlots.Add(new InventorySlot(produce, 1));
        updateDisplay();
    }

    public bool RemoveItem(SO_Produce produce, int amount) {
        if (InventorySlots.Count > 0) {
            foreach (InventorySlot slot in InventorySlots) {
                if (slot.produce == produce) {
                    if (slot.numberOfProduce - amount < 0) {
                        return false;
                    }
                    slot.numberOfProduce = slot.numberOfProduce - amount;
                    updateDisplay();
                    return true;
                }
            }
        }
        return false;
    }
    public int CountItem(SO_Produce produce) {
        if (InventorySlots.Count > 0) {
            foreach (InventorySlot slot in InventorySlots) {
                if (slot.produce == produce) {
                    return slot.numberOfProduce;
                }
            }
        }
        return 0;
    }

    public int CountItem(ProduceName tgt) {
        var count = 0;
        foreach (var slot in InventorySlots) {
            if (slot.produce.produceName.Equals(tgt)) {
                count += slot.numberOfProduce;
            }
        }
        return count;
    }

    public void RemoveItem(ProduceName tgt, int count) {
        foreach (var slot in InventorySlots) {
            if (tgt.Equals(slot.produce.produceName)) {
                slot.numberOfProduce -= count;
            }
        }
        updateDisplay();
    }

    public SO_Produce ReturnProduceByName(ProduceName name) {
        foreach (var item in sO_Produces) {
            if (item.produceName == name)
                return item;
        }
        return null;
    }

    private void updateDisplay() {
        if (display != null) {
            display.UpdateInventory();
        }
    }
}
