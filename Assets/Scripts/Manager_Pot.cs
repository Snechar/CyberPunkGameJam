using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotManager : MonoBehaviour
{
    [SerializeField]
    private SO_Plant plant;
    [SerializeField]
    private bool readyToHarvest;
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {

    }

    public void PlantSeed(Seed seed)
    {   
        
        
        if (plant != null)
            return;

        plant = Instantiate(seed.GetPlant());
        UpdateSprite();


    }

    private void UpdateSprite()
    {
        SpriteRenderer.sprite = plant.GetCurrentCycleSprite();
    }


    public void UpdateVeggieCycle()
    {
        if (plant.UpdateCycle())
        {
            readyToHarvest = true;
        }
        UpdateSprite();
    }



}
