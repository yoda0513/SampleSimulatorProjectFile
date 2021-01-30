using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * シーンに関する便利な関数のまとめ
 */

static class SceneChange
{
    public static string mainSceneName = "Common";

    //シーンを起動時の状態戻す
    public static void resetScene()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainSceneName));
        List<Scene> scene = new List<Scene>();
       
        for(int i =0; i< SceneManager.sceneCount; i++)
        {
            Scene x = SceneManager.GetSceneAt(i);
            if (x.name != mainSceneName)
            {
                scene.Add(x);
            }
        }

        foreach(var i in scene)
        {
            Debug.Log(i.name);
            SceneManager.UnloadSceneAsync(i);
        }
    }

    //シーンを読み込む
    public static void AddLoadScene(string sceneName , bool async = false)
    {
        if (!isLoaded(sceneName))
        {
            if (async)
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
    }

    public static void UnloadScene(string sceneName)
    {
        if (isLoaded(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }


    //読み込まれているかどうか
    public static bool isLoaded(string sceneName = "")
    {
        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        if (targetScene.isLoaded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //シーンのルートオブジェクトを参照し、指定のコンポーネントをもつオブジェクトを呼び出す
    public static T getSceneComponent<T>(string sceneName)
    {
        T x = default(T);

        GameObject[] roots = null;
        
        roots = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
        

        for (int i = 0; i < roots.Length; i++)
        {
            x = roots[i].GetComponent<T>();
            if (x != null) break;
        }

        return x;
    }

    //シーンから名前でGameObjectを参照する。
    public static GameObject getSceneObject(string ObjectName, string sceneName)
    {
        GameObject[] roots = null;

        roots = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].name == ObjectName) return roots[i];
        }
        return null;

    }
}
