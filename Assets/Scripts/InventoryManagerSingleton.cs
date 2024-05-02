using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerSingleton : MonoBehaviour
{

    public static InventoryManagerSingleton Instance { get; private set; }
    [SerializeField]
    public List<InventorySlot> InventorySlots = new List<InventorySlot>();

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void AddItem(SO_Produce produce)
    {   if (InventorySlots.Count < 1)
        {
            InventorySlots.Add(new InventorySlot(produce,1));
            return;
        }
        foreach (InventorySlot slot in InventorySlots)
        {
            if (slot.produce == produce)
            {
                slot.numberOfProduce++;
                return;
            }
        }
        InventorySlots.Add(new InventorySlot(produce, 1));
    }
}
