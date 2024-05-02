using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventorySlot 
{
    [SerializeField]
    public SO_Produce produce;
    [SerializeField]
    public int numberOfProduce;

   public InventorySlot(SO_Produce produce, int numberOfProduce)
    {
        this.produce = produce;
        this.numberOfProduce = numberOfProduce;
    }

    override public String ToString() {
        return produce.getName() + " x {numberOfProduce}";
    }
}
