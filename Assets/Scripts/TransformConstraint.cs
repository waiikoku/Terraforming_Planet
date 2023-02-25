using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformConstraint : MonoBehaviour
{
    public bool LocalObject = false;
    public Vector3 parentPos;
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;

    private void Start()
    {
        if (LocalObject)
        {
            parentPos = transform.parent.transform.position;
            if (transform.position != parentPos + pos)
            {
                transform.position = parentPos + pos;
            }
            if (transform.rotation != Quaternion.Euler(rot))
            {
                transform.rotation = Quaternion.Euler(rot);
            }
            if (transform.localScale != scale)
            {
                transform.localScale = scale;
            }
        }
    }

    private void Update()
    {
        if (LocalObject)
        {
            if (transform.position != parentPos + pos)
            {
                transform.position = parentPos + pos;
            }
            if (transform.rotation != Quaternion.Euler(rot))
            {
                transform.rotation = Quaternion.Euler(rot);
            }
            if (transform.localScale != scale)
            {
                transform.localScale = scale;
            }
        }
    }
}
