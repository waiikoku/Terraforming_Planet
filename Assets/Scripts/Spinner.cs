using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Spin Axis")]
    [SerializeField] private bool AxisX;
    [SerializeField] private bool AxisY;
    [SerializeField] private bool AxisZ;

    [Header("Property")]
    public float minimumSpeed = 0f;
    public float currentSpeed = 0f;
    public float maximumSpeed = 10f;
    public bool isFullSpeed = false;
    public bool StartSpin = false;
    public bool StopSpin = false;
    public bool Spinning = false;

    private float timer = 0;
    private void FixedUpdate()
    {
        if (StartSpin)
        {
            if (StopSpin)
            {
                StopSpin = false;
            }
            if (!Spinning)
            {
                Spinning = true;
            }
            if (currentSpeed < maximumSpeed)
            {
                currentSpeed += (0.1f * Time.deltaTime) * timer;
            }
            else
            {
                currentSpeed = maximumSpeed;
                isFullSpeed = true;
                StartSpin = false;
            }
        }
        if (Spinning)
        {
            timer += Time.deltaTime;
            if (AxisX)
            {
                transform.Rotate(Vector3.right * currentSpeed);
            }
            if (AxisY)
            {
                transform.Rotate(Vector3.up * currentSpeed);
            }
            if (AxisZ)
            {
                transform.Rotate(Vector3.forward * currentSpeed);
            }

        }
        else
        {
            if(timer != 0)
            {
                timer = 0;
            }
        }
        if (StopSpin)
        {
            if (StartSpin)
            {
                StartSpin = false;
            }
            if (isFullSpeed)
            {
                isFullSpeed = false;
            }
            if(currentSpeed > minimumSpeed)
            {
                currentSpeed -= (0.1f * Time.deltaTime) * (timer * (maximumSpeed * 0.1f));
            }
            else
            {
                currentSpeed = minimumSpeed;
                Spinning = false;
                StopSpin = false;
            }
        }

    }

    public void ForceStart()
    {
        if (StopSpin)
        {
            StopSpin = false;
        }
        if (!StartSpin)
        {
            StartSpin = true;
        }
    }

    public void ForceStop()
    {
        if (StartSpin)
        {
            StartSpin = false;
        }
        if (!StopSpin)
        {
            StopSpin = true;
        }
    }
}
