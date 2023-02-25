using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreNodeUI : MonoBehaviour
{
    [SerializeField] private Selection selection;
    [SerializeField] private GameObject selector;
    [SerializeField] private OreNode orenode;
    [SerializeField] private UIController.ODP_Type odpType;
    [SerializeField] private ObjectCaptionPanel on_ocp;
    [SerializeField] private Sprite IconImage;
    [SerializeField] private Color IconColour = Color.white;
    private void Start()
    {
        if (!selection.BeingSelected)
        {
            if (selector.activeInHierarchy)
            {
                selector.SetActive(false);
            }
        }
        if(orenode == null)
        {
            if (GetComponent<OreNode>())
            {
                orenode = GetComponent<OreNode>();
            }
        }
        if (selection == null)
        {
            if (GetComponent<Selection>())
            {
                selection = GetComponent<Selection>();
            }
        }
    }

    private void Update()
    {
        if (on_ocp == null)
        {
            on_ocp = UIController.instance.ObjectDetailPanel[(int)odpType].GetComponent<ObjectCaptionPanel>();
        }
        if (selection.BeingSelected)
        {
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
            }
            if (!playerSelector.instance.MultiSelect)
            {
                OpenDetailUI();
            }
        }
        else
        {
            if (selector.activeInHierarchy)
            {
                selector.SetActive(false);
            }
        }
    }

    private void OpenDetailUI()
    {
        if (UIController.instance.CurrentOdp != on_ocp)
        {
            UIController.instance.CurrentOdp = on_ocp.gameObject;
        }
        if (UIController.instance.CurrentOdpType != odpType)
        {
            UIController.instance.CurrentOdpType = odpType;
        }
        if (!on_ocp.gameObject.activeInHierarchy)
        {
            on_ocp.gameObject.SetActive(true);
        }
        if (!UIController.instance.SeparateODP.activeInHierarchy)
        {
            UIController.instance.SeparateODP.SetActive(true);
        }
        if (UIController.instance.odp_power.gameObject.activeSelf)
        {
            UIController.instance.odp_power.gameObject.SetActive(false);
        }
        if (UIController.instance.odp_salvage.gameObject.activeSelf)
        {
            UIController.instance.odp_salvage.gameObject.SetActive(false);
        }
        if (UIController.instance.odp_title.text != orenode.nr.nrName)
        {
            UIController.instance.odp_title.text = orenode.nr.nrName;
        }
        if (on_ocp != null)
        {
            if (on_ocp.GetComponent<ObjectCaptionPanel>())
            {
                if (on_ocp.GetComponent<ObjectCaptionPanel>().description != null)
                {
                    if (on_ocp.GetComponent<ObjectCaptionPanel>().description.text != orenode.description)
                    {
                        on_ocp.GetComponent<ObjectCaptionPanel>().description.text = orenode.description;
                    }
                }
                if (on_ocp.GetComponent<ObjectCaptionPanel>().number_txt.Length >= 2)
                {
                    if (on_ocp.GetComponent<ObjectCaptionPanel>().number_txt[0].text != orenode.rn.ToString())
                    {
                        on_ocp.GetComponent<ObjectCaptionPanel>().number_txt[0].text = orenode.rn.ToString();
                    }
                    if (on_ocp.GetComponent<ObjectCaptionPanel>().number_txt[1].text != orenode.currentAmount.ToString())
                    {
                        on_ocp.GetComponent<ObjectCaptionPanel>().number_txt[1].text = orenode.currentAmount.ToString();
                    }
                }
                if (on_ocp.GetComponent<ObjectCaptionPanel>().icon.Length >= 3)
                {
                    for (int i = 0; i < on_ocp.GetComponent<ObjectCaptionPanel>().icon.Length; i++)
                    {
                        if (on_ocp.GetComponent<ObjectCaptionPanel>().icon[i].sprite != IconImage)
                        {
                            on_ocp.GetComponent<ObjectCaptionPanel>().icon[i].sprite = IconImage;
                        }
                        if (on_ocp.GetComponent<ObjectCaptionPanel>().icon[i].color != IconColour)
                        {
                            on_ocp.GetComponent<ObjectCaptionPanel>().icon[i].color = IconColour;
                        }
                    }
                }
            }
        }
    }

    private void CloseDetailUI()
    {

    }
}
