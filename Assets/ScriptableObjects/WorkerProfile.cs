using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Profile",menuName ="MyGame/Worker")]
public class WorkerProfile : ScriptableObject
{
    public enum BotType { Multifunction , Constructor , Harvester , Researcher , Explorer , Transporter }
    public BotType Type;

    [Header("Detail")]
    public string WorkerName;
    public GameObject Workerprefab;
    public Sprite WorkerImage;
    [TextArea]
    public string workerDescription;

    public string[] CommandList = { "Idle" , "Move" , "Follow" };

    [Header("Status")]
    public int maximumHealth = 100;
    public int shieldCapacity = 100;
    public int batteryCapacity = 100;

    public float movementSpeed = 1;
    public float turnSpeed = 1;

    [Header("Functions")]
    public bool Constructable = false;
    public float constructionSpeed;

    public bool Harvestable = false;
    public float harvestSpeed;
    public float harvestPerTime;

    public bool Researchable = false;
    public float researchSpeed;

    public bool Exploreable = false;
    public float explorationSpeed;

    public bool Transportable = false;
    public float transportSpeed;
    public float maximumWeight;
}
