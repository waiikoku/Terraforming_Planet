using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Research",menuName = "MyGame/ResearchFormula")]
public class ResearchFormula : ScriptableObject
{
    public string topic = "Research Topic";
    public int TechnologyTier = 1;
    public NaturalResources[] nr;
    public int[] cost;
    public float researchingTime = 60;
    public string effect = "Null";
}
