using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using System.IO;

public class Sample : MonoBehaviour
{

    public Master master;

    //素材```````````````````````````````````````````````````````````````````
    public Text populationName;
    public Text dataCount;

    public Slider sampleCountBar;
    public Slider sampleSizeBar;

    public InputField sampleCountText;
    public InputField sampleSizeText;

    public GameObject sampleImage;

    public List<List<float>> dataList ; //採取結果をまとめるリスト
    public List<float> averageList = new List<float>();

    SpreadSheet spreadSheet;

    public GameObject LoadPanel;
    public GameObject sampleLoadPanel;

    public Text errorMessage;
    bool nowErrorMessage = false;

    public Text sampleID;
    public Text sampleAverage;

    public Histogram hist;

    public Bubble[] helpBubbleList = new Bubble[] { };

    public bool isRestore = true;

    public Toggle checkRestored;
    public GameObject getSSButton;
    public GameObject setSSButton;

    

    void Start()
    {
        LoadPanel.SetActive(false);
        sampleLoadPanel.SetActive(false);
        master = SceneChange.getSceneComponent<Master>(SceneChange.mainSceneName);
        updateSampleScene(SpreadSheet.sheetName, master.population.Count);
        sampleImage.SetActive(false);

        TableInitialize();

        spreadSheet = SceneChange.getSceneComponent<SpreadSheet>("Common");

        if (SpreadSheet.RestrictedSpreadSheetAccess)
        {
            getSSButton.SetActive(false);
            setSSButton.SetActive(false);
        }

        if (SpreadSheet.RestrictedNoneRestored)
        {
            checkRestored.interactable = false;
        }

    }
   




    void updateSampleScene(string sheetName, int dataCount)
    {
        if (isRestore) //復元　
        {
            sampleSizeBar.maxValue = 1000;

            updateUI();

            populationName.text = sheetName;
            this.dataCount.text = "データ数:" + dataCount.ToString();

        }
        else　　//非復元　
        {
            sampleSizeBar.maxValue = dataCount / 2;

            updateUI();

            populationName.text = sheetName;
            this.dataCount.text = "データ数:" + dataCount.ToString();
        }
        
    }

    public void updateUI()
    {
        sampleCountText.text = sampleCountBar.value.ToString();
        sampleSizeText.text = sampleSizeBar.value.ToString();

        if(master.population.Count != 0)
        {
            float rate = sampleSizeBar.value / master.population.Count;
            sampleImage.GetComponent<RectTransform>().localScale = new Vector3(rate, rate, 1);
        }
        
    }

    public List<float> oneSample(int sampleSize, int j)
    {
        List<float> returnList = new List<float>();
        

        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond * j);
        for (int i=0; i<sampleSize; i++)
        {
            if (isRestore) //復元抽出　気にしなくても良い
            {
                int index = UnityEngine.Random.Range(0, master.population.Count);
                returnList.Add(master.population[index]);
            }
            else  //非復元抽出　処理が必要
            {
                bool trigger = true;
                List<int> indexData = new List<int>();

                while (trigger)  //まだ抽出されたことがないものを取るまで繰り返す
                {
                    int index = UnityEngine.Random.Range(0, master.population.Count);

                    if (!indexData.Contains(index)) //一度抽出されたデータはスルー
                    {
                        indexData.Add(index);
                        returnList.Add(master.population[index]);
                        trigger = false;
                    }

                    
                }

            }
        }

        return returnList;
    }


  

    IEnumerator sampleCoroutin()
    {
        sampleLoadPanel.SetActive(true);

        var dataList = new List<List<float>>();
        var sampleCount = (int)sampleCountBar.value;

        yield return null;

        TableInitialize();

        yield return null;


        //サンプリングを行う
        for (int i = 0; i < sampleCount; i++)
        {
            if (i % 10 == 0) yield return null;
            var subList = oneSample((int)sampleSizeBar.value, i);
            dataList.Add(subList);
        }

        yield return null;
        this.dataList = dataList;
        this.averageList = new List<float>();
        yield return null;


        for (int i = 0; i < dataList.Count; i++)  //表を更新する
        {
            sampleID.text += (i+1).ToString() + "\n";

            var average = dataList[i].Average();
            averageList.Add(average);

            sampleAverage.text += average.ToString() + "\n";
        }

        
        //ヒストグラムの生成
        sampleLoadPanel.SetActive(false);
        Distibution x = new Distibution(averageList);
        hist.resetHist();
        hist.createHistgram(x);
    }

    public void SamplingButton()
    {
        nowErrorMessage = false;
        TableInitialize();
        StartCoroutine(sampleCoroutin());
    }

    


    private void TableInitialize()
    {
        sampleID.text = "";
        sampleAverage.text = "";
    }

    public void goHome()
    {
        dataList = null;
        averageList = null;
        SceneChange.AddLoadScene("HomeScene");
        SceneChange.UnloadScene("SampleScene");
        SceneChange.UnloadScene("Histogram");
    }

    public void writeSS()
    {
        if(dataList!= null)
        {

            LoadPanel.SetActive(true);
            StartCoroutine(spreadSheet.SetGoogleSpreadSheet2(averageList));

        }
        else
        {
            runErrorMessage("標本を抽出してください。");
        }
        
    }

    public void endSS(int x, string y ="") 
    {
        LoadPanel.SetActive(false);

        if (x == 0)
        {
            
        }else 
        {
            runErrorMessage(y);
        }
    }

    public void pressHelpButton()
    {
        if(helpBubbleList[0].isView == true)
        {
            foreach(var i in helpBubbleList)
            {
                i.IsView = false;
            }
        }
        else
        {
            foreach (var i in helpBubbleList)
            {
                i.IsView = true;
            }
        }
    }

    void runErrorMessage(string x)
    {
        StartCoroutine(ErrorCoroutine(x));
    }

    IEnumerator ErrorCoroutine(string x)
    {
        nowErrorMessage = false;
        yield return null;
        nowErrorMessage = true;
        errorMessage.text = x;

        float time = 0;

        while ((time <5) && (nowErrorMessage))
        {
            time += Time.deltaTime;
            yield return null;
        }
        nowErrorMessage = false;
        errorMessage.text = "";
        

    }

    public void onBrowserButton()
    {
        Application.OpenURL(SpreadSheet.sheet_URL);//""の中には開きたいWebページのURLを入力します
    }

    public void switchRestore()
    {
        

        if (isRestore)  //復元抽出　→　非復元
        {
            sampleImage.SetActive(true);
            isRestore = false;
        }
        else　　//非復元抽出　→　復元
        {
            sampleImage.SetActive(false);
            isRestore = true;
        }

        updateSampleScene(SpreadSheet.sheetName, master.population.Count);
    }

    public void endCountInput()
    {
        int count = 0;
        if(int.TryParse(sampleCountText.text, out count))
        {
            if (count < sampleCountBar.minValue) count = (int)sampleCountBar.minValue;
            if (count > sampleCountBar.maxValue) count = (int)sampleCountBar.maxValue;
        }

        sampleCountBar.value = count;
    }

    public void endSizeInput()
    {
        int count = 0;
        if (int.TryParse(sampleSizeText.text, out count))
        {
            if (count < sampleSizeBar.minValue) count = (int)sampleSizeBar.minValue;
            if (count > sampleSizeBar.maxValue) count = (int)sampleSizeBar.maxValue;
        }

        sampleSizeBar.value = count;
    }
}
