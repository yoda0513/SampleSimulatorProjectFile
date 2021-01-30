using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col : MonoBehaviour
{
    [SerializeField]List<Cell> cells = new List<Cell>();

    private void updateCol()
    {
        cells.Clear();

       foreach(Transform child in this.gameObject.transform)
        {
            Cell cell = child.gameObject.GetComponent<Cell>();
            if (cell == null)
            {
                Destroy(child.gameObject);
            }
            
        }
    }



    private void OnValidate()
    {
        updateCol();
    }
}
