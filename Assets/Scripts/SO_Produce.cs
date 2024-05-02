using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ProduceType
{
    Plant,
    Fish
}
[CreateAssetMenu(menuName = "Plants/Produce")]
public class SO_Produce : ScriptableObject
{
    public int ID;
    public ProduceType produceType;
    public Sprite Sprite;
    public float Price;
}
