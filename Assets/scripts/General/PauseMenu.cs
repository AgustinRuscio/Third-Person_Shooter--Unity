//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;

    private void Awake()
    {
        EventManager.Suscribe(ManagerKeys.PauseGame, PauseGame);
        EventManager.Suscribe(ManagerKeys.ResumeGame, ResumeGame);
    }

    private void PauseGame(params object[] parameters)
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
    }

    private void ResumeGame(params object[] parameters)
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.UnSuscribe(ManagerKeys.PauseGame, PauseGame);
        EventManager.UnSuscribe(ManagerKeys.ResumeGame, ResumeGame);
    }
}
