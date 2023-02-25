using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tech",menuName ="MyGame/Upgrading")]
public class UpgradeTech : ScriptableObject
{
    public string upgradeName = "Tech";
    public NaturalResources[] nr;
    public int[] cost;
    public float upgradeTime = 60;
    public string result = "Nothing";
}
