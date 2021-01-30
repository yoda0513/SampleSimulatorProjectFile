using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 * Histogramオブジェクト...このコンポーネントを取り付けたオブジェクトでCanvasコンポーネントを持っている。
 * 
 * 度数分布とラインを生成し、カメラで撮影したものをリアルタイムで800×500のRendererTextureに焼き付ける。
 * そのためCanvasの要素の座標範囲は中心０、右上が(400,250)になるのに対して、 LineRendererのようなグローバル座標を用いるものは、中心(0,0)
 * で、右上が(8,5)となる。
 * 
 * 
 * 関連クラス
 * HistogramFunction..まとめているだけ
 * Distribution...度数分布表オブジェクト
 * 
 */




[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class Histogram : MonoBehaviour
{
    public GameObject HistgramBar;
    public GameObject number;

    

    private void Start()
    {
        
    }

    public void resetHist()　//Histogramのリセット。
    {
        foreach (Transform n in this.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
    }

    public void createHistgram(Distibution distribution)  //distributionオブジェクトを渡すことで、ヒストグラムを描画する。
    {
        float barWidth = (float)730 / (float)distribution.classCount;
        float barHeightPerValue = (float)420 / (float)distribution.maxFrequency; //一度あたりの描画する高さ

        
        //下の度数を表示
        for(int i = 0; i < distribution.classCount ; i++)
        {
            float x = -219;
            if (i % 2 != 0) x = -237; 

            rectInstantiate(number, 
                new Vector2(1, 1), 
                new Vector3(-300 + barWidth * (i), x, 1), 
                distribution.classTable[i].ToString());
        }


        
        //ヒストグラムのバーを生成。
        for (int i =0; i<distribution.classCount -1; i++)
        {
            Vector2 size = new Vector2(barWidth, barHeightPerValue * distribution.frequency[i]);
            Vector3 position =  new Vector3((-300 + (barWidth / 2) + barWidth * i), -200, 0);
            GameObject x = rectInstantiate(HistgramBar, size, position);

            x.GetComponent<HistogramBar>().setText(distribution.frequency[i].ToString());
        }

        
    }




    private GameObject rectInstantiate(GameObject x, Vector2 deltaSize, Vector3 localPosition , string text = "")
    {
        GameObject instance = Instantiate(x);
        SceneManager.MoveGameObjectToScene(instance, SceneManager.GetSceneByName("Histogram"));
        instance.transform.SetParent(this.transform);
        RectTransform barTransform = instance.GetComponent<RectTransform>();


        barTransform.sizeDelta = deltaSize;
        barTransform.localPosition = localPosition;
        barTransform.localScale = new Vector3(1, 1, 1);

        if(instance.GetComponent<Text>() != null)
        {
            instance.GetComponent<Text>().text = text;
        }

        return instance;
    }

    


    

    
}











    

