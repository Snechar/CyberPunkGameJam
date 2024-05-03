using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public string getName() {
        switch (produceType) {
            case ProduceType.Plant:
                return "Plant";
            case ProduceType.Fish:
                return "Fish";
        }
        return "Unknown";
    }

    public static ProduceName? NameFromString(string desc) {
        desc = desc.ToLower();
        switch (desc) {
            case "tom":
            case "tomato":
                return ProduceName.tomato;
            case "egg":
            case "eggplant":
                return ProduceName.eggplant;
            case "cuke":
            case "cucumber":
                return ProduceName.cucumber;
            case "let":
            case "lettuce":
                return ProduceName.lettuce;
            case "salmon":
                return ProduceName.salmon;
            case "cod":
                return ProduceName.cod;
            case "til":
            case "tilapia":
                return ProduceName.tilapia;
        }
        return null;
    }
}
