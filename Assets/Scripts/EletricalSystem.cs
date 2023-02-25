using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EletricalSystem : MonoBehaviour
{
    public BuildingState bs;

    public bool CEP;
    public float currentEPC;
    public float ElectricPowerConsumption;

    public bool EG;
    public float currentGEP;
    public float GenerateElectricPower;

    public float overloadConsume;
    public float overloadGenerate;
    public float FailureProgress = 0;
    public float MaximumFailure = 100;

    public bool Switch = false;
    public bool Activate = false;
    public bool overload = false;

    [SerializeField] private UnityAction Btn_Switch;
    [SerializeField] private UnityAction Btn_Salvage;
    private bool addFunction1 = false;
    private bool addFunction2 = false;
    private void Start()
    {
        if (bs.bcprofile.ConsumeElectricPower)
        {
            CEP = bs.bcprofile.ConsumeElectricPower;
            ElectricPowerConsumption = bs.bcprofile.ElectricPowerConsumption;
        }
        if (bs.bcprofile.ElectricityGeneration)
        {
            EG = bs.bcprofile.ElectricityGeneration;
            GenerateElectricPower = bs.bcprofile.GenerateElectricPower;
        }
        overloadConsume = bs.bcprofile.overloadConsume;
        overloadGenerate = bs.bcprofile.overloadGenerate;
        Btn_Switch += ActivateSwitch;
        Btn_Salvage += SelfDestruction;
    }

    private void Update()
    {
        if (!EletricalSystemController.instance.electronicalMachine.Contains(gameObject))
        {
            EletricalSystemController.instance.AssignGenerator(gameObject);
        }
        if (bs.buildingCompleted)
        {
            if (GetComponent<Selection>().BeingSelected)
            {
                if (!addFunction1)
                {
                    addFunction1 = true;
                    if (UIController.instance.odp_power.GetComponent<ButtonSwitch>())
                    {
                        UIController.instance.odp_power.GetComponent<ButtonSwitch>().SetBool(Switch);
                    }
                    UIController.instance.odp_power.onClick.AddListener(Btn_Switch);
                }
                if (!addFunction2)
                {
                    addFunction2 = true;
                    UIController.instance.odp_salvage.onClick.AddListener(Btn_Salvage);
                }
                if (Activate)
                {
                    if (UIController.instance.odp_power.GetComponent<ButtonSwitch>())
                    {
                        if (!UIController.instance.odp_power.GetComponent<ButtonSwitch>().VerifyBool())
                        {
                            UIController.instance.odp_power.GetComponent<ButtonSwitch>().SetBool(Activate);
                        }

                    }
                    /*
                    UIController.instance.switchForButton.transform.GetComponent<Image>().color = Color.green;
                    UIController.instance.switchForButton.value = 1;
                    */
                }
                else
                {
                    if (UIController.instance.odp_power.GetComponent<ButtonSwitch>())
                    {
                        if (UIController.instance.odp_power.GetComponent<ButtonSwitch>().VerifyBool())
                        {
                            UIController.instance.odp_power.GetComponent<ButtonSwitch>().SetBool(Activate);
                        }

                    }
                    /*
                    UIController.instance.switchForButton.transform.GetComponent<Image>().color = Color.red;
                    UIController.instance.switchForButton.value = 0;
                    */
                }
            }
            else
            {
                if (addFunction1)
                {
                    addFunction1 = false;
                    UIController.instance.odp_power.onClick.RemoveListener(Btn_Switch);
                }
                if (addFunction2)
                {
                    addFunction2 = false;
                    UIController.instance.odp_salvage.onClick.RemoveListener(Btn_Salvage);
                }
            }
            if (Switch)
            {
                currentEPC = ElectricPowerConsumption;
                currentGEP = GenerateElectricPower;
            }
            else
            {
                currentEPC = 0;
                currentGEP = 0;
            }

            if (overload)
            {
                currentEPC = currentEPC + overloadConsume;
                currentGEP = currentGEP + overloadGenerate;
                FailureProgress += 1 * Time.deltaTime;
                if (FailureProgress >= MaximumFailure)
                {
                    Debug.Log("Self Destruction");
                }
            }

            if (EletricalSystemController.instance.currentPower < 0)
            {
                if (bs.bcprofile.bt == BuildingConstruction.BuildingType.Generator)
                {

                }
                else
                {
                    if (Switch)
                    {
                        Switch = false;
                    }
                    if (overload)
                    {
                        overload = false;
                    }
                }

            }
        }
        else
        {
            if (GetComponent<Selection>().BeingSelected)
            {
                if (!addFunction2)
                {
                    addFunction2 = true;
                    UIController.instance.odp_salvage.onClick.AddListener(Btn_Salvage);
                }
            }
            else
            {
                if (addFunction2)
                {
                    addFunction2 = false;
                    UIController.instance.odp_salvage.onClick.RemoveListener(Btn_Salvage);
                }
            }
        }
    }

    private void ActivateSwitch()
    {
        Activate = !Activate;
        Debug.Log("Delegate Call Function OnClick");
        if (Activate)
        {
            if (!Switch)
            {
                PowerSwitch();
            }
        }
        else
        {
            if (Switch)
            {
                PowerSwitch();
            }
        }
        if (UIController.instance.odp_power.GetComponent<ButtonSwitch>())
        {
            UIController.instance.odp_power.GetComponent<ButtonSwitch>().Switch();
        }
    }

    private void PowerSwitch()
    {
        if (EletricalSystemController.instance.currentPower <= 0)
        {
            if (bs.bcprofile.bt == BuildingConstruction.BuildingType.Generator)
            {

                Switch = !Switch;
                return;
            }
        }
        else
        {
            Switch = !Switch;
        }
    }
    public void PowerSwitchAgent()
    {
        PowerSwitch();
    }

    private void SelfDestruction()
    {
        if (GetComponent<Selection>().BeingSelected)
        {
            GetComponent<Selection>().BeingSelected = false;
            if (UIController.instance.SeparateODP.activeInHierarchy)
            {
                UIController.instance.SeparateODP.SetActive(false);
            }
            if (bs.bu.bu_local_odp.gameObject.activeInHierarchy)
            {
                bs.bu.bu_local_odp.gameObject.SetActive(false);
            }
        }
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        Btn_Switch -= ActivateSwitch;
        Btn_Salvage -= SelfDestruction;
    }
}
