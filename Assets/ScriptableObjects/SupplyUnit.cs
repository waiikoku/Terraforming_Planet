using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Supply",menuName = "MyGame/Foods")]
public class SupplyUnit : ScriptableObject
{
    public string define;
    public int CurrentSupply;
    public int SupplyCapacity;
}
