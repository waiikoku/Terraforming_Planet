using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCount : MonoBehaviour
{
    public Text txt;
    public float currentTime;
    public int minute;
    private string newMinute;
    private string newSecond;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(minute < 10)
        {
            newMinute = "0" + minute;
        }
        else
        {
            newMinute = minute.ToString();
        }
        if(currentTime < 10)
        {
            newSecond = "0" + currentTime.ToString("F0");
        }
        else
        {
            newSecond = currentTime.ToString("F0");
        }
        txt.text = newMinute + ":" + newSecond;
        if(currentTime > 60)
        {
            minute += 1;
            currentTime = 0;
        }
    }
}
