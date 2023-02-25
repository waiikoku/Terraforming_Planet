using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSelector : MonoBehaviour
{
    public Selection selection;
    public GameObject selector;

    private void Start()
    {
        if (selector.activeInHierarchy)
        {
            selector.SetActive(false);
        }
    }
    private void Update()
    {
        if (selection.BeingSelected)
        {
            if (!selector.activeInHierarchy)
            {
                selector.SetActive(true);
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
}
