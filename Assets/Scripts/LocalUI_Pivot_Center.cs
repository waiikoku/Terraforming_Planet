using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalUI_Pivot_Center : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private Vector2 RTSize;
    [SerializeField] private float posX;
    [SerializeField] private float posY;
    [SerializeField] private Vector2 RTPos;
    [SerializeField] private Vector2 RealSizePosition;

    private void Start()
    {
        if (GetComponent<RectTransform>())
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            if(rectTransform != null)
            {
                width = rectTransform.sizeDelta.x;
                height = rectTransform.sizeDelta.y;
                RTSize = new Vector2(width,height);
                posX = rectTransform.anchoredPosition.x;
                posY = rectTransform.anchoredPosition.y;
                RTPos = new Vector2(posX,posY);

                RealSizePosition = RTSize - RTPos;
            }
        }
    }

    private void Update()
    {
        if (rectTransform != null)
        {
            width = rectTransform.sizeDelta.x;
            height = rectTransform.sizeDelta.y;
            RTSize = new Vector2(width, height);
            posX = rectTransform.anchoredPosition.x;
            posY = rectTransform.anchoredPosition.y;
            RTPos = new Vector2(posX, posY);
            RealSizePosition = RTSize - RTPos;

            rectTransform.anchoredPosition = new Vector2(Mathf.Clamp(rectTransform.anchoredPosition.x,-(Screen.width / 2) + ( width / 2), (Screen.width / 2) - (width / 2)), Mathf.Clamp(rectTransform.anchoredPosition.y, -(Screen.height / 2) + ( height / 2), (Screen.height / 2) - (height / 2)));
        }
    }
}
