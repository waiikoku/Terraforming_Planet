using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStatus : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider ts_collider;
    [SerializeField] private string ts_CurrentTag;

    [SerializeField] private List<GameObject> ObjectTriggred;

    public List<GameObject> TriggeredObject;
    public bool Triggered = false;
    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (ts_collider == null)
        {
            ts_collider = GetComponent<Collider>();
        }
        if (ts_CurrentTag == null)
        {
            ts_CurrentTag = "Untagged";
        }
    }

    private bool CheckTag(GameObject go)
    {
        if (!go.CompareTag(ts_CurrentTag))
        {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTag(other.gameObject))
        {
            if (!ObjectTriggred.Contains(other.gameObject))
            {
                ObjectTriggred.Add(other.gameObject);
            }
            Triggered = true;
            TriggeredObject = ObjectTriggred;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTag(other.gameObject))
        {
            if (ObjectTriggred.Contains(other.gameObject))
            {
                ObjectTriggred.Remove(other.gameObject);
            }
            Triggered = false;
            TriggeredObject = ObjectTriggred;
        }
    }
}
