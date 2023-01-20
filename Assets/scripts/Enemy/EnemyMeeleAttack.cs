//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleAttack : MonoBehaviour
{
    [SerializeField]
    private float _attackCoolDowm;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private EnemyModel _enemyModel;

    [SerializeField]
    private EnemyAnimsEvent events;

    private GenericTimer _attackTimer;

    private void Awake() => _attackTimer = new GenericTimer(_attackCoolDowm);

    private void Start() => _damage *= PlayerPrefs.GetFloat("EnemyDamageMultiplier");

    private void Update() => _attackTimer.RunTimer();
    
    private void OnTriggerStay(Collider other)
    {
        var damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null  && _attackTimer.CheckCoolDown())
        {
            _enemyModel.RunAttackAnim();
            events.FillMeleeVariants(damageable, _damage, _attackTimer);
        }
    }
}
