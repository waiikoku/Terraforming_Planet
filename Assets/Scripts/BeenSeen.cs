using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeenSeen : MonoBehaviour
{
    public bool OnScreen = false;
    public Vector3 screenPoint;
    public bool DisplayLocalCanvas = false;
    public TextMeshProUGUI DisplayToRect;
    private void Update()
    {
        screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        OnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //https://answers.unity.com/questions/720447/if-game-object-is-in-cameras-field-of-view.html
        if (OnScreen)
        {
            if (DisplayLocalCanvas)
            {
                DisplayToRect.text = Camera.main.WorldToScreenPoint(transform.position).ToString();
            }
        }
        else
        {
            if (DisplayLocalCanvas)
            {
                if (DisplayToRect.text != "")
                {
                    DisplayToRect.text = "";
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (OnScreen)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }
}
