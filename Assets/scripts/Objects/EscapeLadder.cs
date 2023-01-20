using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeLadder : MonoBehaviour, IInteractuable
{
    [SerializeField]
    private GameObject _text;

    [SerializeField]
    private AudioSource _sound;

    private void Awake() 
    {
        EventManager.Suscribe(ManagerKeys.PauseGame, OnPause); 
        EventManager.Suscribe(ManagerKeys.ResumeGame, OnResume); 
    }
    
    private void OnPause(params object[] parameters) => _sound.volume = 0;

    private void OnResume(params object[] parameters) => _sound.volume = 0.7f;

    public void OnInteractaction() => SceneManager.LoadScene("WinScene");

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerModel>();

        if (player != null)
            _text.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerModel>();

        if (player != null)
            _text.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.Suscribe(ManagerKeys.PauseGame, OnPause);
        EventManager.Suscribe(ManagerKeys.ResumeGame, OnResume);
    }
}