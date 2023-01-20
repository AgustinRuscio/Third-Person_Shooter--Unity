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

    private void Awake() => PlayerPrefs.SetString("Dif", "Normal");
    

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
    public void BackToMainMenu()
    {
        ClickSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() => Application.Quit();
    private void ClickSound() => AudioManager.instance.AudioPlay(_clickSound);
    
    public void EsaySelector()
    {
        ClickSound();
        PlayerPrefs.SetString("Dif", "Easy");
    }

    public void NormalSelector()
    {
        ClickSound();
        PlayerPrefs.SetString("Dif", "Normal");
    }

    public void HardSelector()
    {
        ClickSound();
        PlayerPrefs.SetString("Dif", "Hard");
    }
}
