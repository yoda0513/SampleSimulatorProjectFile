using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//インデックスは全て数学のような入れ方をする。
//0からではなく1から。
public class Table : MonoBehaviour
{
    //Prefab-----------------------------------------------------------------
    public GameObject colPrefab;
    public GameObject cellPrefab;


    

   //削除系～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～

    //中身を完全に消す
   public void resetTable()
    {
        foreach (Transform n in this.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }


    //列の削除。1~
    public void clearCol(int colNumber)
    {
        Destroy(this.transform.GetChild(colNumber - 1).gameObject);
    }

    //行の削除
    public void clearRow(int rowNumber)
    {
        int colNumber = this.transform.childCount;
        for (int i = 0; i < colNumber; i++)
        {
            Destroy(this.transform.GetChild(i).GetChild(rowNumber - 1).gameObject);
        }
    }



    //セル追加系統～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    //列にセルを追加する。
    public void addCell(int colIndex, string content = "")
    {
        Transform col = this.transform.GetChild(colIndex - 1);
        var x = Instantiate(cellPrefab);
        x.transform.SetParent(col);
        x.GetComponent<Cell>().CellContent = content;
    }

    //列を追加
    public void addCol()
    {
        GameObject col = Instantiate(colPrefab);
        col.transform.SetParent(this.transform);

        var i = this.transform.GetChild(0).childCount;
        for (int x = 0; x < i; x++)
        {
            addCell(this.transform.childCount);
        }
    }

    //行にセルを追加
    public void addRow()
    {
        var x = this.transform.childCount;
        for(int i=0; i< x; i++)
        {
            addCell(i+1);
        }
    } 




    //検索～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    //1行１列～　で指定
    public Cell getCell(int gyou, int retu)
    {
        Transform col = this.transform.GetChild(retu - 1);
        Cell cell = col.GetChild(gyou -1).gameObject.GetComponent<Cell>();
        return cell;
    }






    //その他～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～～
    private void OnValidate()
    {
        updateCol();
    }

    private void updateCol()
    {

        foreach (Transform child in this.gameObject.transform)
        {
            Col col = child.gameObject.GetComponent<Col>();
            if (col == null)
            {
                Destroy(child.gameObject);
            }
        }
    }


    


}
