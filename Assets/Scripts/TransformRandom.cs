using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRandom : MonoBehaviour
{
    public bool Activate = true;
    public bool RNG_Pos = false;
    public bool RNG_Rot = false;
    public bool RNG_Scale = false;
    public bool NormalSurface = false;
    public bool SelfDestruction = true;
    private void Start()
    {
        if (RNG_Pos)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,out hit, Mathf.Infinity))
            {
                if (transform.position != hit.point)
                {
                    transform.position = hit.point;
                }
            }
        }
        if (RNG_Rot)
        {
            transform.rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        }
        if (RNG_Scale)
        {
            transform.localScale = new Vector3(Random.Range(1.0f, 2.0f), Random.Range(1.0f, 2.0f), Random.Range(1.0f, 2.0f));
        }
        if (NormalSurface)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down,out hit, Mathf.Infinity))
            {
                if (transform.position != hit.point)
                {
                    transform.position = hit.point;
                }
                transform.rotation = Quaternion.LookRotation(hit.normal,Vector3.up);
            }
        }
        if (SelfDestruction)
        {
            Destroy(this);
        }
    }
}
