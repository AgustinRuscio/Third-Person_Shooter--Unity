//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


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
    private Scrollbar _shootBar;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        EventManager.Suscribe(ManagerKeys.LifeEvent, UpdateHealthBar);
    }

    public void UpdateStaminaBar(float stamina, float maxStamina) => _staminaBar.size = stamina / maxStamina;

    public void UpdateHealthBar(params object[] parameter) => _healthBar.size = (float)parameter[0] / (float)parameter[1];

    public void UpdateShootBar(float shootTime, float maxShootTime) => _shootBar.size = shootTime / maxShootTime;

    private void OnDestroy() => EventManager.UnSuscribe(ManagerKeys.LifeEvent, UpdateHealthBar);
}