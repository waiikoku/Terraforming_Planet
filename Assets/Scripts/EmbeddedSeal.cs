using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbeddedSeal : MonoBehaviour
{
    public ConstructRecipe esProfile;
    public SupplyUnit suu;
    public int fsuSize;

    private void Start()
    {
        fsuSize = esProfile.unitSize;
    }
}
