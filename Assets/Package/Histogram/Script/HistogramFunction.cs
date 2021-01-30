using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


//関数をまとめるだけ
public static class HistogramFunction
{
    //カメラの座標（スクリーン座標）をグローバル座標に変換する
     static Vector3 convertCoordinate(Vector3 cameraPosition)
    {
        Vector3 returnVector = new Vector3(cameraPosition.x * (8 / 400), cameraPosition.y * (5 / 250), cameraPosition.z);

        return returnVector;
    }

   


}


public class Distibution
{
    public List<float> classTable = new List<float>();  //階級を左はしから右はしまで羅列したもの。階級の数は「classtableの要素-1」となる。
    public List<int> frequency = new List<int>();   //各階級の頻度

    public int sampleCount = 0;
    public int classCount = 0;
    public float classWidth = 0;
    public int maxFrequency = 0;

    //コンストラクター
    public Distibution(List<float> data)
    {
        int minValue = (int)data.Min();
        int maxValue = (int)data.Max();
        Debug.Log($"最小値:::{minValue},::::{maxValue}");

        int minPower = 1;
        int maxPower = 1;

        if (Digit(minValue) > 2)
        {
            minPower = (int)Math.Pow(10, (float)(Digit(minValue) - 2));
        }

        if (Digit(maxValue) > 2)
        {
            maxPower = (int)Math.Pow(10, (float)(Digit(maxValue) - 2));
        }


        Debug.Log($"Power:::{minPower}::::{maxPower}");
        Debug.Log((float)(maxValue / maxPower));

        int min = (int)(Math.Floor((float)(minValue/     minPower      ))*   minPower         );
        int max = (int)(Math.Ceiling( (float)(data.Max() /  maxPower )    )       *      maxPower    );

        Debug.Log($"min:::{min}, max::::{max}");

        sampleCount = data.Count;
        classCount = (int)Math.Ceiling(1 + Math.Log(sampleCount, 2)); //スタージェスの公式
        if (classCount < 3) classCount = 4;

        float x = (float)(max - min);
        float x2 = x / (classCount - 1);
        classWidth = (float)Math.Ceiling(x2);


      
        Debug.Log($"Width:::{classWidth}");

        classTable.Add(min); //一番最初の行をセット

        for (int i = 1; i < classCount; i++)  //階級リストを生成
        {
            classTable.Add(classTable[i - 1] + classWidth);  //前の階級に階級幅を足していく      
        }


        for (int i = 0; i < classTable.Count - 1; i++)  //度数リストを生成
        {
            int y = 0;

            foreach (var k in data)
            {
                if ((k >= classTable[i]) && (k < classTable[i + 1]))
                {
                    y++;
                }


            }

            frequency.Add(y);
        }

        maxFrequency = frequency.Max();
    }

    public int Digit(int num)
    {
        if (num < 0) num = num * -1;
        // Mathf.Log10(0)はNegativeInfinityを返すため、別途処理する。
        return (num == 0) ? 1 : ((int)Mathf.Log10(num) + 1);
    }
}




