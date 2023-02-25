using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorkerUI : MonoBehaviour
{
    public Selection selection;
    public Worker worker;
    public LocalUIWorldSpace luws;
    public UIController.ODP_Type odpType = UIController.ODP_Type.Unit;
    [SerializeField] private ObjectCaptionPanel wu_local_odp;

    public Sprite image;
    public string workerName;
    public string workerCommand;
    public string workerCarry;
    public string workerDestination;
    public float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float energy;
    public float maxEnergy;
    public float progress = 0;
    public float maxProgress = 100;

    private UnityAction Btn_Salvage;
    private bool addFunction = false;

    private void Start()
    {
        Btn_Salvage += SelfDestruction;
    }

    private void Update()
    {
        if(wu_local_odp == null)
        {
            wu_local_odp = UIController.instance.ObjectDetailPanel[(int)odpType].GetComponent<ObjectCaptionPanel>();
        }
        /*
        if (selection.BeingSelected)
        {
            image = worker.workerProfile.WorkerImage;
            workerName = worker.WorkerName;
            workerCommand = worker.currentCommand;
            health = worker.currentHealth;
            shield = worker.currentShield;
            energy = worker.currentEnergy;
            maxHealth = worker.maximumHealth;
            maxShield = worker.ShieldCapacity;
            maxEnergy = worker.BatteryCapacity;

            health = worker.currentHealth.ToString("F0");
            shield = worker.currentShield.ToString("F0");
            energy = worker.currentEnergy.ToString("F0");
            maxHealth = worker.maximumHealth.ToString("F0");
            maxShield = worker.ShieldCapacity.ToString("F0");
            maxEnergy = worker.BatteryCapacity.ToString("F0");
            if (worker.Harvesting)
            {
                progress = worker.HarvestProgress;
            }
            if (worker.Researching)
            {
                progress = worker.ResearchProgress;
            }
            if (worker.Exploring)
            {
                progress = worker.ExplorationSpeed;
            }
          

            #region OldUIControl
            UIController.instance.icon.color = Color.white;
            UIController.instance.icon.sprite = image;
            UIController.instance.progressBar.value = progress / 100f;
            if (UIController.instance.SS_TMPro)
            {
                UIController.instance.TMPro_NCS.text = workerName;
                UIController.instance.TMPro_Health.text = health;
                UIController.instance.TMPro_Shield.text = shield;
                UIController.instance.TMPro_Energy.text = energy;
                UIController.instance.TMPro_maxShield.text = "/" + maxShield;
                UIController.instance.TMPro_maxHealth.text = "/" + maxHealth;
                UIController.instance.TMPro_maxEnergy.text = "/" + maxEnergy;
                UIController.instance.TMPro_CurrentCommand.text = workerCommand;
            }
            else
            {
                UIController.instance.nameCurrentSelect.text = workerName;
                UIController.instance.health.text = health;
                UIController.instance.shield.text = shield;
                UIController.instance.energy.text = energy;
                UIController.instance.maxShield.text = maxShield;
                UIController.instance.maxHealth.text = maxHealth;
                UIController.instance.maxEnergy.text = maxEnergy;
                UIController.instance.currentCommand.text = workerCommand;
            }
            #endregion
        }
        */

        if (selection.BeingSelected)
        {
            if (!addFunction)
            {
                addFunction = true;
                UIController.instance.odp_salvage.onClick.AddListener(Btn_Salvage);
            }
            if (!playerSelector.instance.MultiSelect)
            {
                OpenDetailUI();
            }
            if(wu_local_odp != null)
            {
                if(wu_local_odp.description.text != worker.workerProfile.workerDescription)
                {
                    wu_local_odp.description.text = worker.workerProfile.workerDescription;
                }
                if (wu_local_odp.number_txt.Length >= 4)
                {
                    wu_local_odp.number_txt[0].text = workerCommand;
                    wu_local_odp.number_txt[1].text = workerCarry;
                    wu_local_odp.number_txt[2].text = workerDestination;
                    wu_local_odp.number_txt[3].text = ((energy / maxEnergy) * 100).ToString("F0") + "%";
                }
                if(wu_local_odp.bar.Length >= 1)
                {
                    wu_local_odp.bar[0].value = energy / maxEnergy;
                }
            }
        }
        else
        {
            if (addFunction)
            {
                addFunction = false;
                UIController.instance.odp_salvage.onClick.RemoveListener(Btn_Salvage);
            }
        }

        image = worker.workerProfile.WorkerImage;
        workerName = worker.WorkerName;
        workerCommand = worker.currentCommand;
        health = worker.currentHealth;
        shield = worker.currentShield;
        energy = worker.currentEnergy;
        maxHealth = worker.maximumHealth;
        maxShield = worker.ShieldCapacity;
        maxEnergy = worker.BatteryCapacity;
        if (worker.Harvesting)
        {
            progress = worker.HarvestProgress;
        }
        if (worker.Researching)
        {
            progress = worker.ResearchProgress;
        }
        if (worker.Exploring)
        {
            progress = worker.ExplorationSpeed;
        }


        if (luws.slider.Length >= 3)
        {
            luws.slider[0].value = health / maxHealth;
            luws.slider[1].value = shield / maxShield;
            if (progress > 0 && progress < 100)
            {
                if (!luws.slider[2].gameObject.activeInHierarchy)
                {
                    luws.slider[2].gameObject.SetActive(true);
                }
                luws.slider[2].value = progress / maxProgress;
            }
            else
            {
                if (luws.slider[2].gameObject.activeInHierarchy)
                {
                    luws.slider[2].gameObject.SetActive(false);
                }
                if(luws.slider[2].value != 0)
                {
                    luws.slider[2].value = 0;
                }
            }
        }

        if(luws.txt.Length > 0)
        {
            luws.txt[0].text = workerName;
        }
        else
        {
            if(luws.txt_tmpro.Length > 0)
            {
                luws.txt_tmpro[0].text = workerName;
            }
        }
    }

    private void OpenDetailUI()
    {
        if (UIController.instance.CurrentOdp != wu_local_odp)
        {
            UIController.instance.CurrentOdp = wu_local_odp.gameObject;
        }
        if (UIController.instance.CurrentOdpType != odpType)
        {
            UIController.instance.CurrentOdpType = odpType;
        }
        if (!UIController.instance.SeparateODP.activeInHierarchy)
        {
            UIController.instance.SeparateODP.SetActive(true);
        }
        if (UIController.instance.odp_power.gameObject.activeSelf)
        {
            UIController.instance.odp_power.gameObject.SetActive(false);
        }
        if (!UIController.instance.odp_salvage.gameObject.activeSelf)
        {
            UIController.instance.odp_salvage.gameObject.SetActive(true);
        }
        if (!wu_local_odp.gameObject.activeInHierarchy)
        {
            wu_local_odp.gameObject.SetActive(true);
        }
        UIController.instance.odp_title.text = workerName;
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
        if (!UIController.instance.odp_salvage.gameObject.activeInHierarchy)
        {
            UIController.instance.odp_salvage.gameObject.SetActive(false);
        }
        if (wu_local_odp.gameObject.activeInHierarchy)
        {
            wu_local_odp.gameObject.SetActive(false);
        }
    }

    private void SelfDestruction()
    {
        if (selection.BeingSelected)
        {
            selection.BeingSelected = false;
            if (UIController.instance.SeparateODP.activeInHierarchy)
            {
                UIController.instance.SeparateODP.SetActive(false);
            }
            if (wu_local_odp.gameObject.activeInHierarchy)
            {
                wu_local_odp.gameObject.SetActive(false);
            }
        }
        Destroy(gameObject, 1f);
    }

    private void OnDestroy()
    {
        Btn_Salvage -= SelfDestruction;
    }
}
