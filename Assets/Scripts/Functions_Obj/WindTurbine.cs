using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : MonoBehaviour
{
    public BuildingUI bu;
    public EletricalSystem es;

    public Spinner turbine;
    public bool runOnce;
    public bool blowing;
    public float blowSpeed = 1f;

    private void Update()
    {
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
            if (!blowing)
            {
                blowing = true;
            }
            if (!turbine.Spinning)
            {
                runOnce = false;
                if (!turbine.StartSpin && !runOnce)
                {
                    turbine.maximumSpeed = blowSpeed;
                    runOnce = true;
                    turbine.ForceStart();
                }
            }
        }
        else
        {
            if (blowing)
            {
                blowing = false;
            }
            if (turbine.Spinning)
            {
                runOnce = false;
                if (!turbine.StopSpin && !runOnce)
                {
                    runOnce = true;
                    turbine.ForceStop();
                }
            }
        }

        if (blowing)
        {

        }
        else
        {

        }
    }

}
