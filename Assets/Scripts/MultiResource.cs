using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiResource : MonoBehaviour
{
    public Slider slider;

    [Header("Recipe")]
    public ConstructRecipe[] recipes;

    [Header("Production Line")]
    public List<string> commands;
    public int Queued = 0;
    public bool StartAction = false;

    [Header("Production Settings")]
    public float currentPercentage = 0;
    public float currentTime = 0;
    public float productionPercentage = 100f;
    public List<float> productionTime;
    private float tempTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ConstructSomething(recipes[0]);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ConstructSomething(recipes[1]);
        }
        if (commands.Count > 0)
        {
            slider.value = currentPercentage / 100f;
            if (!StartAction)
            {
                StartAction = true;
                StartCoroutine("ProductionLine");
            }
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            commands.RemoveAt(commands.Count - 1);
            productionTime.RemoveAt(productionTime.Count - 1);
            Queued -= 1;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartAction = false;
            commands.Clear();
            productionTime.Clear();
            Queued = 0;
        }
    }

    public void ConstructSomething(ConstructRecipe cr)
    {
        if (CheckCondition(cr))
        {
            for (int i = 0; i < cr.nrProfile.Length; i++)
            {
                ResourceController.instance.FilterResource(cr.nrProfile[i]).nrAmount -= cr.requireAmount[i];
            }
            Debug.Log("Can Build :"+cr.crName);
            Queued++;
            commands.Add("Add "+cr.crName+" To Queue No."+Queued);
            productionTime.Add(cr.productionTime);
        }
        else
        {
            Debug.Log("Not Enough Resources");
        }
    }

    bool CheckCondition(ConstructRecipe recipe)
    {
        if (recipe.nrProfile.Length == 1)
        {
            if (ResourceController.instance.FilterResource(recipe.nrProfile[0]).nrAmount >= recipe.requireAmount[0])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if(recipe.nrProfile.Length > 1)
        {
            for (int i = 0; i < recipe.nrProfile.Length; i++)
            {
                if(ResourceController.instance.FilterResource(recipe.nrProfile[i]).nrAmount >= recipe.requireAmount[i])
                {

                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator ProductionLine()
    {
        while (StartAction)
        {
            if (currentTime < productionTime[0])
            {
                tempTime += Time.deltaTime;
                currentTime = tempTime;
            }
            if(currentPercentage < productionPercentage)
            {
                currentPercentage = (int)((currentTime / productionTime[0]) * 100);
            }
            else
            {
                Debug.Log("Finished One");
                ClearValue();
                commands.RemoveAt(0);
                productionTime.RemoveAt(0);
                Debug.Log("Start New One");
            }
            yield return null;
            if(commands.Count == 0)
            {
                StartAction = false;
                Queued = 0;
                commands.Clear();
            }
        }

    }

    private void ClearValue()
    {
        currentTime = 0;
        currentPercentage = 0;
        tempTime = 0;
        slider.value = 0;
    }
}
