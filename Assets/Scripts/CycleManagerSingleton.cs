using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CycleManagerSingleton : MonoBehaviour {

    public static CycleManagerSingleton Instance {
        get; private set;
    }

    public Singleton_Pot Singleton_Pot;
    [SerializeField]
    private int currentCycle = 0;
    private void Awake() {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public int GetCurrentCycle() {
        return currentCycle;
    }

    public void NextCycle() {
        Singleton_Pot.NextCycle();
        currentCycle++;
        Driver.GetInstance()?.Tick();
    }
}
