using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LocalPopUp : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private bool PointerOnThis = false;
    public GameObject LocalPopUpElement;

    private void Start()
    {
        if (LocalPopUpElement != null)
        {
            if (LocalPopUpElement.activeInHierarchy)
            {
                LocalPopUpElement.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (LocalPopUpElement != null)
        {
            if (!LocalPopUpElement.activeInHierarchy)
            {
                LocalPopUpElement.SetActive(true);
            }
        }
        if (!PointerOnThis)
        {
            PointerOnThis = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (LocalPopUpElement != null)
        {
            if (LocalPopUpElement.activeInHierarchy)
            {
                LocalPopUpElement.SetActive(false);
            }
        }
        if (PointerOnThis)
        {
            PointerOnThis = false;
        }
    }
}
