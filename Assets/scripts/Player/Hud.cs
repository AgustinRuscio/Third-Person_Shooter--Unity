//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public static Hud instance;

    [SerializeField]
    private Scrollbar _staminaBar;

    [SerializeField]
    private Scrollbar _healthBar;

    [SerializeField]
    private Image _deadlyPanel;

    [SerializeField]
    private Scrollbar _shootBar;

    [SerializeField]
    private GameObject[] _granadeImages = new GameObject[3];

    private void Awake()
    {
        if(instance == null)
            instance = this;

         _healthBar.enabled = true;
         _staminaBar.enabled = true;
         _staminaBar.enabled = true;

        EventManager.Suscribe(ManagerKeys.LifeEvent, UpdateHealthBar);
        EventManager.Suscribe(ManagerKeys.GranadeNumber, UpdateGranadeImages);
        EventManager.Suscribe(ManagerKeys.Death, Death);
    }

    public void UpdateStaminaBar(float stamina, float maxStamina) => _staminaBar.size = stamina / maxStamina;

    public void UpdateHealthBar(params object[] parameter)
    {
        _healthBar.size = (float)parameter[0] / (float)parameter[1];

        if ((bool)parameter[2])
        {
            _deadlyPanel.gameObject.SetActive(true);
        }
        else
        {
            _deadlyPanel.gameObject.SetActive(false);
        }

    }

    public void UpdateShootBar(float shootTime, float maxShootTime) => _shootBar.size = shootTime / maxShootTime;

    public void UpdateGranadeImages(params object[] granadeAmount)
    {
        int amountCasted = (int)granadeAmount[0];

        if(amountCasted == 3)
        {
            for (int i = 0; i < _granadeImages.Length; i++)
            {
                _granadeImages[i].SetActive(true);
            }
        }
        else if(amountCasted == 2)
        {
            _granadeImages[0].SetActive(true);
            _granadeImages[1].SetActive(true);

            _granadeImages[2].SetActive(false);
        }
        else if(amountCasted == 1)
        {
            _granadeImages[0].SetActive(true);

            _granadeImages[1].SetActive(false);
            _granadeImages[2].SetActive(false);
        }
        else
        {
            for (int i = 0; i < _granadeImages.Length; i++)
            {
                _granadeImages[i].SetActive(false);
            }
        }
    }

     private void Death(params object[] parameters)
     {
        _healthBar.gameObject.SetActive(false);
        _staminaBar.gameObject.SetActive(false);
        _shootBar.gameObject.SetActive(false);


        for (int i = 0; i < _granadeImages.Length; i++)
        {
            _granadeImages[i].SetActive(false);
        }
    }

    private void OnDestroy()
    {
        EventManager.UnSuscribe(ManagerKeys.LifeEvent, UpdateHealthBar);
        EventManager.UnSuscribe(ManagerKeys.GranadeNumber, UpdateGranadeImages);
        EventManager.UnSuscribe(ManagerKeys.Death, Death);
    }
}