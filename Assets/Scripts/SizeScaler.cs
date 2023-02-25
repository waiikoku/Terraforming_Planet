using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour
{
    public float index;
    public float frequency = 1f;
    public float amplitude = 2f;
    public float value;
    public Vector3 defaulScale;
    public Vector3 scale;
    public bool multipie;

    private void Update()
    {
        index = Time.time;
        value = Mathf.Sin(index * frequency) * amplitude;
        if (multipie)
        {
            scale = defaulScale * value;
        }
        else
        {
            scale = defaulScale + Vector3.one * value;
        }
        transform.localScale = scale;
    }
}
