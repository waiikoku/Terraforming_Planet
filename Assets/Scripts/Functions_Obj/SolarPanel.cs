using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public BuildingUI bu;
    public EletricalSystem es;

    public Animator anim;
    public bool isOpen;
    public bool runOnce;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<BuildingState>().buildingCompleted)
        {
            if (es.Activate)
            {
                if (DayNightCycle.Day)
                {
                    if (!es.Switch)
                    {
                        es.PowerSwitchAgent();
                    }
                }
                if (DayNightCycle.Night)
                {
                    if (es.Switch)
                    {
                        es.PowerSwitchAgent();
                    }
                }
            }
        }
        if (bu.select.BeingSelected)
        {
            bu.bu_local_odp.description.text = bu.bs.bcprofile.description;
            if (bu.bu_local_odp.topic_txt.Length >= 3)
            {
                if (bu.bu_local_odp.topic_txt[0].text != "Power Production")
                {
                    bu.bu_local_odp.topic_txt[0].text = "Power Production";
                }
                if (bu.bu_local_odp.topic_txt[1].text != "Power Production")
                {
                    bu.bu_local_odp.topic_txt[1].text = "Power Production";
                }
                if (bu.bu_local_odp.topic_txt[2].text != "Power Demand")
                {
                    bu.bu_local_odp.topic_txt[2].text = "Power Demand";
                }
            }
            if (bu.bu_local_odp.number_txt.Length >= 3)
            {
                bu.bu_local_odp.number_txt[0].text = CustomFunction.ValueToShortText(es.currentGEP, "W");
                bu.bu_local_odp.number_txt[1].text = CustomFunction.ValueToShortText(EletricalSystemController.instance.powerSupply, "W");
                bu.bu_local_odp.number_txt[2].text = CustomFunction.ValueToShortText(EletricalSystemController.instance.powerDemand, "W");
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
            if (!runOnce)
            {
                runOnce = true;
                if (!isOpen)
                {
                    isOpen = true;
                    anim.SetBool("Open", true);
                }
            }
        }
        else
        {
            if (isOpen)
            {
                if (runOnce)
                {
                    runOnce = false;
                }
                isOpen = false;
                anim.SetBool("Open", false);
            }
        }
    }
}
