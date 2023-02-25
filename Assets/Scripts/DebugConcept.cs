using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConcept : MonoBehaviour
{
    public Transform startPos;
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public bool isGizmos = false;

    private void OnDrawGizmos()
    {
        if (isGizmos)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(startPos.position, 0.5f);
            Gizmos.DrawLine(transform.position, startPos.position);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Pos1.position, 0.5f);
            Gizmos.DrawLine(startPos.position, Pos1.position);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Pos2.position, 0.5f);
            Gizmos.DrawLine(Pos1.position, Pos2.position);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(Pos3.position, 0.5f);
            Gizmos.DrawLine(Pos2.position, Pos3.position);
        }
    }
}
