using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plants/Plant")]
public class SO_Plant : ScriptableObject
{
    [SerializeField]
    private int timeToGrowCycles = 3;
    [SerializeField]
    private int currentCycle = 0;
    [SerializeField]
    private Sprite[] sprites = new Sprite[3];
    [SerializeField]
    private SO_Produce produce;
    public ProduceType ProduceType;
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
            currentCycle = timeToGrowCycles - 1;
            currentSprite = sprites[currentCycle];
            return true;

        }
        else {
            currentSprite = sprites[currentCycle];
            currentCycle++;
            return false;
        }

    }
}
