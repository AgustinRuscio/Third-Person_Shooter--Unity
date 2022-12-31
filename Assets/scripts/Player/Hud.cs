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
            instance= this;
    }

    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
        _staminaBar.size = stamina / maxStamina;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _healthBar.size = currentHealth/ maxHealth;
    }

    public void UpdateShootBar(float shootTime, float maxShootTime)
    {
        _shootBar.size = shootTime / maxShootTime;
    }
}
