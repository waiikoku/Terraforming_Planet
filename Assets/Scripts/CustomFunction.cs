using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFunction : MonoBehaviour
{
    public static string ValueToShortText(float value,string addition)
    {
        string ValueText = value.ToString("F0");
        int ValueLength = value.ToString("F0").Length;
        if(value < 1000) //Less than thousand example : "999"
        {
            return ValueText + addition;
        }
        else if (value < 1000000) //7 //Less than million. Example : "999k"
        {
            string result = ValueText.Substring(0, ValueLength - 3) + "k" + addition;
            return result;
        }
        else if (value < 1000000000) //10 //Less than billion.  Example : "999M"
        {
            string result = ValueText.Substring(0, ValueLength - 6) + "M" + addition;
            return result;
        }
        return "Null";
    }
}
