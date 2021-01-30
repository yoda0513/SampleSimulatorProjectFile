using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Sample Simulationの基本。常にこいつから始まり、常にこいつがい続ける。


public class Master : MonoBehaviour
{
    public SpreadSheet spreadSheet;
    public List<float> population = new List<float>();

    public bool Debug = false;  //trueだったら自動でホームを超える。

    private void Awake()
    {
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += SceneLoaded;
    }
    //起動始め
    void Start()
    {
        SceneChange.resetScene();

        if (!Debug)
        {
            SceneChange.AddLoadScene("HomeScene");
        }
        else
        {
            SpreadSheet.sheet_URL = "https://docs.google.com/spreadsheets/d/1pTlx_zjj_FwGH9k8OhZsYs0ziig3xxu-FIWGskARXmI/edit#gid=2067789342";

            StartCoroutine(SpreadSheet.LoadGoogleSpreadSheet());
        }
    }


    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if(nextScene.name == "SampleScene")
        {
            SceneChange.AddLoadScene("Histogram");
        }else if(nextScene.name == "Histogram")
        {
            
            SceneChange.getSceneComponent<Sample>("SampleScene").hist = SceneChange.getSceneComponent<Histogram>("Histogram");
            SceneChange.getSceneComponent<Sample>("SampleScene").hist.resetHist();
        }
    }

}
