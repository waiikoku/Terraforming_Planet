using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SliderAdapt : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public GameObject slider_go;

    private void Update()
    {
        if (slider != null && slider_go != null)
        {
            if (slider.value == 0)
            {
                if (slider_go.activeInHierarchy)
                {
                    slider_go.SetActive(false);
                }
            }
            else
            {
                if (!slider_go.activeInHierarchy)
                {
                    slider_go.SetActive(true);
                }
            }
            /*
            if (slider.value >= 1)
            {
                slider.value = 0;
            }
            slider.value += 0.1f * Time.deltaTime;
            */
        }
    }
}
