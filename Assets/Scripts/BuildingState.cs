using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingState : MonoBehaviour
{
    public BuildingUI bu;
    public BuildingInfo bi;


    [Header("Setup")]
    [SerializeField] private bool isReadySetup = false;
    [SerializeField] private bool isTransparent = false;
    [SerializeField] private bool isOriginMat = false;
    private bool freespace = true;
    public bool placementPreview = false;
    public List<Transform> expception;
    public List<Transform> childrenTransform;
    public List<Transform> grandchildrenTransform;
    public List<MeshFilter> mfs;
    public List<MeshRenderer> mrs;
    public List<SkinnedMeshRenderer> smrs;
    public List<Material> mats;
    [SerializeField] private Material tempMat;
    public float buildingSize;
    public Vector3 buildingScale;
    public Vector3[] CornerPos;
    [SerializeField] private NavMeshObstacle nmo;
    [SerializeField] private GameObject ctSign;

    [Header("building progress")]
    public bool buildingCompleted = false;
    public bool isBuildable = false;
    public List<GameObject> worker;
    public bool underConstruction = false;
    public BuildingConstruction bcprofile;
    public float currentCT = 0;
    public float currentCP = 0;

// Start is called before the first frame update
    void Start()
    {
        if (GetComponent<NavMeshObstacle>())
        {
            if(nmo == null)
            {
                nmo = GetComponent<NavMeshObstacle>();
            }
            if (!buildingCompleted)
            {
                nmo.enabled = false;
            }
            else
            {
                nmo.enabled = true;
            }
        }
        if (CornerPos.Length > 0)
        {
            for (int i = 0; i < CornerPos.Length; i++)
            {
                CornerPos[i] = new Vector3((buildingScale.x / 2) * 1, 0, (buildingScale.z / 2) * 1);
            }
        }
        foreach (Transform item in transform)
        {
            if (!childrenTransform.Contains(item))
            {
                childrenTransform.Add(item);
            }
        }
        if (GetComponent<SkinnedMeshRenderer>())
        {
            smrs.Add(GetComponent<SkinnedMeshRenderer>());
        }
        else
        {
            if (GetComponent<MeshFilter>())
            {
                mfs.Add(GetComponent<MeshFilter>());
            }
            if (GetComponent<MeshRenderer>())
            {
                mrs.Add(GetComponent<MeshRenderer>());
            }
        }

        if(childrenTransform.Count > 0)
        {
            for (int i = 0; i < childrenTransform.Count; i++)
            {
                foreach (Transform item in childrenTransform[i])
                {
                    if (!grandchildrenTransform.Contains(item))
                    {
                        grandchildrenTransform.Add(item);
                    }
                }
                if (childrenTransform[i].GetComponent<MeshFilter>())
                {
                    if (!mfs.Contains(childrenTransform[i].GetComponent<MeshFilter>()))
                    {
                        mfs.Add(childrenTransform[i].GetComponent<MeshFilter>());
                    }
                }
                if (childrenTransform[i].GetComponent<MeshRenderer>())
                {
                    if (!mrs.Contains(childrenTransform[i].GetComponent<MeshRenderer>()))
                    {
                        mrs.Add(childrenTransform[i].GetComponent<MeshRenderer>());
                    }
                }
                if (childrenTransform[i].GetComponent<SkinnedMeshRenderer>())
                {
                    if (!smrs.Contains(childrenTransform[i].GetComponent<SkinnedMeshRenderer>()))
                    {
                        smrs.Add(childrenTransform[i].GetComponent<SkinnedMeshRenderer>());
                    }
                }
            }
        }
        if(grandchildrenTransform.Count > 0)
        {
            for (int i = 0; i < grandchildrenTransform.Count; i++)
            {
                if (grandchildrenTransform[i].GetComponent<MeshFilter>())
                {
                    if (!mfs.Contains(grandchildrenTransform[i].GetComponent<MeshFilter>()))
                    {
                        mfs.Add(grandchildrenTransform[i].GetComponent<MeshFilter>());
                    }
                }
                if (grandchildrenTransform[i].GetComponent<MeshRenderer>())
                {
                    if (!mrs.Contains(grandchildrenTransform[i].GetComponent<MeshRenderer>()))
                    {
                        mrs.Add(grandchildrenTransform[i].GetComponent<MeshRenderer>());
                    }
                }
                if (grandchildrenTransform[i].GetComponent<SkinnedMeshRenderer>())
                {
                    if (!smrs.Contains(grandchildrenTransform[i].GetComponent<SkinnedMeshRenderer>()))
                    {
                        smrs.Add(grandchildrenTransform[i].GetComponent<SkinnedMeshRenderer>());
                    }
                }
            }
        }
        if(mfs.Count > 0)
        {

        }
        if(mrs.Count > 0)
        {
            for (int i = 0; i < mrs.Count; i++)
            {
                Material[] newMat;
                newMat = mrs[i].materials;
                if(newMat.Length > 1)
                {
                    for (int x = 0; x < newMat.Length; x++)
                    {
                        if (!mats.Contains(newMat[x]))
                        {
                            mats.Add(newMat[x]);
                        }
                    }
                }
                else
                {
                    if (!mats.Contains(newMat[0]))
                    {
                        mats.Add(newMat[0]);
                    }
                }
            }

        }
        if(smrs.Count > 0)
        {
            for (int i = 0; i < smrs.Count; i++)
            {
                Material[] newMat;
                newMat = smrs[i].materials;
                if(newMat.Length > 1)
                {
                    for (int x = 0; x < newMat.Length; x++)
                    {
                        if (!mats.Contains(newMat[x]))
                        {
                            mats.Add(newMat[x]);
                        }
                    }
                }
                else
                {
                    if (!mats.Contains(newMat[0]))
                    {
                        mats.Add(newMat[0]);
                    }
                }
            }
        }

        buildingScale = GetComponent<Collider>().bounds.size;
        buildingSize = buildingScale.x * buildingScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            AssignMaterial(tempMat);
            Debug.Log(gameObject.name + " Assign prebuild mat");
        }
        if (!isReadySetup)
        {
            isReadySetup = true;
            tempMat = ConstructionController.instance.prebuildMat;
            mrs[0].material = tempMat;
            tempMat = mrs[0].material;
            mrs[0].material = mats[0];
        }
        if (!isTransparent && isReadySetup)
        {
            AssignMaterial(tempMat);
            //Debug.Log("Assign prebuild mat");
            isTransparent = true;
        }
        if (!placementPreview)
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit);
            if (isHit)
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    transform.position = hit.point;
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }
            
            if (freespace)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (CheckResourceToBuild(bcprofile))
                    {
                        for (int i = 0; i < bcprofile.require.Length; i++)
                        {
                            ResourceController.instance.FilterResource(bcprofile.require[i]).nrAmount -= bcprofile.cost[i];
                        }
                        if (gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
                        {
                            gameObject.layer = LayerMask.NameToLayer("Default");
                        }
                        if (childrenTransform.Count > 0)
                        {
                            for (int a = 0; a < childrenTransform.Count; a++)
                            {
                                if (!expception.Contains(childrenTransform[a]))
                                {
                                    if (childrenTransform[a].gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
                                    {
                                        childrenTransform[a].gameObject.layer = LayerMask.NameToLayer("Default");
                                    }
                                }
                            }
                        }
                        if (grandchildrenTransform.Count > 0)
                        {
                            for (int b = 0; b < grandchildrenTransform.Count; b++)
                            {
                                if (!expception.Contains(grandchildrenTransform[b]))
                                {
                                    if (grandchildrenTransform[b].gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
                                    {
                                        grandchildrenTransform[b].gameObject.layer = LayerMask.NameToLayer("Default");
                                    }
                                }
                            }
                        }
                        tempMat.SetColor("_Color", Color.white);
                        placementPreview = true;
                        GetComponent<Collider>().isTrigger = false;
                        if (GetComponent<BoxCollider>())
                        {
                            GetComponent<BoxCollider>().enabled = false;
                        }
                        ConstructionController.instance.CurrentBuilded = true;
                        if (!GetComponent<BuildingInfo>())
                        {
                            gameObject.AddComponent<BuildingInfo>();
                        }
                        if (!GetComponent<BuildingUI>())
                        {
                            gameObject.AddComponent<BuildingUI>();
                        }
                        bu.icon = bcprofile.buildingIcon;
                        GetComponent<BuildingInfo>().AssignProfile(bcprofile);
                    }
                    else
                    {
                        Debug.Log("Desire to build there still not enough resources");
                    }
                }
            }
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(gameObject);
                ConstructionController.instance.CurrentBuilded = true;
            }
        }
        else
        {
            if (!buildingCompleted)
            {
                bu.timer = (currentCT / bcprofile.constructTime);
                if (CheckWorker())
                {
                    if (CheckWorking() >= 1)
                    {
                        underConstruction = true;
                    }
                    else
                    {
                        underConstruction = false;
                    }
                }
                else
                {
                    if (underConstruction)
                    {
                        underConstruction = false;
                    }
                }
                if (currentCT >= bcprofile.constructTime || currentCP >= bcprofile.constructPoint)
                {
                    buildingCompleted = true;
                }
            }
        }
        if (buildingCompleted && !isOriginMat)
        {
            if (underConstruction)
            {
                underConstruction = false;
                if(nmo != null && nmo.enabled == false)
                {
                    nmo.enabled = true;
                }  
                bi.Completed();
            }
            AssignOriginalMaterial(mats);
        }
        if (buildingCompleted && ctSign.activeInHierarchy)
        {
            ctSign.SetActive(false);
            //Debug.Log("Set Construction Warning Signs Off");
        }

    }

    public void Construction(float amount)
    {
        currentCT += amount * Time.deltaTime;
    }

    public bool CheckWorker()
    {
        if(worker.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < worker.Count; i++)
        {
            if (worker[i] == null)
            {
                worker.RemoveAt(i);
            }
            else
            {
                if (worker[i].GetComponent<Worker>())
                {
                    if (worker[i].GetComponent<Worker>().Structure != transform.gameObject)
                    {
                        worker.RemoveAt(i);
                    }
                }
            }
            /*
            if (worker[i].GetComponent<SpaceConstructionVehicle>())
            {
                if (worker[i].GetComponent<SpaceConstructionVehicle>().currentTargetTransform == transform)
                {

                }
                else
                {
                    worker.RemoveAt(i);
                }
            }
            */

        }
        for (int f = 0; f < worker.Count; f++)
        {
            if (worker[f].GetComponent<Worker>())
            {
                if (worker[f].GetComponent<Worker>().Structure == transform.gameObject)
                {
                    return true;
                }
            }
            /*
            if (worker[f].GetComponent<SpaceConstructionVehicle>())
            {
                if (worker[f].GetComponent<SpaceConstructionVehicle>().currentTargetTransform == transform)
                {
                    return true;
                }
            }
            */

        }
        return false;
    }

    private int CheckWorking()
    {
        int currentWorking = 0;
        for (int i = 0; i < worker.Count; i++)
        {
            if (worker[i].GetComponent<Worker>())
            {
                if (worker[i].GetComponent<Worker>().UnderConstruction)
                {
                    currentWorking += 1;
                }
                else
                {

                }
            }
        }
        //Debug.Log(currentWorking);
        return currentWorking;
    }

    public void returnResources()
    {
        for (int i = 0; i < bcprofile.require.Length; i++)
        {
            ResourceController.instance.FilterResource(bcprofile.require[i]).nrAmount += bcprofile.cost[i];
        }
        return;
    }

    private bool CheckResourceToBuild(BuildingConstruction bc)
    {
        if(bc.require.Length == 1)
        {
            if(ResourceController.instance.FilterResource(bc.require[0]).nrAmount >= bc.cost[0])
            {

            }
            else
            {
                return false;
            }

        }
        if(bc.require.Length > 1)
        {
            for (int i = 0; i < bc.require.Length; i++)
            {
                if (ResourceController.instance.FilterResource(bc.require[i]).nrAmount >= bc.cost[i])
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

    private void AssignOriginalMaterial(List<Material> mats)
    {
        if (!isReadySetup)
        {
            return;
        }
        int currentIndex = 0;
        if (mrs.Count > 0)
        {
            for (int i = 0; i < mrs.Count; i++)
            {
                if (!expception.Contains(mrs[i].transform))
                {
                    if (mrs[i].materials.Length == 1)
                    {
                        //Debug.Log("Assign :" + currentIndex);
                        mrs[i].material = mats[currentIndex];
                        currentIndex++;
                        //Debug.Log("Increase :" + currentIndex);
                    }
                    else
                    {
                        Material[] newMat = new Material[mrs[i].materials.Length];
                        for (int nm = 0; nm < newMat.Length; nm++)
                        {
                            //Debug.Log("Assign :" + currentIndex);
                            newMat[nm] = mats[currentIndex];
                            currentIndex++;
                            //Debug.Log("Increase :" + currentIndex);
                        }
                        mrs[i].materials = newMat;
                    }
                }
                else
                {
                    currentIndex++;
                    //Debug.Log("Increase :" + currentIndex);
                }
            }
        }
        if(smrs.Count > 0)
        {
            for (int i = 0; i < smrs.Count; i++)
            {
                if (!expception.Contains(smrs[i].transform))
                {
                    if (smrs[i].materials.Length == 1)
                    {
                        Debug.Log("Assign :" + currentIndex);
                        smrs[i].material = mats[currentIndex];
                        currentIndex++;
                        Debug.Log("Increase :" + currentIndex);
                    }
                    else
                    {
                        Material[] newMat = new Material[smrs[i].materials.Length];
                        for (int nm = 0; nm < newMat.Length; nm++)
                        {
                            Debug.Log("Assign :" + currentIndex);
                            newMat[nm] = mats[currentIndex];
                            currentIndex++;
                            Debug.Log("Increase :" + currentIndex);
                        }
                        smrs[i].materials = newMat;
                    }
                }
                else
                {
                    currentIndex++;
                    Debug.Log("Increase :" + currentIndex);
                }
            }
        }
        isOriginMat = true;
    }

    public void AssignMaterial(Material mat)
    {
        if (smrs.Count > 0)
        {
            for (int i = 0; i < smrs.Count; i++)
            {
                if (!expception.Contains(smrs[i].transform))
                {
                    Material[] newMat;
                    newMat = new Material[smrs[i].materials.Length];
                    for (int b = 0; b < newMat.Length; b++)
                    {
                        newMat[b] = mat;
                    }
                    Debug.Log("SkinMeshRenderer Length :" + smrs[i].materials.Length);
                    if (smrs[i].materials.Length > 1)
                    {
                        smrs[i].materials = newMat;
                        Debug.Log("Assign Array Mat");
                    }
                    else
                    {
                        smrs[i].material = newMat[0];
                        Debug.Log("Assign Single Mat");
                    }

                }
            }
        }
        if (mfs.Count > 0)
        {

        }
        if (mrs.Count > 0)
        {
            for (int i = 0; i < mrs.Count; i++)
            {
                if (!expception.Contains(mrs[i].transform))
                {
                    Material[] newMat;
                    newMat = new Material[mrs[i].materials.Length];
                    for (int b = 0; b < newMat.Length; b++)
                    {
                        newMat[b] = mat;
                    }
                    //Debug.Log("MeshRenderer Length :" + mrs[i].materials.Length);
                    if (mrs[i].materials.Length > 1)
                    {
                        mrs[i].materials = newMat;
                        //Debug.Log("Assign Array Mat");
                    }
                    else
                    {
                        mrs[i].material = newMat[0];
                        //Debug.Log("Assign Single Mat");
                    }
                }
            }

        }
    }
    
    bool CheckMaterial(Material mat)
    {
        if (smrs.Count > 0)
        {
            for (int i = 0; i < smrs.Count; i++)
            {
                if (!expception.Contains(smrs[i].transform))
                {
                    Material[] newMat;
                    newMat = new Material[smrs[i].materials.Length];
                    for (int b = 0; b < newMat.Length; b++)
                    {
                        newMat[b] = mat;
                    }
                    if (smrs[i].materials == newMat)
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            if (mfs.Count > 0)
            {

            }
            if (mrs.Count > 0)
            {
                for (int i = 0; i < mrs.Count; i++)
                {
                    if (!expception.Contains(mrs[i].transform))
                    {
                        Material[] newMat;
                        newMat = new Material[mrs[i].materials.Length];
                        for (int b = 0; b < newMat.Length; b++)
                        {
                            newMat[b] = mat;
                        }
                        if (mrs[i].materials == newMat)
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }
        }
        return true;
    }

    #region collision
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (!placementPreview && isReadySetup)
        {
            if (!collision.transform.CompareTag("Ground"))
            {
                tempMat.SetColor("_Color", Color.red);
                freespace = false;
                Debug.Log("Cant Build Here");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!placementPreview && isReadySetup)
        {
            if (!collision.transform.CompareTag("Ground"))
            {
                tempMat.SetColor("_Color", Color.green);
                freespace = true;
                Debug.Log("Can Build Here");
            }
        }
    }
    */
    #endregion

    #region trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!placementPreview && isReadySetup)
        {
            if (!other.transform.CompareTag("Ground"))
            {
                tempMat.SetColor("_Color", Color.red);
                freespace = false;
                Debug.Log("Cant Build Here");
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!placementPreview && isReadySetup)
        {
            if (!other.transform.CompareTag("Ground"))
            {
                tempMat.SetColor("_Color", Color.green);
                freespace = true;
                Debug.Log("Can Build Here");
            }
        }
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if (gameObject.GetComponent<MeshFilter>())
        {
            Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, 0, transform.position, transform.rotation, transform.localScale);
        }
        if (gameObject.GetComponent<SkinnedMeshRenderer>())
        {
            Gizmos.DrawWireMesh(GetComponent<SkinnedMeshRenderer>().sharedMesh, 0, transform.position, transform.rotation, transform.localScale);
        }
        if(childrenTransform.Count > 0)
        {
            for (int ct = 0; ct < childrenTransform.Count; ct++)
            {
                if (childrenTransform[ct].GetComponent<MeshFilter>())
                {
                    Gizmos.DrawWireMesh(childrenTransform[ct].GetComponent<MeshFilter>().sharedMesh, 0, childrenTransform[ct].position, childrenTransform[ct].rotation, Vector3.one);
                }
                if (childrenTransform[ct].GetComponent<SkinnedMeshRenderer>())
                {
                    Gizmos.DrawWireMesh(childrenTransform[ct].GetComponent<SkinnedMeshRenderer>().sharedMesh, 0, childrenTransform[ct].position, childrenTransform[ct].rotation, Vector3.one);
                }
            }
        }
        if(grandchildrenTransform.Count > 0)
        {
            for (int gct = 0; gct < grandchildrenTransform.Count; gct++)
            {
                if (grandchildrenTransform[gct].GetComponent<MeshFilter>())
                {
                    Gizmos.DrawWireMesh(grandchildrenTransform[gct].GetComponent<MeshFilter>().sharedMesh, 0, grandchildrenTransform[gct].position, grandchildrenTransform[gct].rotation, Vector3.one);
                }
                if (grandchildrenTransform[gct].GetComponent<SkinnedMeshRenderer>())
                {
                    Gizmos.DrawWireMesh(grandchildrenTransform[gct].GetComponent<SkinnedMeshRenderer>().sharedMesh, 0, grandchildrenTransform[gct].position, grandchildrenTransform[gct].rotation, Vector3.one);
                }
            }
        }
    }
}
