using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCenter : MonoBehaviour
{
    public Selection select;
    [SerializeField] private GameObject selector;

    [Header("Stat & Status")]
    public int currentWorker = 0;
    public int maximumWorker = 16;
    public int currentHealth = 100;
    public int maximumHealth = 100;
    public int currentShield = 100;
    public int ShieldCapacity = 100;
    public int currentEnergy = 100;
    public int EnergyCapacity = 100;

    [Header("Settings")]
    public GameObject scv;
    public float CollectResouceRange = 5f;
    public Transform spawnPosition;
    [SerializeField] private Transform defaultSpawnPosition;
    private int currentSpawned = 0;
    public int resPerScv = 50;
    public float trainTime = 15f;

    [Header("Functions")]


    public List<string> Queued;
    public List<GameObject> StructureList;
    public List<GameObject> UnitList;
    public List<string> Skills;
    public List<string> Upgrades;

    [Header("Natural Resources")]
    public NaturalResources nr;

    [Header("Gizmos")]
    public Color rallypoint_col = Color.white;

    private void Update()
    {
        if (select.BeingSelected)
        {
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    spawnPosition.position = hit.point;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                TrainSCV();
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

    public void TrainSCV()
    {
        if(ResourceController.instance.FilterResource(nr).nrAmount > 0)
        {
            if (ResourceController.instance.FilterResource(nr).nrAmount - resPerScv >= 0)
            {
                ResourceController.instance.FilterResource(nr).nrAmount -= resPerScv;
                GameObject SCV;
                if(Physics.OverlapSphere(spawnPosition.position,1f).Length > 0)
                {
                    SCV = Instantiate(scv, spawnPosition.position + new Vector3(Random.Range(0.0f,1.0f),0, Random.Range(0.0f, 1.0f)),Quaternion.identity);
                }
                else
                {
                    SCV = Instantiate(scv, spawnPosition.position, Quaternion.identity);
                }

                SCV.GetComponent<SpaceConstructionVehicle>().currentCommand = "Idle";
                currentSpawned++;
                SCV.name = "SCV No." + currentSpawned;
            }
        }
    }

    public void UpgradeStation()
    {
        Debug.Log("Upgraded!");
    }

    Vector3 RandomAround(Vector3 center,float radius)
    {
        float randomX = Random.Range(radius,radius + 1.0f);
        float randomZ = Random.Range(radius, radius + 1.0f);
        Vector3 newPos = new Vector3(randomX, 0, randomZ);
        Vector3 result = center + newPos;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position,Vector3.one * CollectResouceRange);
        Gizmos.color = rallypoint_col;
        Gizmos.DrawLine(transform.position, spawnPosition.position);
    } 
}
