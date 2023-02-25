using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool isLookingAt = false;
    private void Update()
    {
        if (isLookingAt)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
