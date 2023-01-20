//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    public Difficulty _difficulty;

    void Awake()
    {
        switch (_difficulty)
        {
            case Difficulty.Easy:
                SetEasySettings(); break;
            case Difficulty.Normal:
                SetNormalSettings(); break;
            case Difficulty.Hard:
                SetHardSettings(); break;
            case Difficulty.Default:
                SetNormalSettings(); break;
        }
    }

    private void SetEasySettings()
    {
        PlayerPrefs.SetFloat("EnemyLifeMultiplier", 0.75f);
        PlayerPrefs.SetFloat("EnemySpeedMultiplier", 0.75f);
        PlayerPrefs.SetFloat("EnemyDamageMultiplier", 0.75f);
    }

    private void SetNormalSettings()
    {
        PlayerPrefs.SetFloat("EnemyLifeMultiplier", 1f);
        PlayerPrefs.SetFloat("EnemySpeedMultiplier", 1f);
        PlayerPrefs.SetFloat("EnemyDamageMultiplier", 1f);
    }

    private void SetHardSettings()
    {
        PlayerPrefs.SetFloat("EnemyLifeMultiplier", 1.5f);
        PlayerPrefs.SetFloat("EnemySpeedMultiplier", 1.5f);
        PlayerPrefs.SetFloat("EnemyDamageMultiplier", 1.5f);
    }
}