using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugToggleActivate : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private bool activate = false;

    private void Start()
    {
        activate = target.activeInHierarchy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activate = !activate;
            target.SetActive(activate);
        }
    }
}
