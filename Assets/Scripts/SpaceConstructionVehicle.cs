using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpaceConstructionVehicle : MonoBehaviour
{
    public Selection select;
    [SerializeField] private GameObject selector;

    [Header("Component")]
    public Rigidbody rb;
    public Animator anim;
    public NavMeshAgent nma;
    private Vector3 currentDestination;

    [Header("Status")]
    public int currentHealth = 100;
    public int maximumHealth = 100;
    public int currentShield = 100;
    public int ShieldCapacity = 100;
    public int currentEnergy = 100;
    public int EnergyCapacity = 100;

    [Header("Command & Position")]
    public string currentCommand;
    public Vector3 currentTargetPosition;
    public Transform currentTargetTransform;
    public Transform storage;
    public Transform holdingItem;

    //Display
    public Transform targetPoint;
    public GameObject liner;

    [Header("Upgrade")]
    public string[] upgradeName;
    public int[] upgradeTier;

    [Header("Constructions")]
    public GameObject currentConstruct;
    public float constructionSpeed = 1f;
    public bool Constructing = false;
    private bool getRandomOffsetFromBuilding;
    private Vector3 tempPos;

    [Header("LocalResource")]
    public GameObject currentNaturalResource;
    public int currentNaturalResourceAmount = 0;
    public bool isHarvest = false;
    public bool isHarvested = false;
    public float harvestProgressive = 0;
    public int harvestTime = 10;
    public int harvestAmount = 5;
    private bool inDistance = false;
    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (nma == null)
        {
            nma = GetComponent<NavMeshAgent>();
        }
    }

    private void Update()
    {
        if (select.BeingSelected)
        {
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
                targetPoint.gameObject.SetActive(true);
                liner.SetActive(true);
            }
            KeyboardInput();
            MouseInput();
        }
        else
        {
            if (selector.activeInHierarchy)
            {
                selector.SetActive(false);
                targetPoint.gameObject.SetActive(false);
                liner.SetActive(false);
            }
        }
        AINavigator();
        if (currentTargetPosition != Vector3.zero)
        {
            targetPoint.position = currentTargetPosition;
        }
        if (currentTargetTransform != null)
        {
            targetPoint.position = currentTargetTransform.position;
        }
    }

    void KeyboardInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
        if (Input.GetKey(KeyCode.LeftShift))
        {

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {

        }
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit);
            if (isHit)
            {
                if (getRandomOffsetFromBuilding)
                {
                    getRandomOffsetFromBuilding = false;
                }
                if (hit.transform.CompareTag("Ground"))
                {
                    currentCommand = "MoveToPosition";
                    currentTargetPosition = hit.point;
                    targetPoint.position = currentTargetPosition;
                    if(currentTargetTransform != null)
                    {
                        currentTargetTransform = null;
                    }
                }
                else
                {
                    if (hit.transform.GetComponent<OreNode>() || hit.transform.GetComponent<CommandCenter>() || hit.transform.GetComponent<BuildingState>())
                    {
                        if (hit.transform.GetComponent<OreNode>())
                        {
                            if (currentTargetTransform != null && currentTargetTransform.GetComponent<OreNode>())
                            {
                                if (hit.transform.GetComponent<OreNode>() != currentTargetTransform.GetComponent<OreNode>())
                                {

                                }
                            }
                            currentCommand = "Harvest";
                            currentTargetTransform = hit.transform;
                            targetPoint.position = currentTargetTransform.position;
                            return;
                        }
                        if (isHarvested && hit.transform.GetComponent<CommandCenter>())
                        {
                            currentCommand = "Transfer";
                            storage = hit.transform;
                            targetPoint.position = storage.position;
                            return;
                        }
                        else
                        {
                            currentCommand = "FollowToTarget";
                            currentTargetTransform = hit.transform;
                            targetPoint.position = currentTargetTransform.position;
                        }
                        if (hit.transform.GetComponent<BuildingState>() && !hit.transform.GetComponent<BuildingState>().buildingCompleted)
                        {
                            currentCommand = "Construct";
                            currentTargetTransform = hit.transform;
                            targetPoint.position = currentTargetTransform.position;
                            return;
                        }
                        else
                        {
                            currentCommand = "FollowToTarget";
                            currentTargetTransform = hit.transform;
                            targetPoint.position = currentTargetTransform.position;
                        }
                    }
                    else
                    {
                        currentCommand = "FollowToTarget";
                        currentTargetTransform = hit.transform;
                        targetPoint.position = currentTargetTransform.position;
                    }
                    if (currentTargetPosition != Vector3.zero)
                    {
                        currentTargetPosition = Vector3.zero;
                    }
                }
            }
        }
    }

    void AINavigator()
    {
        if (currentCommand == "MoveToPosition")
        {
            nma.SetDestination(currentTargetPosition);
        }
        if (currentCommand == "FollowToTarget")
        {
            if (currentTargetTransform.GetComponent<BuildingState>())
            {
                if (!getRandomOffsetFromBuilding)
                {
                    getRandomOffsetFromBuilding = true;
                    float randomA = Random.Range(0, 100);
                    float multipierA;
                    if(randomA > 50)
                    {
                        multipierA = 1;
                    }
                    else
                    {
                        multipierA = -1;
                    }
                    float randomB = Random.Range(0, 100);
                    float multipierB;
                    if (randomB > 50)
                    {
                        multipierB = 1;
                    }
                    else
                    {
                        multipierB = -1;
                    }
                    tempPos = new Vector3((currentTargetTransform.GetComponent<BuildingState>().buildingScale.x / 2) * multipierA, 0, (currentTargetTransform.GetComponent<BuildingState>().buildingScale.z / 2) * multipierB);
                    Debug.Log("tP: " + tempPos);
                }
                nma.SetDestination(currentTargetTransform.position + tempPos);
            }
            else
            {
                nma.SetDestination(currentTargetTransform.position);
            }
        }
        if (currentCommand == "Harvest")
        {
            if (currentTargetTransform.GetComponent<OreNode>().CanHarvest)
            {
                if (!isHarvested)
                {
                    nma.SetDestination(currentTargetTransform.position);
                    currentDestination = currentTargetTransform.position;
                    if (nma.pathPending)
                    {
                    }
                    else
                    {
                        if (nma.hasPath)
                        {
                            if (nma.remainingDistance != Mathf.Infinity)
                            {
                                if (nma.remainingDistance < nma.stoppingDistance)
                                {
                                    inDistance = true;
                                }
                                else
                                {
                                    inDistance = false;
                                }
                            }
                        }
                    }
                    if (inDistance)
                    {
                        if (currentTargetTransform.GetComponent<OreNode>().CanHarvest)
                        {
                            if (harvestProgressive < 100)
                            {
                                harvestProgressive += harvestTime * Time.deltaTime;
                            }
                            else
                            {
                                inDistance = false;
                                if (!isHarvested)
                                {
                                    currentTargetTransform.GetComponent<OreNode>().Harvesting(harvestAmount, holdingItem);
                                    currentNaturalResource = holdingItem.GetChild(0).gameObject;
                                    isHarvested = true;
                                    harvestProgressive = 0;
                                }
                                currentCommand = "Transfer";
                            }
                        }
                        else
                        {
                            harvestProgressive = 0;
                            currentCommand = "Idle";
                        }
                    }
                }
                else
                {
                    currentCommand = "Transfer";
                }
            }
            else
            {
                if (!isHarvest)
                {
                    currentCommand = "Idle";
                    nma.SetDestination(transform.position);
                }
            }
        }
        if (currentCommand == "Transfer")
        {
            if(currentNaturalResource == null)
            {
                return;
            }
            if (currentDestination  != nearestTransform(ResourceController.instance.storageTransform).position)
            {
                storage = nearestTransform(ResourceController.instance.storageTransform);
                currentDestination = storage.position;
                Debug.Log("Assign Best Target To Store");
            }
            else
            {
                Debug.Log("SetTransferDestination");
                nma.SetDestination(storage.position);

                if (nma.pathPending)
                {
                }
                else
                {
                    if (nma.hasPath)
                    {
                        if (nma.remainingDistance != Mathf.Infinity)
                        {
                            if (nma.remainingDistance < nma.stoppingDistance)
                            {
                                ResourceController.instance.FilterResource(currentNaturalResource.GetComponent<localOre>().profile).nrAmount += currentNaturalResource.GetComponent<localOre>().localAmount;
                                Destroy(currentNaturalResource.transform.gameObject);
                                currentNaturalResource = null;
                                Debug.Log("Delivered!");
                                isHarvested = false;
                                if (currentTargetTransform.GetComponent<OreNode>().CanHarvest)
                                {
                                    currentCommand = "Harvest";
                                }
                                else
                                {
                                    currentCommand = "Idle";
                                }
                            }
                        }
                    }
                }
            }
        }
        if (currentCommand == "Construct")
        {
            if (!currentTargetTransform.GetComponent<BuildingState>().buildingCompleted)
            {
                if (!getRandomOffsetFromBuilding)
                {
                    getRandomOffsetFromBuilding = true;
                    float randomA = Random.Range(0, 100);
                    float multipierA;
                    if (randomA > 50)
                    {
                        multipierA = 1;
                    }
                    else
                    {
                        multipierA = -1;
                    }
                    float randomB = Random.Range(0, 100);
                    float multipierB;
                    if (randomB > 50)
                    {
                        multipierB = 1;
                    }
                    else
                    {
                        multipierB = -1;
                    }
                    tempPos = new Vector3((currentTargetTransform.GetComponent<BuildingState>().buildingScale.x / 2) * multipierA, 0, (currentTargetTransform.GetComponent<BuildingState>().buildingScale.z / 2) * multipierB);
                    Debug.Log("tP: " + tempPos);
                }
                nma.SetDestination(currentTargetTransform.position + tempPos);

                if (nma.pathPending)
                {
                }
                else
                {
                    if (nma.hasPath)
                    {
                        if (nma.remainingDistance != Mathf.Infinity)
                        {
                            if (nma.remainingDistance < nma.stoppingDistance)
                            {
                                Constructing = true;
                            }
                            else
                            {
                                Constructing = false;
                            }
                        }
                    }
                }

                if (Constructing)
                {
                    if (!currentTargetTransform.GetComponent<BuildingState>().worker.Contains(gameObject))
                    {
                        currentTargetTransform.GetComponent<BuildingState>().worker.Add(gameObject);
                    }
                    currentTargetTransform.GetComponent<BuildingState>().Construction(constructionSpeed);
                }
            }
            else
            {
                nma.SetDestination(transform.position);
                currentCommand = "Idle";
            }
        }
    }

    void HarvestNaturalResources()
    {

    }
    Transform nearestTransform(Transform[] target)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(Transform potentialTarget in target)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (currentCommand == "MoveToPosition")
        {
            Gizmos.DrawLine(transform.position, currentTargetPosition);
        }
        if (currentCommand == "FollowToTarget")
        {
            if (currentTargetTransform != null)
            {
                Gizmos.DrawLine(transform.position, currentTargetTransform.position);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(currentDestination, new Vector3(2, 10, 2));
    }
}
