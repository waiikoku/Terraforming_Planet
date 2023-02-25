using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class mouseEvent : MonoBehaviour
    , IPointerClickHandler
    , IDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public static mouseEvent instance;

    public bool OnUI = false;

    public GameObject UiHoverElement;

    public GameObject Current;
    public GameObject Clicker;

    public Vector2 realScreenSize;
    [SerializeField] private Vector2 screenSize;
    [SerializeField] private Vector3 dragMousePos;
    [SerializeField] private Vector3 hoverMousePos;
    [SerializeField] private Vector3 objOffset;
    private bool GetFirstOffset = false;
    [SerializeField] private Vector3 OffsetBetweenMousePos;
    [SerializeField] private Vector3 hoverSize;
    [SerializeField] private Vector3 hoverOffset;

    [SerializeField] private bool isDrag = false;
    [SerializeField] private bool isHover = false;
    [SerializeField] private bool Hovered = false;
    [SerializeField] private float countHoverTime = 0.2f;
    [SerializeField] private float currentHoverTime = 0f;

    [Header("RTS Selection Box GUI")]
    public RectTransform selectSquareImage;
    public Vector3 startPos;
    private Vector3 startMousePos;
    public Vector3 endPos;

    private Vector2 centreDebug;
    public static Rect SelectionBoxRect;
    private void Start()
    {
        instance = this;
        selectSquareImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isHover)
        {
            if (currentHoverTime < countHoverTime)
            {
                if (Hovered)
                {
                    Hovered = false;
                }
                currentHoverTime += 1 * Time.deltaTime;
            }
            else
            {
                if (currentHoverTime != countHoverTime)
                {
                    currentHoverTime = countHoverTime;
                }
                if (!Hovered)
                {
                    Hovered = true;
                }
            }
        }
        else
        {
            if (Hovered)
            {
                Hovered = false;
            }
            if (currentHoverTime != 0)
            {
                currentHoverTime = 0;
            }
        }
        if (Hovered)
        {
            if (Current != null)
            {
                if (Current.GetComponent<LocalUIController>())
                {
                    if (Current.GetComponent<LocalUIController>().Interactable)
                    {
                        if (Current.GetComponent<LocalUIController>().HoverShowDetail)
                        {
                            if (UiHoverElement != null)
                            {
                                if (!UiHoverElement.activeInHierarchy)
                                {
                                    UiHoverElement.SetActive(true);
                                }
                                if (UiHoverElement.transform.GetSiblingIndex() != UiHoverElement.transform.parent.childCount - 1)
                                {
                                    UiHoverElement.transform.SetAsLastSibling();
                                }
                                if (hoverSize != new Vector3(UiHoverElement.GetComponent<RectTransform>().sizeDelta.x / 2, UiHoverElement.GetComponent<RectTransform>().sizeDelta.y / 2, 0))
                                {
                                    hoverSize = UiHoverElement.GetComponent<RectTransform>().sizeDelta / 2;
                                }
                                hoverMousePos = new Vector3(Input.mousePosition.x + hoverSize.x + hoverOffset.x, Input.mousePosition.y - hoverSize.y - hoverOffset.y, 0);
                                UiHoverElement.transform.position = hoverMousePos;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (UiHoverElement != null)
            {
                if (UiHoverElement.activeInHierarchy)
                {
                    UiHoverElement.SetActive(false);
                }
                UiHoverElement.transform.position = Vector3.zero;
            }
        }
        if (realScreenSize != new Vector2(Screen.width, Screen.height))
        {
            realScreenSize = new Vector2(Screen.width, Screen.height);
        }
        if (GetComponent<CanvasScaler>())
        {
            if (GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
            {
                if (screenSize != new Vector2(Screen.width, Screen.height))
                {
                    screenSize = new Vector2(Screen.width, Screen.height);
                }
            }
            if (GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                if (GameObject.FindObjectOfType<GraphicSettings>())
                {
                    if (!GameObject.FindObjectOfType<GraphicSettings>().CheckResolution(realScreenSize))
                    {
                        if (GetComponent<CanvasScaler>().referenceResolution != new Vector2(Screen.width, Screen.height))
                        {
                            GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
                        }
                    }
                    else
                    {
                        if (GetComponent<CanvasScaler>().referenceResolution != new Vector2(Screen.width, Screen.height))
                        {
                            GetComponent<CanvasScaler>().referenceResolution = new Vector2(Screen.width, Screen.height);
                        }
                    }
                }
                if (screenSize != new Vector2(GetComponent<CanvasScaler>().referenceResolution.x, GetComponent<CanvasScaler>().referenceResolution.y))
                {
                    screenSize = new Vector2(GetComponent<CanvasScaler>().referenceResolution.x, GetComponent<CanvasScaler>().referenceResolution.y);
                }
            }
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };
            pointerData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, result);
            //https://answers.unity.com/questions/1009987/detect-canvas-object-under-mouse-because-only-some.html?_ga=2.237382403.2020186347.1580117221-1133669525.1572521665
            if (result.Count >= 1)
            {
                if (Current == result[0].gameObject)
                {
                    if (!isDrag)
                    {
                        if (!isHover)
                        {
                            isHover = true;
                        }
                    }
                    else
                    {
                        if (isHover)
                        {
                            isHover = false;
                        }
                    }
                }
                else
                {
                    isHover = false;
                    Current = result[0].gameObject;
                }
            }
        }
        if (!isDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (OnUI)
                {
                    startPos = Vector3.zero;
                    endPos = Vector3.zero;
                    return;
                }
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    startPos = hit.point;
                }
                else
                {
                    startPos = Vector3.zero;
                    endPos = Vector3.zero;
                    return;
                }
                startMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                if (startPos == Vector3.zero)
                {
                    if (endPos != Vector3.zero)
                    {
                        endPos = Vector3.zero;
                    }
                    return;
                }
                if (Input.mousePosition != startMousePos)
                {
                    //https://www.youtube.com/watch?v=vsdIhyLKgjc 01 Feb 2020 , 10:35AM
                    if (!selectSquareImage.gameObject.activeInHierarchy)
                    {
                        selectSquareImage.gameObject.SetActive(true);
                    }
                    endPos = Input.mousePosition;
                    endPos = new Vector3(Mathf.Clamp(endPos.x, 0, screenSize.x), Mathf.Clamp(endPos.y, 0, screenSize.y), 0);

                    Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
                    squareStart.z = 0f;

                    Vector3 centre = (squareStart + endPos) / 2f;
                    centreDebug = new Vector2(centre.x, centre.y);
                    selectSquareImage.position = centre;

                    float sizeX = Mathf.Abs(squareStart.x - endPos.x);
                    float sizeY = Mathf.Abs(squareStart.y - endPos.y);
                    selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);

                    SelectionBoxRect = new Rect(selectSquareImage.anchoredPosition.x, selectSquareImage.anchoredPosition.y,sizeX,sizeY);

                }
            }
        }
        else
        {
            if (selectSquareImage.gameObject.activeInHierarchy)
            {
                selectSquareImage.gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (startMousePos != Vector3.zero)
            {
                startMousePos = Vector3.zero;
            }
            if (selectSquareImage.gameObject.activeInHierarchy)
            {
                selectSquareImage.gameObject.SetActive(false);
            }
            if (Clicker != null)
            {
                Clicker = null;
            }
            if (isDrag)
            {
                isDrag = false;
                if (GetFirstOffset)
                {
                    GetFirstOffset = false;
                }
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        Debug.Log(eventData.button + " Click");
        /*
        if(eventData.button == PointerEventData.InputButton.Left)
        { 
            LeftClicking = true;
        }
        */
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        Debug.Log(eventData.button + " Drag");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!isDrag)
            {
                isDrag = true;
            }
            if (Clicker != null)
            {
                if (Clicker.GetComponent<LocalUIController>())
                {
                    if (Clicker.GetComponent<LocalUIController>().Interactable)
                    {
                        if (Clicker.GetComponent<LocalUIController>().Dragable)
                        {
                            if (objOffset != new Vector3(Clicker.GetComponent<RectTransform>().sizeDelta.x / 2, Clicker.GetComponent<RectTransform>().sizeDelta.y / 2, 0))
                            {
                                objOffset = Clicker.GetComponent<RectTransform>().sizeDelta / 2;
                            }
                            if (!GetFirstOffset)
                            {
                                GetFirstOffset = true;
                                OffsetBetweenMousePos = Clicker.transform.position - Input.mousePosition;
                            }
                            dragMousePos = new Vector3(Mathf.Clamp(Input.mousePosition.x,objOffset.x - OffsetBetweenMousePos.x, screenSize.x - objOffset.x - OffsetBetweenMousePos.x), Mathf.Clamp(Input.mousePosition.y, objOffset.y - OffsetBetweenMousePos.y, screenSize.y - objOffset.y - OffsetBetweenMousePos.y),0);
                            if(Clicker.transform.GetSiblingIndex() != Clicker.transform.parent.childCount - 1)
                            {
                                Clicker.transform.SetAsLastSibling();
                            }
                            Clicker.transform.position = OffsetBetweenMousePos + dragMousePos;
                            
                        }
                    }
                }
            }
            else
            {
                Clicker = eventData.pointerCurrentRaycast.gameObject;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!OnUI)
        {
            OnUI = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnUI)
        {
            OnUI = false;
        }
        if(Current != null)
        {
            Current = null;
        }
    }

    /*
    private void OnGUI()
    {
        GUI.Box(new Rect(screenSize.x / 2, screenSize.y / 2, 196, 32), "Current Screen Size :"+screenSize);
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2 - 32, 196, 32), "CRealS Size :" + new Vector2(Screen.width,Screen.height));
    */
}
