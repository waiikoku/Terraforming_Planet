using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public enum ODP_Type
    { 
        Generator,
        Factory,
        Storage,
        Mainbase,
        Unit,
        Item 
    }

    public static UIController instance;

    public GameObject Menu;

    [Header("Resources")]
    public TextMeshProUGUI TMPro_Mineral;
    public TextMeshProUGUI TMPro_Metal;
    public TextMeshProUGUI TMPro_Supply;

    [Header("Electricity")]
    public TextMeshProUGUI TMPro_Electricity;

    [Header("Map")]
    public RawImage minimap;

    [Header("Single Selection")]
    public GameObject SingleSelection;
    public bool SS_TMPro = false;
    public Image icon;
    /*
    public Text shield;
    public Text health;
    public Text energy;
    public Text maxShield;
    public Text maxHealth;
    public Text maxEnergy;
    */
    public TextMeshProUGUI TMPro_Shield;
    public TextMeshProUGUI TMPro_Health;
    public TextMeshProUGUI TMPro_Energy;
    public TextMeshProUGUI TMPro_maxShield;
    public TextMeshProUGUI TMPro_maxHealth;
    public TextMeshProUGUI TMPro_maxEnergy;
    
    //public Text nameCurrentSelect;
    public TextMeshProUGUI TMPro_NCS;

   // public Text currentCommand;
    public TextMeshProUGUI TMPro_CurrentCommand;

    public Slider progressBar;
    public RawImage unitPortrait;

    public Button Btn_function;
    public Scrollbar switchForButton;

    [Header("Multi Selection")]
    public GameObject MultiSelection;

    [Header("Function Panel")]
    public GameObject CurrentOdp;
    public ODP_Type CurrentOdpType;

    public GameObject[] ObjectDetailPanel;
    public ODP_Type[] odpType;
    public GameObject SeparateODP;
    public TextMeshProUGUI odp_title;
    public Button odp_priority;
    public Button odp_power;
    public Button odp_salvage;

    public Button CreateDrone;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(CurrentOdp == null)
        {
            foreach (GameObject item in ObjectDetailPanel)
            {
                if (item != null)
                {
                    if (item.activeInHierarchy)
                    {
                        item.SetActive(false);
                    }
                }
            }
            if (SeparateODP.activeInHierarchy)
            {
                SeparateODP.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject item in ObjectDetailPanel)
            {
                if (item != CurrentOdp)
                {
                    if (item != null)
                    {
                        if (item.activeInHierarchy)
                        {
                            item.SetActive(false);
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool activate = minimap.transform.parent.gameObject.activeInHierarchy;
            minimap.transform.parent.gameObject.SetActive(!activate);
        }
    }
}
