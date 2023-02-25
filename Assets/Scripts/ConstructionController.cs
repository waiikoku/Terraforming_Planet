using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public static ConstructionController instance;
    public bool CurrentBuilded = false;
    private Vector3 lastPos;
    public BuildingConstruction[] bc;
    public BuildingConstruction currentBC;
    private Vector3 placepos;
    public Material prebuildMat;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void Construct(BuildingConstruction bc)
    {
        Debug.Log("Place preview");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit);
        if (isHit)
        {
            if (hit.transform.CompareTag("Ground"))
            {
                CurrentBuilded = false;
                placepos = hit.point;
                GameObject building = Instantiate(bc.prefab, placepos, Quaternion.identity);
                building.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                lastPos = building.transform.position;
                Debug.Log("Raycast hit ground And Place preview building");
                if (!building.GetComponent<BuildingState>())
                {
                    building.AddComponent<BuildingState>();
                }
                building.GetComponent<BuildingState>().bcprofile = bc;
                if (building.GetComponent<EletricalSystem>())
                {
                    EletricalSystemController.instance.AssignGenerator(building);
                }
            }
        }

    }

    private void Destruct()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit);
        if (isHit)
        {
            if (hit.transform.gameObject.GetComponent<BuildingState>())
            {
                if (hit.transform.gameObject.GetComponent<BuildingState>().placementPreview)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.transform.gameObject.GetComponent<BuildingState>().returnResources();
                        Destroy(hit.transform.gameObject);
                    }
                    return;
                }
                if (hit.transform.gameObject.GetComponent<BuildingState>().buildingCompleted)
                {
                    return;
                }
            }
        }
    }
}
