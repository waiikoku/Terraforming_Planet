using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building",menuName = "MyGame/Building")]
public class BuildingConstruction : ScriptableObject
{
    public enum BuildingType { Harvester , Storage , Generator, RandD , Work , Multifunction }

    public string buildingName = "Building";
    public Sprite buildingIcon;
    [TextArea]
    public string description;
    [Header("Setting")]
    public BuildingType bt;
    public int maximumHealth = 100;
    public int ShieldCapacity = 100;
    public int EnergyCapacity = 100;
    public bool isNeedWorker = false;

    public bool ConsumeElectricPower = true;
    public int ElectricPowerConsumption = 0;

    public bool ElectricityGeneration = false;
    public int GenerateElectricPower = 0;

    public int overloadConsume = 100;
    public int overloadGenerate = 100;

    public int maximumWorker = 16;
    public int level = 1;
    public int tier = 1;
    public GameObject prefab;

    [Header("Construction")]
    public NaturalResources[] require;
    public int[] cost;
    public float constructTime = 60f;
    public float constructPoint = 1000f;
}
