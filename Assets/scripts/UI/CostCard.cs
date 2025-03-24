using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostCard : MonoBehaviour
{
    public Image image;
    public TMP_Text countText;

    public void SetColor(Color color)
    {
        image.color = color;
    }
    
    public void SetCountText(int x)
    {
        countText.text = x.ToString();
    }
}
