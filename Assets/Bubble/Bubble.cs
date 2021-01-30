using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public Text bubbleText;

    [TextArea(1, 500)]
    public string text;

    public GameObject imageObject;
    public bool isView = false;

    private void Start()
    {
        isView = false;
    }

    private void OnValidate()
    {
        bubbleText.text = this.text;
        imageObject.SetActive(isView);
        
    }

    public bool IsView
    {
        set
        {
            this.isView = value;
            imageObject.SetActive(isView);
        }
        get
        {
            return this.isView;
        }
    }
}
