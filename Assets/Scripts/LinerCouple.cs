using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinerCouple : MonoBehaviour
{
    public LineRenderer line;
    public Transform pointA;
    public Transform pointB;
    public Vector3 offset = new Vector3(0, 0.1f, 0);

    private void Start()
    {
        if(line != null)
        {
            if(line.positionCount != 2)
            {
                line.positionCount = 2;
            }
            line.SetPosition(0, pointA.localPosition + offset);
            line.SetPosition(1, pointB.localPosition + offset);
        }
    }

    private void Update()
    {
        if (line != null)
        {
            line.SetPosition(0, pointA.localPosition + offset);
            line.SetPosition(1, pointB.localPosition + offset);
        }
    }
}
