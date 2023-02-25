using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="recipe",menuName ="MyGame/Recipe")]
public class ConstructRecipe : ScriptableObject
{
    public string crName = "Structure";
    public NaturalResources[] nrProfile;
    public int[] requireAmount;
    public float productionTime = 0;
    public int unitSize = 1;
    public GameObject product;
}
