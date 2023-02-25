using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasField : MonoBehaviour
{
    public OreNode orenode;
    public int CurrentHarvester = 0;
    public int MaximumHarvester = 4;
    public int Remaining = 1000;
    [SerializeField] private GameObject gas;
    [SerializeField] private NaturalResources nrp;

    private void Start()
    {
        //orenode.currentAmount = Remaining;
        orenode.ore = gas;
        orenode.nr = nrp;
        if (Remaining > 0)
        {
            orenode.CanHarvest = true;
        }
    }

    private void Update()
    {
        Remaining = orenode.currentAmount;
        if (Remaining > 0)
        {
            orenode.CanHarvest = true;
        }
        if (Remaining <= 0)
        {
            orenode.CanHarvest = false;
            Destroy(gameObject, 1f);
        }
    }

    public void AssignHarvester()
    {
        CurrentHarvester += 1;
    }

    public void DeassignHarvester()
    {
        CurrentHarvester -= 1;
    }
}
