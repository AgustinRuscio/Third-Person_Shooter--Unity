//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string nextLevel;

    public static void LoadLevel(string lvlName)
    {
        nextLevel = lvlName;

        SceneManager.LoadScene("Loading");
    }
}
