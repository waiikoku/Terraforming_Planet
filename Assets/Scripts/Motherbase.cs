using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Motherbase : MonoBehaviour
{
    public enum actionType { create , upgrade , research }
    public List<actionType> ActionList;

    private Camera cam;

    [Header("Selection")]
    public Selection selection;
    public GameObject selector;

    [Header("Status")]
    public int currentHealth = 100;
    public int maximumHealth = 100;
    public int currentShield = 100;
    public int ShieldCapacity = 100;
    public int currentEnergy = 100;
    public int EnergyCapacity = 100;
    public int currentWorker = 0;
    public int maximumWorker = 16;
    public BuildingUI bu;


    [Header("Order")]
    public List<string> command;
    public List<float> actionTime;
    public List<ScriptableObject> solist;
    public int queued = 0;

    [Header("Functional")]
    public ConstructRecipe[] recipe;
    public ResearchFormula[] formula;
    public UpgradeTech[] upgrade;
    public List<int> tempMineral;
    public List<int> tempGas;
    public bool startAction = false;
    public float currentPercentage = 0;
    public float currentTime = 0;
    private float maxPercentage = 100;
    private float tempTime = 0;

    [Header("Coordinate")]
    public Transform spawnPoint;
    public Transform rallyPoint;
    [SerializeField] private Transform rallyTransform;
    [SerializeField] private bool rallyTarget;
    public GameObject liner;

    public UnityEngine.UI.Button UI_Btn;
    private bool addFunction = false;
    public UnityEngine.Events.UnityAction Btn_Create;

    private void Start()
    {
        cam = Camera.main;
        Btn_Create += CreateDrone;
    }

    private void Update()
    {
        if(UI_Btn == null)
        {
            UI_Btn = UIController.instance.CreateDrone;
        }
        if (command.Count == 0)
        {
            if (tempTime != 0)
            {
                tempTime = 0;
            }
            if (currentTime != 0)
            {
                currentTime = 0;
            }
            if (currentPercentage != 0)
            {
                currentPercentage = 0;
            }
        }
        if (selection.BeingSelected)
        {
            if (bu.bu_local_odp.description.text != bu.bs.bcprofile.description)
            {
                bu.bu_local_odp.description.text = bu.bs.bcprofile.description;
            }
            if (!addFunction)
            {
                addFunction = true;
                UI_Btn.onClick.AddListener(Btn_Create);
            }
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
                spawnPoint.gameObject.SetActive(true);
                rallyPoint.gameObject.SetActive(true);
                liner.SetActive(true);
            }
            if(command.Count > 0)
            {
                if (!startAction)
                {
                    startAction = true;
                    StartCoroutine("Function");
                }
            }
            else
            {
                if (startAction)
                {
                    startAction = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape)  || Input.GetKeyDown(KeyCode.Delete))
            {
                if(command.Count > 0)
                {
                    Debug.Log("Canncel "+command[command.Count - 1]);
                    if (ActionList[ActionList.Count - 1] == actionType.create)
                    {
                        ResourceController.instance.suu.CurrentSupply -= 1;
                    }
                    command.RemoveAt(command.Count - 1);
                    actionTime.RemoveAt(actionTime.Count - 1);
                    ActionList.RemoveAt(ActionList.Count - 1);
                    ResourceController.instance.nrProfile[0].nrAmount += tempMineral[tempMineral.Count - 1];
                    ResourceController.instance.nrProfile[1].nrAmount += tempGas[tempGas.Count - 1];
                    tempMineral.RemoveAt(tempMineral.Count - 1);
                    tempGas.RemoveAt(tempGas.Count - 1);
                }
                else
                {
                    if(Input.GetKeyDown(KeyCode.Escape))
                    {
                        selection.BeingSelected = false;
                        playerSelector.instance.currentSelect = null;
                    }
                    if (Input.GetKeyDown(KeyCode.Delete))
                    {
                        SelfDestruction();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Creating(recipe[0]);
            }
            /*
            if (Input.GetKeyDown(KeyCode.U))
            {
                Upgrading(upgrade[0]);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Researching(formula[0]);
            }
            */
            if (rallyTarget)
            {
                rallyPoint.position = new Vector3(rallyTransform.position.x,spawnPoint.position.y, rallyTransform.position.z);
            }
            Raycaster();
        }
        else
        {
            if (addFunction)
            {
                addFunction = false;
                UI_Btn.onClick.RemoveListener(Btn_Create);
            }
            if (selector.activeInHierarchy)
            {
                selector.SetActive(false);
                spawnPoint.gameObject.SetActive(false);
                rallyPoint.gameObject.SetActive(false);
                liner.SetActive(false);
            }
        }
    }

    private void SelfDestruction()
    {
        Destroy(gameObject, 1f);
    }

    public void CreateDrone()
    {
        ConstructRecipe cr = recipe[0];
        if (CheckForCreate(cr))
        {
            solist.Add(cr);
            ActionList.Add(actionType.create);
            for (int i = 0; i < cr.nrProfile.Length; i++)
            {
                ResourceController.instance.FilterResource(cr.nrProfile[i]).nrAmount -= cr.requireAmount[i];
            }
            ResourceController.instance.suu.CurrentSupply += cr.unitSize;
            if (cr.requireAmount.Length > 1)
            {
                tempMineral.Add(cr.requireAmount[0]);
                tempGas.Add(cr.requireAmount[1]);
            }
            else
            {
                tempMineral.Add(cr.requireAmount[0]);
                tempGas.Add(0);
            }
            queued++;
            command.Add("[Create Unit]:" + cr.crName + " Queue No." + queued);
            actionTime.Add(cr.productionTime);
        }
    }

    public void Creating(ConstructRecipe cr)
    {
        if (CheckForCreate(cr))
        {
            solist.Add(cr);
            ActionList.Add(actionType.create);
            for (int i = 0; i < cr.nrProfile.Length; i++)
            {
                ResourceController.instance.FilterResource(cr.nrProfile[i]).nrAmount -= cr.requireAmount[i];
            }
            ResourceController.instance.suu.CurrentSupply += cr.unitSize;
            if(cr.requireAmount.Length > 1)
            {
                tempMineral.Add(cr.requireAmount[0]);
                tempGas.Add(cr.requireAmount[1]);
            }
            else
            {
                tempMineral.Add(cr.requireAmount[0]);
                tempGas.Add(0);
            }
            queued++;
            command.Add("[Create Unit]:" + cr.crName + " Queue No." + queued);
            actionTime.Add(cr.productionTime);
        }
    }

    public void Upgrading(UpgradeTech ut)
    {
        if (CheckForUpgrade(ut))
        {
            ActionList.Add(actionType.upgrade);
            for (int i = 0; i < ut.nr.Length; i++)
            {
                ResourceController.instance.FilterResource(ut.nr[i]).nrAmount -= ut.cost[i];
            }
            if (ut.cost.Length > 1)
            {
                tempMineral.Add(ut.cost[0]);
                tempGas.Add(ut.cost[1]);
            }
            else
            {
                tempMineral.Add(ut.cost[0]);
                tempGas.Add(0);
            }
            queued++;
            command.Add("[Upgrade Technology]:" + ut.upgradeName + " Queue No." + queued);
            actionTime.Add(ut.upgradeTime);
        }
    }

    public void Researching(ResearchFormula rf)
    {
        if (CheckForResearch(rf))
        {
            ActionList.Add(actionType.research);
            for (int i = 0; i < rf.nr.Length; i++)
            {
                ResourceController.instance.FilterResource(rf.nr[i]).nrAmount -= rf.cost[i];
            }
            if (rf.cost.Length > 1)
            {
                tempMineral.Add(rf.cost[0]);
                tempGas.Add(rf.cost[1]);
            }
            else
            {
                tempMineral.Add(rf.cost[0]);
                tempGas.Add(0);
            }
            queued++;
            command.Add("[Research Technology]:" + rf.topic + " Queue No." + queued);
            actionTime.Add(rf.researchingTime);
        }
    }

    private bool CheckForCreate(ConstructRecipe ccr)
    {
        if(ResourceController.instance.suu.CurrentSupply < ResourceController.instance.suu.SupplyCapacity && ResourceController.instance.suu.CurrentSupply + ccr.unitSize <= ResourceController.instance.suu.SupplyCapacity)
        {

        }
        else
        {
            return false;
        }
        if(ccr.nrProfile.Length == 1)
        {
            if (ResourceController.instance.FilterResource(ccr.nrProfile[0]).nrAmount >= ccr.requireAmount[0])
            {

            }
            else
            {
                return false;
            }
        }
        if(ccr.nrProfile.Length > 1)
        {
            for (int i = 0; i < ccr.nrProfile.Length; i++)
            {
                if (ResourceController.instance.FilterResource(ccr.nrProfile[i]).nrAmount >= ccr.requireAmount[i])
                {

                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckForUpgrade(UpgradeTech cut)
    {
        if(cut.nr.Length == 1)
        {
            if(ResourceController.instance.FilterResource(cut.nr[0]).nrAmount >= cut.cost[0])
            {

            }
            else
            {
                return false;
            }
        }
        if(cut.nr.Length > 1)
        {
            for (int i = 0; i < cut.nr.Length; i++)
            {
                if(ResourceController.instance.FilterResource(cut.nr[i]).nrAmount >= cut.cost[i])
                {

                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckForResearch(ResearchFormula crf)
    {
        if(crf.nr.Length == 1)
        {
            if(ResourceController.instance.FilterResource(crf.nr[0]).nrAmount >= crf.cost[0])
            {

            }
            else
            {
                return false;
            }
        }
        if(crf.nr.Length > 1)
        {
            for (int i = 0; i < crf.nr.Length; i++)
            {
                if(ResourceController.instance.FilterResource(crf.nr[i]).nrAmount >= crf.cost[i])
                {

                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }
    private void Raycaster()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                if (hitInfo.transform.CompareTag("Ground"))
                {
                    if (rallyTarget)
                    {
                        rallyTarget = false;
                        rallyTransform = null;
                    }
                    rallyPoint.position = hitInfo.point;
                    return;
                }
                else
                {
                    rallyTarget = true;
                    rallyTransform = hitInfo.transform;
                    rallyPoint.position = rallyTransform.position;
                }
            }
        }
    }

    IEnumerator Function()
    {
        while (startAction)
        {
            if(currentTime < actionTime[0])
            {
                tempTime += Time.deltaTime;
                currentTime = tempTime;
            }
            if(currentPercentage < maxPercentage)
            {
                currentPercentage = (int)(currentTime / actionTime[0] * 100);
            }
            else
            {
                if(ActionList[0] != actionType.create)
                {
                    if (solist[0].Equals(formula))
                    {
                        //Debug.Log("Formula One");
                    }

                    if (solist[0].Equals(upgrade))
                    {
                        //Debug.Log("Upgrader");
                    }
                    tempMineral.RemoveAt(0);
                    tempGas.RemoveAt(0);
                    ActionList.RemoveAt(0);
                    //Debug.Log(command[0] + " has completed!");
                    tempTime = 0;
                    currentTime = 0;
                    currentPercentage = 0;
                    command.RemoveAt(0);
                    actionTime.RemoveAt(0);
                }
                else
                {
                    if (ResourceController.instance.suu.CurrentSupply >= ResourceController.instance.suu.SupplyCapacity)
                    {
                        //Debug.Log("Supply Block!");
                        //Do nothing
                    }
                    else
                    {
                        if(FilterCR(solist[0]))
                        {
                            //Debug.Log("Reciper");
                        }
                        GameObject unit = Instantiate(recipe[0].product, spawnPoint.position, Quaternion.identity);
                        if (unit.GetComponent<Worker>())
                        {
                            if (rallyTarget)
                            {
                                if(rallyTransform != null)
                                {
                                    if (rallyTransform.GetComponent<OreNode>())
                                    {
                                        if (rallyTransform.GetComponent<OreNode>().CanHarvest)
                                        {
                                            unit.GetComponent<Worker>().HarvestOre(rallyTransform);
                                        }
                                    }
                                    else
                                    {
                                        unit.GetComponent<Worker>().FollowTarget(rallyTransform);
                                    }

                                }
                            }
                            else
                            {
                                unit.GetComponent<Worker>().MoveToPosition(rallyPoint.position);
                            }
                        }

                        tempMineral.RemoveAt(0);
                        tempGas.RemoveAt(0);
                        ActionList.RemoveAt(0);
                        //Debug.Log(command[0] + " has completed!");
                        tempTime = 0;
                        currentTime = 0;
                        currentPercentage = 0;
                        command.RemoveAt(0);
                        actionTime.RemoveAt(0);
                    }
                }
            }
            yield return null;
            bu.timer = (currentPercentage / 100);
            if(command.Count  == 0)
            {
                startAction = false;
                queued = 0;
                command.Clear();
            }
        }

    }

    private bool FilterCR(object cr)
    {
        for (int i = 0; i < recipe.Length; i++)
        {
            if(cr == recipe[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool FilterRF(ResearchFormula rf)
    {
        for (int i = 0; i < formula.Length; i++)
        {
            if(rf == formula[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool FilterUT(UpgradeTech ut)
    {
        for (int i = 0; i < upgrade.Length; i++)
        {
            if(ut == upgrade[i])
            {
                return true;
            }
        }
        return false;
    }

    private void OnDestroy()
    {
        Btn_Create -= CreateDrone;
    }
}
