using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCorner : MonoBehaviour
{
    public float width;
    public float height;
    public float length;
    public Color[] Colour;
    public bool generatedColour;
    public int max;
    public int current;
    public float sphereSize = 1f;
    public float currentTime;
    public float cooldownTime = 1f;
    public Vector3 colliderSize;
    public Vector3[] csp;
    public Vector3[] cornerPos;
    public Vector3[] AroundPoint;
    public GameObject target;
    public bool Cleared = false;

    private void OnDrawGizmos()
    {
        if (Cleared)
        {
            Cleared = false;
            csp = new Vector3[0];
            cornerPos = new Vector3[0];
            AroundPoint = new Vector3[0];
            colliderSize = Vector3.zero;
            Colour = new Color[0];
        }
        max = (int)(width * height * length);
        current = 0;
        if(currentTime <= cooldownTime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0;
            generatedColour = false;
        }
        if (!generatedColour)
        {
            generatedColour = true;
            Colour = new Color[max];
            for (int i = 0; i < width * height * length; i++)
            {
                Colour[i] = new Color(Random.Range(0.00f,1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f));
            }
        }
        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                for (int l = 0; l < length; l++)
                {
                    Gizmos.color = Colour[current];
                    if (current < max)
                    {
                        current++;
                    }
                    Gizmos.DrawLine(new Vector3(w, h, l) + transform.position, new Vector3(w, h + 1, l) + transform.position);
                    Gizmos.DrawWireSphere(new Vector3(w, h, l) + transform.position,sphereSize);
                    Gizmos.DrawWireSphere(new Vector3(w, h + 1, l) + transform.position, sphereSize + 0.1f);
                    if (current >= max)
                    {
                        current = 0;
                    }
                }
            }
        }


        if (target != null)
        {
            if (csp.Length != 8)
            {
                csp = new Vector3[8];
            }
            if(cornerPos.Length != csp.Length)
            {
                cornerPos = new Vector3[csp.Length];
            }
            if(AroundPoint.Length != csp.Length)
            {
                AroundPoint = new Vector3[csp.Length];
            }
            if (target.GetComponent<Collider>())
            {
                colliderSize = target.GetComponent<Collider>().bounds.size;

                csp[0] = new Vector3(colliderSize.x, 0, colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[0] / 2, sphereSize);
                cornerPos[0] = target.transform.position + csp[0] / 2;

                csp[1] = new Vector3(-colliderSize.x, 0, colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[1] / 2, sphereSize);
                cornerPos[1] = target.transform.position + csp[1] / 2;

                csp[2] = new Vector3(colliderSize.x, 0, -colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[2] / 2, sphereSize);
                cornerPos[2] = target.transform.position + csp[2] / 2;

                csp[3] = new Vector3(-colliderSize.x, 0, -colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[3] / 2, sphereSize);
                cornerPos[3] = target.transform.position + csp[3] / 2;

                csp[4] = new Vector3(1 * colliderSize.x, 0, 0);
                Gizmos.DrawWireSphere(target.transform.position + csp[4] / 2, sphereSize);
                cornerPos[4] = target.transform.position + csp[4] / 2;

                csp[5] = new Vector3(-1 * colliderSize.x, 0, 0);
                Gizmos.DrawWireSphere(target.transform.position + csp[5] / 2, sphereSize);
                cornerPos[5] = target.transform.position + csp[5] / 2;

                csp[6] = new Vector3(0, 0, 1 * colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[6] / 2, sphereSize);
                cornerPos[6] = target.transform.position + csp[6] / 2;

                csp[7] = new Vector3(0, 0, -1 * colliderSize.z);
                Gizmos.DrawWireSphere(target.transform.position + csp[7] / 2, sphereSize);
                cornerPos[7] = target.transform.position + csp[7] / 2;

                for (int i = 0; i < AroundPoint.Length; i++)
                {
                    AroundPoint[i] = target.transform.position + (csp[i] / 2);
                }

                /*
                csp[0] = new Vector3(colliderSize.x, 0, colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[0] / 2, sphereSize);
                csp[1] = new Vector3(-colliderSize.x, 0, colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[1] / 2, sphereSize);
                csp[2] = new Vector3(colliderSize.x, 0, -colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[2] / 2, sphereSize);
                csp[3] = new Vector3(-colliderSize.x, 0, -colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[3] / 2, sphereSize);

                csp[4] = new Vector3(1 * colliderSize.x, 0, 0);
                Gizmos.DrawWireSphere(cornerPos[4] / 2, sphereSize);
                csp[5] = new Vector3(-1 * colliderSize.x, 0, 0);
                Gizmos.DrawWireSphere(cornerPos[5] / 2, sphereSize);
                csp[6] = new Vector3(0, 0, 1 * colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[6] / 2, sphereSize);
                csp[7] = new Vector3(0, 0, -1 * colliderSize.z);
                Gizmos.DrawWireSphere(cornerPos[7] / 2, sphereSize);
                */
            }
        }
    }
}
