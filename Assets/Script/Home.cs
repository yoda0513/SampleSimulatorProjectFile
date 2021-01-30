using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Home : MonoBehaviour
{
    public WebGLNativeInputField inputField;
    SpreadSheet spreadSheet;
    public GameObject LoadPanel;
    public Text ErrorMEssage;

    string DiceURL = "https://docs.google.com/spreadsheets/d/1FnfC3eNBtoXfg0OjwxpNRif4WVtb3e1onbj8p6J14wQ/edit#gid=1214137560";
    string CoinURL = "https://docs.google.com/spreadsheets/d/1_8HduTiTRY0SJgAweOZ35EBd9bonXBXK-NEnQBCt214/edit#gid=0";
    string PopulationURL = "https://docs.google.com/spreadsheets/d/1JWuteEHxM3Es-pAzQpivUMqUVI1pSAhsMDLiwAR8uZQ/edit#gid=1311378734";



    void Start()
    {
        Debug.Log(Math.Ceiling(2.3f));
        LoadPanel.SetActive(false);
        spreadSheet = SceneChange.getSceneComponent<SpreadSheet>(SceneChange.mainSceneName);
    }

    void Update()
    {
        
    }

    public void pressEnter()
    {
        if (inputField.text == "")
        {
            Error(0);
        }
        else
        {
            LoadPanel.SetActive(true);
            SpreadSheet.sheet_URL = inputField.text;
            SpreadSheet.RestrictedNoneRestored = false; 
            SpreadSheet.RestrictedSpreadSheetAccess = false;
            StartCoroutine(SpreadSheet.LoadGoogleSpreadSheet());
            
        }
    }

    public void pressDice()
    {
        LoadPanel.SetActive(true);
        SpreadSheet.sheet_URL = DiceURL;
        SpreadSheet.RestrictedNoneRestored = true; //復元ちゅうしゅつを制限する
        SpreadSheet.RestrictedSpreadSheetAccess = true; //アクセスできないようにする
        StartCoroutine(SpreadSheet.LoadGoogleSpreadSheet());

    }

    public void pressCoin()
    {
        LoadPanel.SetActive(true);
        SpreadSheet.sheet_URL = CoinURL;
        SpreadSheet.RestrictedNoneRestored = true; //復元ちゅうしゅつを制限する
        SpreadSheet.RestrictedSpreadSheetAccess = true; //アクセスできないようにする
        StartCoroutine(SpreadSheet.LoadGoogleSpreadSheet());
    }

    public void pressPopulation()
    {
        LoadPanel.SetActive(true);
        SpreadSheet.sheet_URL = PopulationURL;
        SpreadSheet.RestrictedNoneRestored = false; //復元ちゅうしゅつを制限する
        SpreadSheet.RestrictedSpreadSheetAccess = true; //アクセスできないようにする
        StartCoroutine(SpreadSheet.LoadGoogleSpreadSheet());
    }

    public void pressDiceData()
    {
        Application.OpenURL(DiceURL);
    }

    public void pressCoinData()
    {
        Application.OpenURL(CoinURL);
    }

    public void pressPopulationData()
    {
        Application.OpenURL(PopulationURL);
    }

    public void Error(int x)
    {
        LoadPanel.SetActive(false);

        if(x == 0)
        {
            ErrorMEssage.text = "URLを入力してください。";
        }else if(x == 1)
        {
            ErrorMEssage.text = "データが取得できませんでした。";
        }else if(x == 2)
        {
            ErrorMEssage.text = "データのサイズが小さすぎます。";
        }
    }
    

    
}
