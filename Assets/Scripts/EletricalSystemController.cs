using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricalSystemController : MonoBehaviour
{
    public static EletricalSystemController instance;
    public List<GameObject> electronicalMachine;
    [SerializeField] private List<float> pslist;
    [SerializeField] private List<float> pdlist;
    public float powerSupply;
    public float powerDemand;
    public float currentPower;
    public bool ExceedPower = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (electronicalMachine.Count > 0)
        {
            for (int i = 0; i < electronicalMachine.Count; i++)
            {
                if (electronicalMachine[i] == null)
                {
                    electronicalMachine.RemoveAt(i);
                    if (pslist.Count >= i)
                    {
                        pslist.RemoveAt(i);
                    }
                    if (pdlist.Count >= i)
                    {
                        pdlist.RemoveAt(i);
                    }
                }
                else
                {
                    if (electronicalMachine[i].GetComponent<EletricalSystem>())
                    {
                        if (electronicalMachine[i].GetComponent<EletricalSystem>().EG)
                        {
                            if (electronicalMachine[i].GetComponent<BuildingState>().buildingCompleted)
                            {
                                //Debug.Log(electronicalMachine[i].name + ": Generate Power");
                                if (pslist.Count < i + 1)
                                {
                                    //Debug.Log("power supply add new value");
                                    pslist.Add(electronicalMachine[i].GetComponent<EletricalSystem>().currentGEP);
                                }
                                else
                                {
                                    //Debug.Log("power supply change value");
                                    pslist[i] = electronicalMachine[i].GetComponent<EletricalSystem>().currentGEP;
                                }
                            }
                            else
                            {
                                if(pslist.Count < i + 1)
                                {
                                    pslist.Add(0);
                                }
                            }
                        }
                        if (electronicalMachine[i].GetComponent<EletricalSystem>().CEP)
                        {
                            if (electronicalMachine[i].GetComponent<BuildingState>().buildingCompleted)
                            {
                                //Debug.Log(electronicalMachine[i].name + ": Consume Power");
                                if (pdlist.Count < i + 1)
                                {
                                    //Debug.Log("power demand add new value");
                                    pdlist.Add(electronicalMachine[i].GetComponent<EletricalSystem>().currentEPC);
                                }
                                else
                                {
                                    //Debug.Log("power demand change value");
                                    pdlist[i] = electronicalMachine[i].GetComponent<EletricalSystem>().currentEPC;
                                }
                            }
                            else
                            {
                                if (pdlist.Count < i + 1)
                                {
                                    pdlist.Add(0);
                                }
                            }
                        }
                    }
                }
                CalculatePower();
            }
        }
        /*
        UIController.instance.PowerSupply.text = powerSupply.ToString();
        UIController.instance.PowerDemand.text = powerDemand.ToString();
        */
        if (currentPower != powerSupply - powerDemand)
        {
            currentPower = powerSupply - powerDemand;
        }
        if (UIController.instance.TMPro_Electricity.text != CustomFunction.ValueToShortText(currentPower, "W"));
        {
            UIController.instance.TMPro_Electricity.text = CustomFunction.ValueToShortText(currentPower, "W");
        }
        if (powerDemand > powerSupply)
        {
            if (!ExceedPower)
            {
                ExceedPower = true;
            }
        }
        else
        {
            if (ExceedPower)
            {
                ExceedPower = false;
            }
        }
    }

    private void CalculatePower()
    {
        powerSupply = sumFloat(pslist);
        powerDemand = sumFloat(pdlist);
    }

    private float sumFloat(List<float> value)
    {
        float result = new float();
        for (int i = 0; i < value.Count; i++)
        {
            result += value[i];
        }
        return result;
    }

    public void AssignGenerator(GameObject go)
    {
        if (!electronicalMachine.Contains(go))
        {
            electronicalMachine.Add(go);
            Debug.Log("Added Generator!");
        }
    }

    public void DeassignGenerator()
    {

    }
}
