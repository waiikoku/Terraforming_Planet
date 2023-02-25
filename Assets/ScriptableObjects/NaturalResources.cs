using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "res",menuName = "MyGame/NaturalResources",order = 0)]
public class NaturalResources : ScriptableObject
{
    public string nrName;
    public int nrAmount;
    public Sprite nrImage;
    public Texture2D nrTexture;
    public GameObject nrPrefab;
    [TextArea]
    public string description;
}
