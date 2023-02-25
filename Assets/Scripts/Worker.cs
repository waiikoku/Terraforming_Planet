using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public Selection selection;
    public GameObject selector;
    public GameObject targetSelector;
    public WorkerProfile workerProfile;
    public WorkerUI wu;

    [Header("Component")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent nma;
    [SerializeField] private SkinnedMeshRenderer smr;

    private float blendShape1 = 0;

    [Header("Status")]
    public string WorkerName;
    public float currentHealth;
    public float maximumHealth;
    public float currentShield;
    public float ShieldCapacity;
    public float currentEnergy;
    public float BatteryCapacity;

    [Header("Commands")]
    public string currentCommand = "Idle";
    public List<string> Commands;
    [SerializeField] private string[] commandList;

    [Header("Transform")]
    public Transform target;
    public Vector3 localPoint;
    public Transform localTarget;
    [SerializeField] bool insideDistance = false;
    private Vector3 tempPos;
    private bool getRandomOffsetFromBuilding = false;
    [Header("Energy")]
    public bool Recharging = false;
    public bool OutOfEnergy = false;
    private string BeforeRunOutCommand;
    public GameObject RechargeStation;


    [Header("Construction")]
    [SerializeField] private bool Constructable = false;
    public GameObject Structure;
    public bool UnderConstruction;
    public float ConstructionSpeed;
    //No need for consturction progress 'Cause Structure have itself

    [Header("Harvest")]
    [SerializeField] private bool Harvestable = false;
    public GameObject Resources;
    public bool Harvesting = false;
    public bool Harvested = false;
    public float HarvestSpeed;
    public float HarvestPerTime;
    public float HarvestProgress;
    public float GoodsQuantity;

    [Header("Transport")]
    [SerializeField] private bool Transportable = false;
    public GameObject item;
    public GameObject Storage;
    public bool holding = false;
    public bool placed = false;
    public float itemAmount;
    public float itemWeight;
    public float totalWeight;
    [SerializeField] private float maximumWeight;
    [SerializeField] private Transform holdingPosition;

    [Header("Research")]
    [SerializeField] private bool Researchable = false;
    public GameObject researchObject;
    public bool Researching = false;
    public bool Researched = false;
    public float ResearchSpeed;
    public float ResearchProgress;

    [Header("Explorer")]
    [SerializeField] private bool Exploreable = false;
    public GameObject ExploreObject;
    public bool Exploring = false;
    public bool Explored = false;
    public float ExplorationSpeed;
    public float ExplorationProgress;

    [Header("OnGUI")]
    public bool showGUI = false;
    public Rect boxRect = new Rect(Screen.width / 2 , Screen.height / 2,256,32);

    private void Start()
    {
        if (currentCommand == null || currentCommand == "")
        {
            currentCommand = "Idle";
        }
        boxRect = new Rect(Screen.width / 2, Screen.height / 2, 256, 32);
        if (selection.BeingSelected)
        {
            selector.SetActive(true);
            targetSelector.SetActive(true);
        }
        else
        {
            selector.SetActive(false);
            targetSelector.SetActive(false);
        }

        if(nma != null)
        {
            nma.speed = workerProfile.movementSpeed;
            nma.angularSpeed = workerProfile.turnSpeed;
        }

        WorkerName = workerProfile.WorkerName;
        maximumHealth = workerProfile.maximumHealth;
        ShieldCapacity = workerProfile.shieldCapacity;
        BatteryCapacity = workerProfile.batteryCapacity;
        commandList = workerProfile.CommandList;


        Constructable = workerProfile.Constructable;
        Harvestable = workerProfile.Harvestable;
        Transportable = workerProfile.Transportable;
        Researchable = workerProfile.Researchable;
        Exploreable = workerProfile.Exploreable;

        ConstructionSpeed = workerProfile.constructionSpeed;
        HarvestSpeed = workerProfile.harvestSpeed;
        HarvestPerTime = workerProfile.harvestPerTime;
        maximumWeight = workerProfile.maximumWeight;
        ResearchSpeed = workerProfile.researchSpeed;
        ExplorationSpeed = workerProfile.explorationSpeed;

        currentHealth = maximumHealth;
        currentShield = ShieldCapacity;
        currentEnergy = BatteryCapacity;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        if(currentEnergy <= 0)
        {
            if (nma.hasPath)
            {
                nma.ResetPath();
            }
            OutOfEnergy = true;
            BeforeRunOutCommand = currentCommand;
            currentCommand = "OutOfEnergy";
        }
        else
        {
            if (OutOfEnergy)
            {
                OutOfEnergy = false;
                currentCommand = BeforeRunOutCommand;
                BeforeRunOutCommand = null;
            }
        }
        if (ResourceController.instance.storage.Count > 0 && ResourceController.instance.storageTransform.Length > 0)
        {
            if (Storage != nearestTransform(ResourceController.instance.storageTransform).gameObject)
            {
                Storage = nearestTransform(ResourceController.instance.storageTransform).gameObject;
            }
        }
        if (selection.BeingSelected)
        {
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
            }
            if (!nma.pathPending)
            {
                if (nma.hasPath)
                {
                    if (nma.remainingDistance <= nma.stoppingDistance)
                    {
                        if (targetSelector.activeInHierarchy)
                        {
                            targetSelector.SetActive(false);
                        }
                    }
                    else
                    {
                        if (currentEnergy > 0)
                        {
                            currentEnergy -= 1 * Time.deltaTime;
                        }
                        if (!targetSelector.activeInHierarchy)
                        {
                            targetSelector.SetActive(true);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {

            }
            if (Input.GetMouseButtonDown(1))
            {
                if (OutOfEnergy)
                {
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isHit = Physics.Raycast(ray, out hit);
                if (isHit)
                {
                    getRandomOffsetFromBuilding = false;
                    if(Resources != null)
                    {
                        Resources = null;
                    }
                    if(HarvestProgress != 0)
                    {
                        HarvestProgress = 0;
                    }
                    if (hit.transform.CompareTag("Ground"))
                    {
                        MoveToPosition(hit.point);
                    }
                    else
                    {
                        if (Constructable)
                        {
                            if (hit.transform.GetComponent<BuildingState>() && hit.transform.GetComponent<BuildingState>().placementPreview && !hit.transform.GetComponent<BuildingState>().buildingCompleted)
                            {
                                Structure = hit.transform.gameObject;
                                localPoint = Vector3.zero;
                                target = hit.transform;
                                localTarget.position = target.position;
                                currentCommand = "Construct";
                                return;
                            }
                        }
                        if (Harvestable)
                        {
                            if (hit.transform.GetComponent<OreNode>() && hit.transform.GetComponent<OreNode>().CanHarvest)
                            {
                                HarvestOre(hit.transform);
                                return;
                            }
                        }
                        if (Transportable)
                        {
                            if (hit.transform.GetComponent<Storage>() && holding)
                            {
                                Storage = hit.transform.gameObject;
                                localPoint = Vector3.zero;
                                target = hit.transform;
                                localTarget.position = target.position;
                                currentCommand = "Transport";
                                return;
                            }
                        }

                        if (currentEnergy < BatteryCapacity)
                        {
                            if (hit.transform.GetComponent<Motherbase>())
                            {
                                localPoint = Vector3.zero;
                                target = hit.transform;
                                localTarget.position = target.position;
                                currentCommand = "Recharging";
                                return;
                            }
                        }

                        FollowTarget(hit.transform);
                    }
                }
            }
        }
        else
        {
            if (selector.activeInHierarchy)
            {
                selector.SetActive(false);
                targetSelector.SetActive(false);
            }
        }
        if (holding)
        {
            if (smr.GetBlendShapeWeight(0) < 100)
            {
                blendShape1 += 80 * Time.deltaTime;
                smr.SetBlendShapeWeight(0, blendShape1);
            }
            else
            {
                if (blendShape1 != 100)
                {
                    blendShape1 = 100;
                }
            }
        }
        else
        {
            if (smr.GetBlendShapeWeight(0) > 0)
            {
                blendShape1 -= 80 * Time.deltaTime;
                smr.SetBlendShapeWeight(0, blendShape1);
            }
            else
            {
                if(blendShape1 != 0)
                {
                    blendShape1 = 0;
                }
            }
        }
        if (currentCommand == "Idle")
        {
            nma.ResetPath();
            if(currentEnergy < 20.0f)
            {
                localPoint = Vector3.zero;
                target = nearestTransform(ResourceController.instance.storageTransform);
                localTarget.position = target.position;
                currentCommand = "Recharging";
            }
        }

        if (currentCommand == "Move")
        {
            localTarget.position = localPoint;
            nma.SetDestination(localPoint);
            if (!nma.pathPending)
            {
                if (nma.hasPath)
                {
                    if (nma.remainingDistance <= nma.stoppingDistance)
                    {
                        currentCommand = "Idle";
                    }
                }
            }
        }

        if (currentCommand == "Follow")
        {
            localTarget.position = target.position;
            Vector3 heading = target.position - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            Vector3 targetDestination = target.position - direction;
            nma.SetDestination(targetDestination);
            if (!nma.pathPending)
            {
                if (nma.hasPath)
                {
                    if (nma.remainingDistance != Mathf.Infinity)
                    {
                        if (nma.remainingDistance < nma.stoppingDistance)
                        {
                            //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
                            Quaternion rotation = Quaternion.LookRotation(heading);
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                        }
                    }
                }
            }
        }

        if (currentCommand == "Construct")
        {
            localTarget.position = target.position;
            Vector3 heading = target.position - transform.position;
            heading.y = 0;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            Vector3 targetDestination = target.position - direction;
            nma.SetDestination(targetDestination);

                if (!Structure.GetComponent<BuildingState>().buildingCompleted)
            {
                if (nma.pathPending)
                {

                }
                else
                {
                    if (nma.hasPath)
                    {
                        if(nma.remainingDistance != Mathf.Infinity)
                        {
                            if(nma.remainingDistance < nma.stoppingDistance + 1)
                            {
                                //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
                                Quaternion rotation = Quaternion.LookRotation(heading);
                                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                                insideDistance = true;
                            }
                            else
                            {
                                insideDistance = false;
                            }
                        }
                    }
                    if (insideDistance)
                    {
                        if (!Structure.GetComponent<BuildingState>().worker.Contains(gameObject))
                        {
                            Structure.GetComponent<BuildingState>().worker.Add(gameObject);
                        }
                        Structure.GetComponent<BuildingState>().Construction(ConstructionSpeed);
                        currentEnergy -= 2 * Time.deltaTime;
                        if (!UnderConstruction)
                        {
                            UnderConstruction = true;
                        }
                    }
                    else
                    {
                        if (UnderConstruction)
                        {
                            UnderConstruction = false;
                        }
                    }
                }
            }
            else
            {
                if (UnderConstruction)
                {
                    UnderConstruction = false;
                }
                currentCommand = "Idle";
            }
        }

        if (currentCommand == "Harvest")
        {
            localTarget.position = target.position;
            Vector3 heading = target.position - transform.position;
            heading.y = 0;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            Vector3 targetDestination = target.position - direction;
            nma.SetDestination(targetDestination);

                if (Resources.GetComponent<OreNode>().CanHarvest)
            {
                if (!Harvested)
                {
                    if (nma.pathPending)
                    {
                    }
                    else
                    {
                        if (nma.hasPath)
                        {
                            if (nma.remainingDistance != Mathf.Infinity)
                            {
                                if (nma.remainingDistance < nma.stoppingDistance + 1)
                                {
                                    if (!insideDistance)
                                    {
                                        //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
                                        Quaternion rotation = Quaternion.LookRotation(heading);
                                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                                        insideDistance = true;
                                    }
                                }
                                else
                                {
                                    insideDistance = false;
                                }
                            }
                        }
                    }
                    if (insideDistance)
                    {
                        if (Resources.GetComponent<OreNode>().CanHarvest)
                        {
                            if (!Harvesting)
                            {
                                Harvesting = true;
                            }
                            if (HarvestProgress < 100)
                            {
                                HarvestProgress += HarvestSpeed * Time.deltaTime;
                                currentEnergy -= 2 * Time.deltaTime;
                            }
                            else
                            {
                                insideDistance = false;
                                if (Harvesting)
                                {
                                    Harvesting = false;
                                }
                                if (!Harvested)
                                {
                                    Resources.GetComponent<OreNode>().Harvesting((int)HarvestPerTime, holdingPosition);
                                    item = holdingPosition.GetChild(0).gameObject;
                                    if (item.GetComponent<Rigidbody>())
                                    {
                                        item.GetComponent<Rigidbody>().useGravity = false;
                                        item.GetComponent<Rigidbody>().isKinematic = true;
                                    }
                                    if(item != null)
                                    {
                                        holding = true;
                                    }
                                    Harvested = true;
                                    HarvestProgress = 0;
                                }
                                if (Storage != null)
                                {
                                    localPoint = Vector3.zero;
                                    target = Storage.transform;
                                    localTarget.position = target.position;
                                    currentCommand = "Transport";
                                    if (Harvesting)
                                    {
                                        Harvesting = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Harvesting)
                            {
                                Harvesting = false;
                            }
                            HarvestProgress = 0;
                            currentCommand = "Idle";
                        }
                    }
                }
                else
                {
                    currentCommand = "Idle";
                }
            }
            else
            {
                if (!Harvested)
                {
                    currentCommand = "Idle";
                    nma.SetDestination(transform.position);
                }
            }
        }

        if (currentCommand == "Transport")
        {
            //https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
            localTarget.position = target.position;
            Vector3 heading = target.position - transform.position;
            heading.y = 0;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            Vector3 targetDestination = target.position - direction;
            nma.SetDestination(targetDestination);
            if (!placed)
                {
                    if (nma.pathPending)
                    {

                    }
                    else
                    {
                        if (nma.hasPath)
                        {
                            if (nma.remainingDistance != Mathf.Infinity)
                            {
                                if (nma.remainingDistance <= nma.stoppingDistance + 1f)
                                {
                                    //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
                                    Quaternion rotation = Quaternion.LookRotation(heading);
                                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                                    if (item.GetComponent<localOre>())
                                        {
                                            ResourceController.instance.FilterResource(item.GetComponent<localOre>().profile).nrAmount += item.GetComponent<localOre>().localAmount;
                                            Destroy(item);
                                            item = null;
                                            if (item == null)
                                            {
                                                holding = false;
                                            }
                                            placed = true;
                                            if (Harvested)
                                            {
                                                Harvested = false;
                                            }

                                            if(Resources != null)
                                            {
                                                if (Resources.GetComponent<OreNode>().CanHarvest)
                                                {
                                                    localPoint = Vector3.zero;
                                                    target = Resources.transform;
                                                    localTarget.position = target.position;
                                                    currentCommand = "Harvest";
                                                    placed = false;
                                                }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (item == null)
                    {
                        currentCommand = "Idle";
                        if (placed)
                        {
                            placed = false;
                        }
                    }
                }
            }

        if(currentCommand == "Recharging")
        {
            localTarget.position = target.position;
            Vector3 heading = target.position - transform.position;
            heading.y = 0;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            Vector3 targetDestination = target.position - direction;
            nma.SetDestination(targetDestination);
            
            if (currentEnergy <= BatteryCapacity)
            {
                if (!nma.pathPending)
                {
                    if (nma.hasPath)
                    {
                        if(nma.remainingDistance <= nma.stoppingDistance)
                        {
                            //https://answers.unity.com/questions/161053/making-an-object-rotate-to-face-another-object.html
                            Quaternion rotation = Quaternion.LookRotation(heading);
                            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                            if (!Recharging)
                            {
                                Recharging = true;
                            }
                        }
                        else
                        {
                            if (Recharging)
                            {
                                Recharging = false;
                            }
                        }
                    }
                }
                if (Recharging)
                {
                    currentEnergy += 5 * Time.deltaTime;
                }
            }
            else
            {
                currentCommand = "Idle";
            }
        }

        if (wu.workerCommand != currentCommand)
        {
            wu.workerCommand = currentCommand;
        }

        if (target != null)
        {
            if (target.GetComponent<BuildingUI>() || target.GetComponent<WorkerUI>())
            {
                if (target.GetComponent<BuildingUI>())
                {
                    if (wu.workerDestination != target.GetComponent<BuildingUI>().TermNoun)
                    {
                        wu.workerDestination = target.GetComponent<BuildingUI>().TermNoun;
                    }
                }
                if (target.GetComponent<WorkerUI>())
                {
                    if (wu.workerDestination != target.GetComponent<WorkerUI>().workerName)
                    {
                        wu.workerDestination = target.GetComponent<WorkerUI>().workerName;
                    }
                }
            }
            else
            {
                if(wu.workerDestination != target.gameObject.name)
                {
                    wu.workerDestination = target.gameObject.name;
                }
            }
        }
        else
        {
            if (wu.workerDestination != "Nothing")
            {
                wu.workerDestination = "Nothing";
            }
        }
        if (item != null)
        {
            if (wu.workerCarry != item.name)
            {
                wu.workerCarry = item.name;
            }
        }
        else
        {
            if (wu.workerCarry != "Nothing")
            {
                wu.workerCarry = "Nothing";
            }
        }

    }

    public void HarvestOre(Transform targetTransform)
    {
        Resources = targetTransform.gameObject;
        localPoint = Vector3.zero;
        target = targetTransform;
        localTarget.position = target.position;
        currentCommand = "Harvest";
    }

    public void FollowTarget(Transform targetTransform)
    {
        localPoint = Vector3.zero;
        target = targetTransform;
        localTarget.position = target.position;
        currentCommand = "Follow";
    }

    public void MoveToPosition(Vector3 targetPos)
    {
        if (target != null)
        {
            target = null;
        }
        localPoint = targetPos;
        localTarget.position = localPoint;
        currentCommand = "Move";
        if (Structure != null)
        {
            Structure = null;
        }
        if (Storage != null)
        {
            Storage = null;
        }
    }

    private Transform nearestTransform(Transform[] target)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in target)
        {
            if (potentialTarget != null)
            {
                if (potentialTarget.GetComponent<BuildingState>())
                {
                    if (potentialTarget.GetComponent<BuildingState>().buildingCompleted)
                    {
                        Vector3 directionToTarget = potentialTarget.position - currentPosition;
                        float dSqrToTarget = directionToTarget.sqrMagnitude;
                        if (dSqrToTarget < closestDistanceSqr)
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }
                }
                else
                {
                    Vector3 directionToTarget = potentialTarget.position - currentPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = potentialTarget;
                    }
                }
            }

        }
        return bestTarget;
    }
    private void OnDrawGizmos()
    {
        if (nma != null && nma.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(nma.destination, nma.destination + (Vector3.up * 5));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(nma.destination + (Vector3.up * 5), 0.1f);
        }
    }

    private void OnGUI()
    {
        if (selection.BeingSelected)
        {
            if (showGUI)
            {
                GUI.Box(boxRect, (nma.remainingDistance).ToString("F2") + "/" + (nma.stoppingDistance).ToString("F2"));
            }
        }
    }
}
