using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish/Fish")]
public class SO_Fish : ScriptableObject
{
    [SerializeField]
    private int timeToGrowCycles = 3;
    [SerializeField]
    private int currentCycle = 0;
    [SerializeField]
    private Sprite[] sprites = new Sprite[3];
    [SerializeField]
    private SO_Produce produce;
    public Sprite currentSprite = null;

    //implement end product : ex tomato

    public int GetCurrentCycle()
    {
        return currentCycle;
    }
    public SO_Produce GetProduce() { return produce; }

    public Sprite GetCurrentCycleSprite() {
        SetupSprite();
        return currentSprite; }

    public void SetupSprite()
    {
        currentSprite = sprites[currentCycle];
    }

    public bool UpdateCycle()
    {
        if (currentCycle + 1 >= timeToGrowCycles-1)
        {
            currentCycle++;
            currentSprite = sprites[currentCycle];
            return true;

        }
        currentCycle++;
        currentSprite = sprites[currentCycle];
        return false;
    }
}
