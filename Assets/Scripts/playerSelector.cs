using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSelector : MonoBehaviour
{
    public static playerSelector instance;

    public GameObject currentSelect;
    public List<GameObject> multiSelect;

    [Header("Mouse Input")]
    [Range(0.01f,1.00f)]
    [SerializeField] private float GlobalMouseDelay = 0.5f;
    [SerializeField] private bool useGlobalMouseDelay = false;

    private int left_clicked = 0;
    private float left_clicktime = 0f;
    private float left_clickdelay = 0.5f;
    private int middle_clicked = 0;
    private float middle_clicktime = 0f;
    private float middle_clickdelay = 0.5f;
    private int right_clicked = 0;
    private float right_clicktime = 0f;
    private float right_clickdelay = 0.5f;

    [SerializeField] private Vector3 mousePos1;
    [SerializeField] private Vector3 mousePos2;
    public List<GameObject> selectableObjects;
    [SerializeField] private List<GameObject> selectingObject;
    [SerializeField] private List<GameObject> selectedObject;

    public bool MultiSelect;

    private bool toggleMenu;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (useGlobalMouseDelay)
        {
            left_clickdelay = GlobalMouseDelay;
            middle_clickdelay = GlobalMouseDelay;
            right_clickdelay = GlobalMouseDelay;
        }
    }

    private void Update()
    {
        MultiSelect = selectedObject.Count > 1 || multiSelect.Count > 1;
        if(currentSelect != null)
        {
            if (!UIController.instance.SingleSelection.activeInHierarchy)
            {
                UIController.instance.SingleSelection.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape) && ConstructionController.instance.CurrentBuilded)
            {
                currentSelect = null;
            }
        }
        else
        {
            if (UIController.instance.SingleSelection.activeInHierarchy)
            {
                UIController.instance.SingleSelection.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                toggleMenu = !toggleMenu;

                UIController.instance.Menu.SetActive(toggleMenu);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!ConstructionController.instance.CurrentBuilded)
            {
                return;
            }
            if (mouseEvent.instance.OnUI)
            {
                return;
            }
            mousePos1 = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit,Mathf.Infinity);
            if (isHit)
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    UIController.instance.CurrentOdp = null;
                    UIController.instance.CurrentOdpType = UIController.ODP_Type.Generator;
                    ClearSelection();
                    if (!mouseEvent.instance.OnUI)
                    {
                        if (currentSelect != null)
                        {
                            if (currentSelect.GetComponent<Selection>())
                            {
                                currentSelect.GetComponent<Selection>().BeingSelected = false;
                            }
                            currentSelect = null;
                        }
                        if (multiSelect.Count > 0)
                        {
                            for (int i = 0; i < multiSelect.Count; i++)
                            {
                                if (multiSelect[i].GetComponent<Selection>())
                                {
                                    multiSelect[i].GetComponent<Selection>().BeingSelected = false;
                                }
                            }
                            multiSelect.Clear();
                        }
                    }
                }
                if (hit.transform.GetComponent<Selection>())
                {
                    if (currentSelect != null)
                    {
                        currentSelect.GetComponent<Selection>().BeingSelected = false;
                        if (multiSelect.Count > 0)
                        {
                            for (int i = 0; i < multiSelect.Count; i++)
                            {
                                if (multiSelect[i].GetComponent<Selection>())
                                {
                                    multiSelect[i].GetComponent<Selection>().BeingSelected = false;
                                }
                            }
                            multiSelect.Clear();
                        }
                    }
                    ClearSelection();
                    currentSelect = hit.transform.gameObject;
                    currentSelect.GetComponent<Selection>().BeingSelected = true;
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            mousePos2 = Input.mousePosition;

            if(mousePos1 != mousePos2)
            {
                SelectObject();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (selectingObject.Count > 0)
            {
                ConfirmSelection();
            }
        }
        //For Source Code Double Click https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
        if (DoubleLeftClick())
        {
            if (!ConstructionController.instance.CurrentBuilded)
            {
                return;
            }
            //Debug.Log("Double Left Clicked!");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool isHit = Physics.Raycast(ray, out hit);
            if (isHit)
            {
                if (hit.transform.GetComponent<SpaceConstructionVehicle>())
                {
                    SpaceConstructionVehicle[] scv = FindObjectsOfType<SpaceConstructionVehicle>();
                    foreach (SpaceConstructionVehicle item in scv)
                    {
                        if (item.GetComponent<BeenSeen>() != null && item.GetComponent<BeenSeen>().OnScreen)
                        {
                            if (!multiSelect.Contains(item.gameObject))
                            {
                                multiSelect.Add(item.gameObject);
                            }
                        }
                    }
                }
                if (hit.transform.GetComponent<Worker>())
                {
                    Worker[] worker = FindObjectsOfType<Worker>();
                    foreach (Worker item in worker)
                    {
                        if (item.GetComponent<BeenSeen>() != null && item.GetComponent<BeenSeen>().OnScreen)
                        {
                            if (!multiSelect.Contains(item.gameObject))
                            {
                                multiSelect.Add(item.gameObject);
                            }
                        }
                    }
                }
            }
            if (multiSelect.Count > 0)
            {
                for (int i = 0; i < multiSelect.Count; i++)
                {
                    if (multiSelect[i].GetComponent<Selection>())
                    {
                        multiSelect[i].GetComponent<Selection>().BeingSelected = true;
                    }
                }
            }
        }
        if (DoubleRightClick())
        {
            //Debug.Log("Double Right Clicked!");
        }
        if (DoubleMiddleMouseClick())
        {
            //Debug.Log("Double Middle Mouse Clicked!");
        }
    }

    private bool DoubleLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            left_clicked++;
            if (left_clicked == 1) left_clicktime = Time.time;
        }
        if (left_clicked > 1 && Time.time - left_clicktime < left_clickdelay)
        {
            left_clicked = 0;
            left_clicktime = 0;
            return true;
        }
        else if (left_clicked > 2 || Time.time - left_clicktime > 1) left_clicked = 0;
        return false;
    }

    private bool DoubleMiddleMouseClick()
    {
        if (Input.GetMouseButtonDown(2))
        {
            middle_clicked++;
            if (middle_clicked == 1) middle_clicktime = Time.time;
        }
        if (middle_clicked > 1 && Time.time - middle_clicktime < middle_clickdelay)
        {
            middle_clicked = 0;
            middle_clicktime = 0;
            return true;
        }
        else if (middle_clicked > 2 || Time.time - middle_clicktime > 1) middle_clicked = 0;
        return false;
    }

    private bool DoubleRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            right_clicked++;
            if (right_clicked == 1) right_clicktime = Time.time;
        }
        if (right_clicked > 1 && Time.time - right_clicktime < right_clickdelay)
        {
            right_clicked = 0;
            right_clicktime = 0;
            return true;
        }
        else if (right_clicked > 2 || Time.time - right_clicktime > 1) right_clicked = 0;
        return false;
    }

    private void SelectObject()
    {
        List<GameObject> remObject = new List<GameObject>();
        mousePos2 = new Vector3(Mathf.Clamp(mousePos2.x, 0, Screen.width), Mathf.Clamp(mousePos2.y, 0, Screen.height), 0);
        /*
        Rect selectRect = new Rect(centre.x, centre.y, sizeX, sizeY);
        */
        Rect selectRect = new Rect();
        if (mousePos2.x > mousePos1.x)
        {
            if (mousePos2.y > mousePos1.y) // > >
            {
                selectRect = new Rect(mousePos1.x, mousePos1.y, mouseEvent.SelectionBoxRect.width, mouseEvent.SelectionBoxRect.height);
            }
            if (mousePos2.y < mousePos1.y) // > <
            {
                selectRect = new Rect(mousePos1.x,mousePos2.y, mouseEvent.SelectionBoxRect.width, mouseEvent.SelectionBoxRect.height);
            }
        }
        if (mousePos2.x < mousePos1.x)
        {
            if (mousePos2.y > mousePos1.y) // < > Pass
            {
                selectRect = new Rect(mousePos2.x, mousePos1.y, mouseEvent.SelectionBoxRect.width, mouseEvent.SelectionBoxRect.height);
            }
            if (mousePos2.y < mousePos1.y) // < < Pass
            {
                selectRect = new Rect(mousePos2.x, mousePos2.y, mouseEvent.SelectionBoxRect.width, mouseEvent.SelectionBoxRect.height);
            }
        }

        foreach (GameObject selectObject in selectableObjects)
        {
            if(selectObject != null)
            {
                if (selectRect.Contains(Camera.main.WorldToScreenPoint(selectObject.transform.position), true))
                {
                    //Debug.Log(Camera.main.WorldToScreenPoint(selectObject.transform.position));
                    //Debug.Log("Object In Area");
                    if (!selectingObject.Contains(selectObject))
                    {
                        selectingObject.Add(selectObject);
                    }
                }
                else
                {
                    if (selectingObject.Contains(selectObject))
                    {
                        selectingObject.Remove(selectObject);
                    }
                }
            }
            else
            {
                remObject.Add(selectObject);
            }
        }

        if(remObject.Count > 0)
        {
            foreach(GameObject rem in remObject)
            {
                selectableObjects.Remove(rem);
            }
            remObject.Clear();
        }
    }

    private void ClearSelection()
    {
        if(selectedObject.Count > 0)
        {
            foreach(GameObject obj in selectedObject)
            {
                if(obj != null)
                {
                    obj.GetComponent<Selection>().BeingSelected = false;
                }
            }
            selectedObject.Clear();
        }
    }

    private void ConfirmSelection()
    {
        if (selectingObject.Count > 0)
        {
            foreach (GameObject go in selectingObject)
            {
                if (!selectedObject.Contains(go))
                {
                    if (go != null)
                    {
                        if (go.GetComponent<Selection>())
                        {
                            if (!go.GetComponent<Selection>().BeingSelected)
                            {
                                go.GetComponent<Selection>().BeingSelected = true;
                            }
                        }
                    }
                    selectedObject.Add(go);
                }
            }
            selectingObject.Clear();
        }

    }

    private void OnDrawGizmos()
    {
        if (selectingObject.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < selectingObject.Count; i++)
            {
                Gizmos.DrawWireSphere(selectingObject[i].transform.position, 0.5f);
                Gizmos.DrawLine(selectingObject[i].transform.position, Camera.main.transform.position - (selectingObject[i].transform.position - Camera.main.transform.position));
            }
        }
    }
}
