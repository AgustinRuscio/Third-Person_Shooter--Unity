using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject _creditsPanel;

    [SerializeField]
    private GameObject _settingsPanel;

    [SerializeField]
    private SoundData _clickSound;

    public void Play() => LevelLoader.LoadLevel("Lvl1");
    

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

    public void OpenSettingsPanel()
    {
        ClickSound();
        _settingsPanel.SetActive(true);
    }

    public void CloseCSettingsPanel()
    {
        ClickSound();
        _settingsPanel.SetActive(false);
    }

    public void QuitGame() => Application.Quit();
    private void ClickSound() => AudioManager.instance.AudioPlay(_clickSound);
    
}
