using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteContainer : MonoBehaviour
{
    public static SpriteContainer instance;
    public Sprite[] sprite;

    private void Start()
    {
        instance = this;
    }
}
