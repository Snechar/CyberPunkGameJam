using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plantable : MonoBehaviour
{
    [SerializeField]
    private SO_Plant plant;
    [SerializeField]
    public Sprite Sprite;


    public SO_Plant GetPlant() { 
        return plant;
    }

    public Plantable GetThisSeed()
    {
        return this ;
    }
}
