using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortableDrill : MonoBehaviour
{
    public BuildingUI bu;
    public EletricalSystem es;

    public Spinner drill;
    public GameObject MiningArea;
    public List<OreNode> oreNode;
    public Transform DropOff;
    public ParticleSystem ps;

    public bool mining = false;
    public float miningSpeed = 1;
    public float drillSpeed = 2;

    public float progress;
    [SerializeField] private float maxProgress = 100;
    public bool GotOre = false;
    public bool CooldownAction = false;
    public int itemPerTime = 10;
    public bool runOnce;

    private void Update()
    {
        if (EletricalSystemController.instance.currentPower > es.ElectricPowerConsumption)
        {
            if (es.Activate)
            {
                if (!es.Switch)
                {
                    es.PowerSwitchAgent();
                }
            }
        }
        if (bu.select.BeingSelected)
        {
            if (bu.bu_local_odp.topic_txt[2].transform.parent.gameObject.activeInHierarchy)
            {
                bu.bu_local_odp.topic_txt[2].transform.parent.gameObject.SetActive(false);
            }
            bu.bu_local_odp.description.text = bu.bs.bcprofile.description;
            if (bu.bu_local_odp.topic_txt.Length >= 3)
            {
                if (oreNode.Count > 0)
                {
                    if (bu.bu_local_odp.topic_txt[0].text != oreNode[0].nr.nrName + " Production")
                    {
                        bu.bu_local_odp.topic_txt[0].text = oreNode[0].nr.nrName + " Production";
                    }
                }
                else
                {
                    if (bu.bu_local_odp.topic_txt[0].text != "No Available Resources")
                    {
                        bu.bu_local_odp.topic_txt[0].text = "No Available Resources";
                    }
                }
                if (es.Switch)
                {
                    if (bu.bu_local_odp.topic_txt[1].text != "Power")
                    {
                        bu.bu_local_odp.topic_txt[1].text = "Power";
                    }
                }
                else
                {
                    if (bu.bu_local_odp.topic_txt[1].text != "Require Power")
                    {
                        bu.bu_local_odp.topic_txt[1].text = "Require Power";
                    }
                }
            }
            if (bu.bu_local_odp.number_txt.Length >= 2)
            {
                bu.bu_local_odp.number_txt[0].text = CustomFunction.ValueToShortText(itemPerTime,"");
                bu.bu_local_odp.number_txt[1].text = CustomFunction.ValueToShortText(es.currentEPC, "W");
            }
            if (bu.bu_local_odp.bar.Length >= 1)
            {
                bu.bu_local_odp.bar[0].value = 0;
            }
            if (bu.bu_local_odp.icon.Length >= 1)
            {
                bu.bu_local_odp.icon[0].sprite = SpriteContainer.instance.sprite[0];
            }
        }
        if (es.Switch)
        {
            if (!mining)
            {
                mining = true;
            }
            if (!drill.Spinning)
            {
                runOnce = false;
                if (!drill.StartSpin && !runOnce)
                {
                    drill.maximumSpeed = drillSpeed;
                    runOnce = true;
                    drill.StartSpin = true;
                }
            }
        }
        else
        {
            if (drill.Spinning)
            {
                runOnce = false;
                if (!drill.StopSpin && !runOnce)
                {
                    runOnce = true;
                    drill.StopSpin = true;
                }
            }
            if (mining)
            {
                mining = false;
            }
        }
        CheckOre();
        if (mining)
        {
            if(MiningArea != null)
            {
                if (MiningArea.GetComponent<TriggerStatus>())
                {
                    if (MiningArea.GetComponent<TriggerStatus>().Triggered)
                    {
                        progress += miningSpeed * Time.deltaTime;
                        bu.timer = (progress / 100);

                        if (progress >= maxProgress)
                        {
                            for (int i = 0; i < oreNode.Count; i++)
                            {
                                if (oreNode[i].CanHarvest)
                                {
                                    oreNode[i].currentAmount -= itemPerTime;
                                    NaturalResources nare = oreNode[i].nr;
                                    ResourceController.instance.FilterResource(nare).nrAmount += itemPerTime;
                                }
                            }
                            ps.gameObject.SetActive(true);
                            ps.Play();
                            progress = 0;
                            if (!GotOre && !CooldownAction)
                            {
                                GotOre = true;
                                if (!CooldownAction)
                                {
                                    CooldownAction = true;
                                    StartCoroutine("CooldownOre");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void CheckOre()
    {
        int TriggerLength = MiningArea.GetComponent<TriggerStatus>().TriggeredObject.Count;
        for (int i = 0; i < TriggerLength; i++)
        {
            if(oreNode.Count > 0)
            {
                if (!MiningArea.GetComponent<TriggerStatus>().TriggeredObject.Contains(oreNode[i].gameObject))
                {
                    oreNode.RemoveAt(i);
                    return;
                }
            }
            if (MiningArea.GetComponent<TriggerStatus>().TriggeredObject[i].GetComponent<OreNode>())
            {
                if (oreNode.Contains(MiningArea.GetComponent<TriggerStatus>().TriggeredObject[i].GetComponent<OreNode>()))
                {
                    oreNode.Add(MiningArea.GetComponent<TriggerStatus>().TriggeredObject[i].GetComponent<OreNode>());
                }
            }
        }
    }
    IEnumerator CooldownOre()
    {
        while (CooldownAction)
        {
            yield return new WaitForSeconds(1f);
            ps.gameObject.SetActive(false);
            CooldownAction = false;
            GotOre = false;

        }
    }
}
