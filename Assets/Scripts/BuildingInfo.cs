using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public Selection selection;
    public BuildingUI bu;
    public BuildingState bs;
    public EletricalSystem es;
    public LocalUIWorldSpace luws;
    private bool setuponce = false;

    public string buildingName;
    public float currentHealth;
    public float maximumHealth;
    public float currentShield;
    public float ShieldCapacity;
    public float currentEnergy;
    public float EnergyCapacity;
    public bool isNeedWorker;
    public int currentWorker;
    public int maximumWorker;
    public int currentLevel;
    public int currentTier;

    public float ConsumePower;
    public float GeneratePower;

    private void Start()
    {
        bu.shield = currentShield.ToString("F0");
        bu.health = currentHealth.ToString("F0");
        bu.energy = currentEnergy.ToString("F0");
        bu.maxShield = ShieldCapacity.ToString("F0");
        bu.maxHealth = maximumHealth.ToString("F0");
        bu.maxEnergy = EnergyCapacity.ToString("F0");
        bu.TermNoun = buildingName;
        ConsumePower = es.ElectricPowerConsumption;
        GeneratePower = es.GenerateElectricPower;
    }
    private void Update()
    {
        if (bs != null)
        {
            if (bs.placementPreview)
            {
                if (!bs.buildingCompleted && !setuponce)
                {
                    if (bs.underConstruction)
                    {
                        if (bu.currentAction != "Under Construction")
                        {
                            bu.currentAction = "Under Construction";
                        }
                    }
                    else
                    {
                        if (bu.currentAction != "Construction Incompleted")
                        {
                            bu.currentAction = "Construction Incompleted";
                        }
                    }
                }
                else
                {

                }
            }
        }
        if (bu.shield != currentShield.ToString("F0"))
        {
            bu.shield = currentShield.ToString("F0");
        }
        if (bu.health != currentHealth.ToString("F0"))
        {
            bu.health = currentHealth.ToString("F0");
        }
        if (bu.energy != currentEnergy.ToString("F0"))
        {
            bu.energy = currentEnergy.ToString("F0");
        }
        if (bu.maxShield != ShieldCapacity.ToString("F0"))
        {
            bu.maxShield = ShieldCapacity.ToString("F0");
        }
        if (bu.maxHealth != maximumHealth.ToString("F0"))
        {
            bu.maxHealth = maximumHealth.ToString("F0");
        }
        if (bu.maxEnergy != EnergyCapacity.ToString("F0"))
        {
            bu.maxEnergy = EnergyCapacity.ToString("F0");
        }
        if (bu.TermNoun != buildingName)
        {
            bu.TermNoun = buildingName;
        }
        if (ConsumePower != es.ElectricPowerConsumption)
        {
            ConsumePower = es.ElectricPowerConsumption;
        }
        if (GeneratePower != es.GenerateElectricPower)
        {
            GeneratePower = es.GenerateElectricPower;
        }
        if (bs.buildingCompleted)
        {
            if (currentHealth < 0)
            {
                Destroy(gameObject);
            }
        }
        if (luws != null)
        {
            if (luws.slider.Length >= 3)
            {
                float value0 = currentHealth / maximumHealth;
                float value1 = currentShield / ShieldCapacity;
                if (luws.slider[0].value != value0)
                {
                    if (luws.slider[0].value < value0)
                    {
                        luws.slider[0].value += 1.0f * Time.deltaTime;
                    }
                    if (luws.slider[0].value > value0)
                    {
                        luws.slider[0].value -= 1.0f * Time.deltaTime;
                    }
                }
                if (luws.slider[1].value != value1)
                {
                    if (luws.slider[1].value < value0)
                    {
                        luws.slider[1].value += 1.0f * Time.deltaTime;
                    }
                    if (luws.slider[1].value > value0)
                    {
                        luws.slider[1].value -= 1.0f * Time.deltaTime;
                    }
                }
                if (bu.timer > 0.0f && bu.timer <= 1.0f)
                {
                    if (!luws.slider[2].gameObject.activeInHierarchy)
                    {
                        luws.slider[2].gameObject.SetActive(true);
                    }
                    luws.slider[2].value = bu.timer;
                }
                else
                {
                    if (luws.slider[2].gameObject.activeInHierarchy)
                    {
                        luws.slider[2].gameObject.SetActive(false);
                    }
                    if (luws.slider[2].value != 0)
                    {
                        luws.slider[2].value = 0;
                    }
                }

            }
            if(luws.txt_tmpro.Length > 0)
            {
                luws.txt_tmpro[0].text = buildingName;
            }
        }
    }
    private void Builded()
    {
        currentHealth = maximumHealth;
        currentShield = ShieldCapacity;
        currentEnergy = EnergyCapacity;
    }
    public void Completed()
    {
        if (!setuponce)
        {
            setuponce = true;
            Builded();
        }
        else
        {
            return;
        }
    }
    public void AssignProfile(BuildingConstruction bcp)
    {
        buildingName = bcp.buildingName;
        maximumHealth = bcp.maximumHealth;
        ShieldCapacity = bcp.ShieldCapacity;
        EnergyCapacity = bcp.EnergyCapacity;
        maximumWorker = bcp.maximumWorker;
        currentLevel = bcp.level;
        currentTier = bcp.tier;
    }

    private void healing()
    {
        if(currentHealth < maximumHealth)
        {

        }
    }

    private void Charge()
    {
        if(currentShield < ShieldCapacity)
        {

        }
        if(currentEnergy < EnergyCapacity)
        {

        }
    }
}
