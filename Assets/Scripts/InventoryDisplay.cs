using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour {
    public TMP_Text label;
    private InventoryManagerSingleton inv;

    private void Awake() {
        UpdateInventory();
    }

    public void UpdateInventory() {
        var inv = InventoryManagerSingleton.Instance;
        var str = "";
        foreach (var e in inv.InventorySlots) {
            var cnt = e.numberOfProduce;
            if (cnt == 0) {
                continue;
            }
            var type = e.produce.produceName;
            str += $"{cnt} x {type}\n";
        }
        if (str.Equals("")) {
            str = "Nothing in stock";
        }
        label.text = str;
    }
}
