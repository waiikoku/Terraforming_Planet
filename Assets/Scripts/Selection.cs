using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public bool BeingSelected = false;

    private void Update()
    {
        if (BeingSelected)
        {
            if (playerSelector.instance.selectableObjects.Contains(this.gameObject))
            {
                playerSelector.instance.selectableObjects.Remove(this.gameObject);
            }
            if (Input.GetKeyDown(KeyCode.Escape) && ConstructionController.instance.CurrentBuilded)
            {
                BeingSelected = false;
            }
        }
        else
        {
            if (!playerSelector.instance.selectableObjects.Contains(this.gameObject))
            {
                playerSelector.instance.selectableObjects.Add(this.gameObject);
            }
        }
    }
}
