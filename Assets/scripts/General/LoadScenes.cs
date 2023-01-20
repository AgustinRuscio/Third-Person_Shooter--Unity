//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    void Start()
    {
        string levelToLoad = LevelLoader.nextLevel;

        StartCoroutine(MaketheLoad(levelToLoad));
    }

    IEnumerator MaketheLoad(string lvl)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(lvl);

        while (operation.isDone == false)
        {
            yield return null;
        }
    }
}
