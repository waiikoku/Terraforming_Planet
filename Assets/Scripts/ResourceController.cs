using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public static ResourceController instance;

    public List<NaturalResources> nrProfile;
    
    public SupplyUnit suu;

    public List<Storage> storage;
    public Transform[] storageTransform;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        suu.CurrentSupply = Counting();

        /*
        UIController.instance.mineralTxt.text = nrProfile[0].nrAmount.ToString();
        UIController.instance.gasTxt.text = nrProfile[1].nrAmount.ToString();
        UIController.instance.foodTxt.text = foodsunit.currentfoods + "/" + foodsunit.maximumfoods;
        */
        if (UIController.instance.TMPro_Mineral.text != CustomFunction.ValueToShortText(nrProfile[0].nrAmount, null))
        {
            UIController.instance.TMPro_Mineral.text = CustomFunction.ValueToShortText(nrProfile[0].nrAmount, null);
        }
        if (UIController.instance.TMPro_Metal.text != CustomFunction.ValueToShortText(nrProfile[1].nrAmount, null))
        {
            UIController.instance.TMPro_Metal.text = CustomFunction.ValueToShortText(nrProfile[1].nrAmount, null);
        }
        if (UIController.instance.TMPro_Supply.text != suu.CurrentSupply.ToString() + "/" + suu.SupplyCapacity.ToString())
        {
            UIController.instance.TMPro_Supply.text = suu.CurrentSupply.ToString() + "/" + suu.SupplyCapacity.ToString();
        }
        if (suu.SupplyCapacity != storage.Count * 10)
        {
            suu.SupplyCapacity = storage.Count * 10;
        }
        if (storage.Count > 0)
        {
            for (int s = 0; s < storage.Count; s++)
            {
                if(storage[s] == null)
                {
                    storage.RemoveAt(s);
                    //Debug.Log("Return After Remove Storage");
                    return;
                }

            }
            if(storageTransform.Length != storage.Count)
            {
                storageTransform = new Transform[storage.Count];
                for (int i = 0; i < storageTransform.Length; i++)
                {
                    storageTransform[i] = storage[i].transform;
                }
                //Debug.Log("Assign Storage Transform");
            }
 
        }
    }

    public NaturalResources FilterResource(NaturalResources nr)
    {
        NaturalResources correctOne;
        if (nrProfile.Contains(nr))
        {
            correctOne = nrProfile[nrProfile.IndexOf(nr)];
        }
        else
        {
            correctOne = null;
        }
        return correctOne;
    }

    private int Counting()
    {
        int result = new int();
        EmbeddedSeal[] ems = FindObjectsOfType<EmbeddedSeal>();
        for (int i = 0; i < ems.Length; i++)
        {
            result += ems[i].fsuSize;
        }
        return result;
    }
}
