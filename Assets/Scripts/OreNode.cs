using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreNode : MonoBehaviour
{
    public enum Richness { Poor, Normal, Rich }
    public Richness rn;
    public bool RandomRichness = false;
    public string description;
    public int currentAmount;
    public bool CanHarvest = false;
    public GameObject ore;
    public NaturalResources nr;
    private void Start()
    {
        if (RandomRichness)
        {
            rn = (Richness)Random.Range(0, 2);
        }
        switch (rn)
        {
            case Richness.Poor:
                currentAmount = Random.Range(1, 100);
                break;
            case Richness.Normal:
                currentAmount = Random.Range(100, 1000);
                break;
            case Richness.Rich:
                currentAmount = Random.Range(1000, 10000);
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (description == "" || description == null)
        {
            description = nr.description;
        }
        if (currentAmount > 0)
        {
            if (!CanHarvest)
            {
                CanHarvest = true;
            }
        }
    }

    public void Harvesting(int amount, Transform holder)
    {
        if (currentAmount > 0)
        {
            if (currentAmount - amount >= 0)
            {
                currentAmount -= amount;
                GameObject mineral = Instantiate(ore, holder);
                mineral.GetComponent<localOre>().localAmount = amount;
                return;
            }
            if (currentAmount - amount < 0)
            {
                GameObject mineral = Instantiate(ore, holder);
                mineral.GetComponent<localOre>().localAmount = currentAmount;
                currentAmount -= currentAmount;
                Debug.Log("Depleted");
            }
        }
    }
}
