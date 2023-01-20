//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Default
}

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    [SerializeField]
    private Image _deathPanel;

    [SerializeField]
    private Text _deathTxt;

    [SerializeField]
    private Text _resetTxt;

    [SerializeField]
    private SoundData _deathSoundFx;

    public Difficulty _currentDifficulty;

    [SerializeField]
    private DifficultySetter _spawner;

    private bool _death;

    private void Awake()
    {
        //SetDifficulty

        _currentDifficulty = GameData.GetDificultyEnum(PlayerPrefs.GetString("Dif"));

        _spawner._difficulty = _currentDifficulty;


        Time.timeScale = 1f;

        if (instance == null)
            instance = this;

        EventManager.Suscribe(ManagerKeys.Death, Death);
        EventManager.Suscribe(ManagerKeys.ResetScene, ResetScene);


        Color c = _deathPanel.color;
        c.a = 0f;
        _deathPanel.color = c;

        Color c2 = _deathTxt.color;
        c2.a = 0f;
        _deathTxt.color = c2;
    }


    private void ResetScene(params object[] parameters)
    {
        if(_death)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
    }

    private void Death(params object[] parameters) => StartCoroutine(StartFadeIn());

    IEnumerator StartFadeIn()
    {
        yield return new WaitForSeconds(3f);

        AudioManager.instance.AudioPlay(_deathSoundFx);

        FadeInDeathPanel();
    }

    private void FadeInDeathPanel() => StartCoroutine(FadeIn());
    

    IEnumerator FadeIn()
    {
        for(float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = _deathPanel.color;
            c.a = f;
            _deathPanel.color = c;

            Color c2 = _deathTxt.color;
            c2.a = f;
            _deathTxt.color = c2;

            yield return new WaitForSeconds(0.05f);
        }

        _resetTxt.gameObject.SetActive(true);
        _death = true;
        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        EventManager.UnSuscribe(ManagerKeys.Death, Death);
        EventManager.UnSuscribe(ManagerKeys.ResetScene, ResetScene);
    }
    
}
