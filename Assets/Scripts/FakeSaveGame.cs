using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSaveGame : MonoBehaviour
{
    public int mineral = 0;
    public int gas = 0;

    public NaturalResources NR_Mineral;
    public NaturalResources NR_Gas;

    private void Start()
    {
        NR_Mineral.nrAmount = mineral;
        NR_Gas.nrAmount = gas;
    }
}
