using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotManager : MonoBehaviour
{
    [SerializeField]
    private SO_Plant plant;
    [SerializeField]
    private bool readyToHarvest = false;
    [SerializeField]
    private SpriteRenderer[] SpriteRenderer = new SpriteRenderer[0];

    private void Start()
    {

    }
    public bool GetHarvestStatus()
    {
        return readyToHarvest;
    }

    public void PlantSeed(Plantable seed)
    {   
        
        
        if (plant != null)
            return;

        plant = Instantiate(seed.GetPlant());
        UpdateSprite();


    }

    private void UpdateSprite()
    {
        foreach (var sprite in SpriteRenderer)
        {
            sprite.sprite = plant.GetCurrentCycleSprite();
        }

    }

    public SO_Produce Harvest()
    {
        Debug.Log("Harvest Has Been Attempted");
        foreach (var sprite in SpriteRenderer)
        {
            sprite.sprite = null;
        }
        var throwAway = plant;
        plant = null;
        readyToHarvest = false;
        return throwAway.GetProduce();
    } 



    public void UpdateVeggieCycle()
    {
        if (plant == null) return;
        if (plant.UpdateCycle())
        {
            Debug.Log("Plant Has Fully Grown");
            readyToHarvest = true;
            UpdateSprite();
        }
        UpdateSprite();
    }



}
