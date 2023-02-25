using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private bool drafted;
    private void Update()
    {
        if (!drafted)
        {
            drafted = true;
            if (!ResourceController.instance.storage.Contains(this))
            {
                ResourceController.instance.storage.Add(this);
            }
        }
    }
}
