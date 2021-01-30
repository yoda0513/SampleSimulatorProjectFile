using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Cell : MonoBehaviour
{
    public Text stringObject;
    public Image back;

    [SerializeField]private int _fontSize = 17;
    [SerializeField]private string _cellString = "";
    [SerializeField]private Color _backColor = new Color(1,1,1,1);
    [SerializeField]private Color _stringColor = new Color(0,0,0,1);

    public int FontSize
    {
        set
        {
            stringObject.fontSize = value;
            this._fontSize = value;
        }

        get
        {
            return this._fontSize;
        }
    }

    public string CellContent
    {
        set
        {
            stringObject.text = value;
            this._cellString = value;
        }

        get
        {
            return this._cellString;
        }
    }

    public Color BackColor
    {
        set
        {
            back.color = value;
            this._backColor = value;
        }

        get
        {
            return this._backColor;
        }
    }

    public Color StringColor
    {
        set
        {
            stringObject.color = value;
            this._stringColor = value;
        }

        get
        {
            return this._stringColor;
        }
    }


    void updateCell(int a, string b, Color c, Color d)
    {
        stringObject.fontSize = a;
        stringObject.text = b;
        back.color = c;
        stringObject.color = d;
    }

    private void OnValidate()
    {
        updateCell(_fontSize, _cellString, _backColor, _stringColor);
    }
}
