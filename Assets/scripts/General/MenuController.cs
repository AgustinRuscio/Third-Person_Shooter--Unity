using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _creditsPanel;

    [SerializeField]
    private SoundData _clickSound;

    public void Play()
    {
        ClickSound();
        SceneManager.LoadScene("Lvl1" ,LoadSceneMode.Single);
    }

    public void OpenCreditsPanel()
    {
        ClickSound();
        _creditsPanel.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        ClickSound();
        _creditsPanel.SetActive(false);
    }

    private void ClickSound() => AudioManager.instance.AudioPlay(_clickSound);
    
}
