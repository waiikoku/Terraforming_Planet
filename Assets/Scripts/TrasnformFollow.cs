using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrasnformFollow : MonoBehaviour
{
    public Transform target;
    public Transform childTarget;

    public Transform myChild;
    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x,0, target.position.z);
        transform.position = targetPos;
        transform.rotation = target.rotation;
        myChild.localPosition = new Vector3(0, childTarget.position.y + 10, 0);
    }
}
