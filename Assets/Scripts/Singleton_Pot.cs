using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton_Pot : MonoBehaviour
{
    public static Singleton_Pot Instance { get; private set; }

    public PotManager[] allPots = new PotManager[0];

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
        }
    }

    private void Start()
    {
      allPots =  GameObject.FindObjectsOfType(typeof(PotManager)) as PotManager[];
    }

    public void NextCycle()
    {
        foreach (var pot in allPots)
        {
            pot.UpdateVeggieCycle();
        }
    }
}
