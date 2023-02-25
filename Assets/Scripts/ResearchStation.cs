using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchStation : MonoBehaviour
{
    public List<string> UpgradeList;

    public void Researching(string point)
    {
        if (UpgradeList.Contains(point))
        {
            Debug.Log("Upgrade : " + UpgradeList[UpgradeList.IndexOf(point)]);
        }
    }
}
