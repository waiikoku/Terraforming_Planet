using UnityEngine;
using UnityEngine.UI;

public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] private Image MainImage;
    [SerializeField] private bool Turn = false;
    [SerializeField] private Color[] colour;

    public void SetBool(bool value)
    {
        if (colour.Length >= 2)
        {
            Turn = value;
            if (Turn)
            {
                if (MainImage.color != colour[0])
                {
                    MainImage.color = colour[0];
                }
            }
            else
            {
                if (MainImage.color != colour[1])
                {
                    MainImage.color = colour[1];
                }
            }
        }
    }

    public bool VerifyBool()
    {
        return Turn;
    }

    public void Switch()
    {
        Turn = !Turn;
        if (colour.Length >= 2)
        {
            if (Turn)
            {
                if (MainImage.color != colour[0])
                {
                    MainImage.color = colour[0];
                }
            }
            else
            {
                if (MainImage.color != colour[1])
                {
                    MainImage.color = colour[1];
                }
            }
        }
    }
}
