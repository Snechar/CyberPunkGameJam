using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProduceType
{
    Plant,
    Fish
}

public enum ProduceName
{
    tomato,
    eggplant,
    cucumber,
    lettuce,
    salmon,
    cod,
    tilapia
}
[CreateAssetMenu(menuName = "Plants/Produce")]
public class SO_Produce : ScriptableObject
{
    public int ID;
    public ProduceType produceType;
    public ProduceName produceName;
    public Sprite Sprite;
    public float Price;
}
