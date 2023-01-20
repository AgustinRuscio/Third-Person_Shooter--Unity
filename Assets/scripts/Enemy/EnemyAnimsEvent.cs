//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimsEvent : MonoBehaviour
{
    private int _use = 0;

    [SerializeField]
    private EnemyModel _enemy;

    [SerializeField]
    private SoundData _roarSound;

    [SerializeField]
    private SoundData _screamSound;

    [SerializeField]
    private SoundData _stepOne;

    [SerializeField]
    private SoundData _stepTwo;

    [SerializeField]
    private SoundData _getHitSound;

    [SerializeField]
    private SoundData _deathSound;

    private IDamageable _damageable;

    private float _damage;

    private GenericTimer _timer;

    public void FillMeleeVariants(IDamageable damageable, float damage, GenericTimer timer)
    {
        _damageable = damageable;
        _damage = damage;
        _timer = timer;
    }
    
    public void MeleeAttack()
    {
        _damageable.TakeDamage(_damage);
        _timer.ResetTimer();
    }

    public void ActivaeMovement() => _enemy.ActivateMovement();

    public void RoarSound() => AudioManager.instance.AudioPlay(_roarSound, transform);

     public void ScreamSound() 
     { 
         if(_use == 0)
         {
             AudioManager.instance.AudioPlay(_screamSound, transform);
             _use++;
         }
     }

    private void StepOne() => AudioManager.instance.AudioPlay(_stepOne, transform.position);
    
    private void StepTwo() => AudioManager.instance.AudioPlay(_stepTwo, transform.position);
    
    public void GetHitSound() => AudioManager.instance.AudioPlay(_getHitSound, transform);

    public void DeathSound() => AudioManager.instance.AudioPlay(_deathSound, transform);
}