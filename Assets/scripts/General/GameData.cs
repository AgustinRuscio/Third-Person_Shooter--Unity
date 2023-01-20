//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static Difficulty GetDificultyEnum(string difficultySelected)
    {
        switch (difficultySelected)
        {
            case "Esay":
                return Difficulty.Easy;

            case "Normal":
                return Difficulty.Normal;

            case "Hard":
                return Difficulty.Hard;

            default:
                return Difficulty.Normal;
        }
    }
}