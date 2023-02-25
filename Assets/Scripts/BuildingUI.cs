using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public enum Priority { Low , Medium , High }

    public UIController.ODP_Type odpType;
    public Priority priority = Priority.Medium;

    public Selection select;
    public BuildingState bs;

    public Sprite icon;
    public string currentAction;
    public string shield;
    public string health;
    public string energy;
    public string maxShield;
    public string maxHealth;
    public string maxEnergy;
    public string TermNoun;
    public float timer;
    public string consumePower;
    public string generatePower;

    public ObjectCaptionPanel bu_local_odp;
    private void Update()
    {
        if(bu_local_odp == null)
        {
            bu_local_odp = UIController.instance.ObjectDetailPanel[(int)odpType].GetComponent<ObjectCaptionPanel>();
        }
        if (select.BeingSelected)
        {
            if (!playerSelector.instance.MultiSelect)
            {
                OpenDetailUI();
            }
            if (bs.buildingCompleted)
            {
                UIController.instance.icon.color = Color.white;
                if (!UIController.instance.odp_power.interactable)
                {
                    UIController.instance.odp_power.interactable = true;
                }
            }
            else
            {
                UIController.instance.icon.color = Color.gray;
                if (UIController.instance.odp_power.interactable)
                {
                    UIController.instance.odp_power.interactable = false;
                }
                if (UIController.instance.odp_power.GetComponent<ButtonSwitch>())
                {
                    if (UIController.instance.odp_power.GetComponent<ButtonSwitch>().VerifyBool())
                    {
                        UIController.instance.odp_power.GetComponent<ButtonSwitch>().SetBool(false);
                    }
                }
               
            }


            UIController.instance.icon.sprite = icon;
            if (UIController.instance.SS_TMPro)
            {

                UIController.instance.TMPro_Shield.text = shield;
                UIController.instance.TMPro_Health.text = health;
                UIController.instance.TMPro_Energy.text = energy;
                UIController.instance.TMPro_maxShield.text = "/" + maxShield;
                UIController.instance.TMPro_maxHealth.text = "/" + maxHealth;
                UIController.instance.TMPro_maxEnergy.text = "/" + maxEnergy;
                UIController.instance.TMPro_NCS.text = TermNoun;
                UIController.instance.TMPro_CurrentCommand.text = currentAction;
            }
            else
            {
                /*
                UIController.instance.shield.text = shield;
                UIController.instance.health.text = health;
                UIController.instance.energy.text = energy;
                UIController.instance.maxShield.text = maxShield;
                UIController.instance.maxHealth.text = maxHealth;
                UIController.instance.maxEnergy.text = maxEnergy;
                UIController.instance.nameCurrentSelect.text = TermNoun;
                UIController.instance.currentCommand.text = currentAction;
                */
            }
            UIController.instance.progressBar.value = timer;
        }
        else
        {

        }
    }

    private void OpenDetailUI()
    {
        if (UIController.instance.CurrentOdp != bu_local_odp)
        {
            UIController.instance.CurrentOdp = bu_local_odp.gameObject;
        }
        if (UIController.instance.CurrentOdpType != odpType)
        {
            UIController.instance.CurrentOdpType = odpType;
        }
        if (!UIController.instance.SeparateODP.activeInHierarchy)
        {
            UIController.instance.SeparateODP.SetActive(true);
        }
        if (!UIController.instance.odp_power.gameObject.activeSelf)
        {
            UIController.instance.odp_power.gameObject.SetActive(true);
        }
        if (!UIController.instance.odp_salvage.gameObject.activeSelf)
        {
            UIController.instance.odp_salvage.gameObject.SetActive(true);
        }
        if (!bu_local_odp.gameObject.activeInHierarchy)
        {
            bu_local_odp.gameObject.SetActive(true);
        }
        UIController.instance.odp_title.text = TermNoun;
    }

    private void CloseDetailUI()
    {
        if (UIController.instance.CurrentOdp != null)
        {
            UIController.instance.CurrentOdp = null;
        }
        if (UIController.instance.SeparateODP.activeInHierarchy)
        {
            UIController.instance.SeparateODP.SetActive(false);
        }
        if (UIController.instance.odp_power.gameObject.activeInHierarchy)
        {
            UIController.instance.odp_power.gameObject.SetActive(false);
        }
        if (UIController.instance.odp_salvage.gameObject.activeSelf)
        {
            UIController.instance.odp_salvage.gameObject.SetActive(false);
        }
        if (bu_local_odp.gameObject.activeInHierarchy)
        {
            bu_local_odp.gameObject.SetActive(false);
        }
    }
}
