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
    {
        if (InventorySlots.Count < 1)
        {
            InventorySlots.Add(new InventorySlot(produce, 1));
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

    public void RemoveItem(SO_Produce produce, int amount)
    {

        if (InventorySlots.Count > 0)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                if (slot.produce == produce)
                {
                    slot.numberOfProduce = slot.numberOfProduce - amount;
                    return;
                }
            }
        }
    }
    public int CountItem(SO_Produce produce)
    {
        if (InventorySlots.Count > 0)
        {
            foreach (InventorySlot slot in InventorySlots)
            {
                if (slot.produce == produce)
                {
                   return slot.numberOfProduce;
                }
            }
        }
        return 0;
    }
}
