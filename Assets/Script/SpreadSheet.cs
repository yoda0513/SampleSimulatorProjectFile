using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

//デリゲート``````````````````````````````````````````````````````````````````````````````````````````````````````````````

public class SpreadSheet:MonoBehaviour
{ 

    public static string sheetName = "";
    public static string sheet_URL = "";

    //GASのウェブアプリのURL。
    const string getApplicationURI  = "https://script.google.com/macros/s/AKfycbxpuPSFeWDbQv8T2B58GDv1URXDFA_AQmTPWAoNyrMByF-ytbYVpKx9/exec";
    const string setApplicationURI = "https://script.google.com/macros/s/AKfycbxYZxQPlgBhCJueErbO3qowF3AcucssogXl6LM3byPTiSPWfqZZDgeTTw/exec";

    public static bool RestrictedNoneRestored = false;  //復元抽出だけに限定したい場合(サイコロやコインのシミュレーション)はtrueにしておく
    public static bool RestrictedSpreadSheetAccess = false;  //スプレッドシートの書き込みや読み込みを限定する


   

    //メイン~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public static IEnumerator LoadGoogleSpreadSheet()
    {
        string URL = $"{getApplicationURI}?url={sheet_URL}";
        var request = UnityEngine.Networking.UnityWebRequest.Get(URL);

        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
        {
            SceneChange.getSceneComponent<Home>("HomeScene").Error(1);
        }
        else
        {
            var result = request.downloadHandler.text; //csv形式のテキスト。
            List<float> data = getCSV(result, out sheetName);

            if(data.Count == 0)
            {
                SceneChange.getSceneComponent<Home>("HomeScene").Error(1);
            }else if(data.Count < 2)
            {
                SceneChange.getSceneComponent<Home>("HomeScene").Error(2);
            }
            else
            {
                Debug.Log($"シート名:::{sheetName}");
                SceneChange.getSceneComponent<Master>("Common").population = data;
                SceneChange.UnloadScene("HomeScene");
                SceneChange.AddLoadScene("SampleScene");

                
            }
            
        }

    }

    
    public IEnumerator SetGoogleSpreadSheet( List<List<float>> data)
    {
        string sample = toCSV(data);
        yield return null;

        string URL = $"{setApplicationURI}?sample={sample}&url={sheet_URL}";
        var request = UnityEngine.Networking.UnityWebRequest.Get(URL);

        yield return request.SendWebRequest();

        int result = 0;

        if (request.isHttpError || request.isNetworkError)
        {
            result = 1;
            Debug.Log(request.error);
            SceneChange.getSceneComponent<Sample>("SampleScene").endSS(result, request.error);
        }
        else
        {
            if(int.Parse(request.downloadHandler.text) == 0)
            {
                SceneChange.getSceneComponent<Sample>("SampleScene").endSS(0);
            }
            else
            {
                SceneChange.getSceneComponent<Sample>("SampleScene").endSS(2, "エラー。");
            }

        }
    }

    public IEnumerator SetGoogleSpreadSheet2(List<float> data)
    {
        //テスト------------------------------------------------------------------
        string sample = "";
        sample += "標本ID,標本平均♂";

        for (int i = 0; i < data.Count; i++)
        {
            sample += $"{i + 1},";

            
            sample += data[i];

            sample += "♂";

            //途中で休憩する。
            if ((i % 200) == 0)
            {
                Debug.Log("サンプル休憩");
                yield return null;
            }
        }

        sample.Substring(0, sample.Length - 1);


        Debug.Log("csv変換完了");
        //--------------------------------------------------------------------

        //string sample = toCSV(data);
        yield return null;
        
        byte[] sampleDataByte = System.Text.Encoding.UTF8.GetBytes(sample);
        byte[] sheetURLByte = System.Text.Encoding.UTF8.GetBytes(sheet_URL);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("sampleData", sampleDataByte));
        formData.Add(new MultipartFormDataSection("sheetUrl", sheetURLByte));
        


        var request = UnityEngine.Networking.UnityWebRequest.Post(setApplicationURI, formData);

        yield return request.SendWebRequest();

        int result = 0;

        if (request.isHttpError || request.isNetworkError)
        {
            result = 1;
            Debug.Log(request.error);
            SceneChange.getSceneComponent<Sample>("SampleScene").endSS(result, request.error);
        }
        else
        {
            int x = 100;
            try
            {
                x = int.Parse(request.downloadHandler.text);
            }
            finally
            {
                
            }

            Debug.Log(request.downloadHandler.text);

            if ( x== 0)
            {
                SceneChange.getSceneComponent<Sample>("SampleScene").endSS(0);
            }
            else
            {
                SceneChange.getSceneComponent<Sample>("SampleScene").endSS(2, "エラー。");
            }
            

        }
        
    }




    //関数~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    //csvテキストをList型に変換する。
    //空白と文字列は除外する。
    static List<float> getCSV(string text, out string sheetName)
    {
        string[] data = text.Split(',');
        List<float> returnList = new List<float>();
        sheetName = data[0];

        for (int i = 0; i < data.Length; i++)
        {
            float element = 0;
            bool success = float.TryParse(data[i], out element);

            if (success)
            {
                returnList.Add(element);
            }
        }

        return returnList;
    }

    public string toCSV(List<List<float>> x)
    {
        string csv = "";

        for (int i = 0; i < x.Count; i++)
        {
            csv += $"標本{i + 1},";


            foreach (var z in x[i])
            {
                csv += z;
                csv += ",";
            }

            csv += "♂";
        }

        csv.Substring(0, csv.Length - 1);

        return csv;
    }



}

[System.Serializable]
public class SSclass  //POSTで送るオブジェクト
{
    public string sheetURL;
    public float[,] data;
    public float floatData = 2;
    public SSclass(string sheetURL, float[,] data)
    {
        this.sheetURL = sheetURL;
        this.data = data;
    }
}
