using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistogramBar : MonoBehaviour
{
    public Text UIText;

    public void setText(string x)
    {
        UIText.text = x;
    }
}
