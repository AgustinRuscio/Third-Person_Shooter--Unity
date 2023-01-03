//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamagelogic : MonoBehaviour
{
    private float _damage;

    private float _explotionForce;

    private bool _isExplosive;

    private float timesUsed = 0;


    private void Awake() => EventManager.Suscribe(ManagerKeys.ExplosionEvent, Activate);

    public void Activate(params object[] parameter)
    {
        _isExplosive = (bool)parameter[0];
        _damage = (float)parameter[1];
        _explotionForce = (float)parameter[2];
    } 

    private void OnTriggerStay(Collider other)
    {
        if (_isExplosive && timesUsed == 0)
        {
            timesUsed++;

            var explosive = other.gameObject.GetComponent<IExplosive>();

            if (explosive != null)
                explosive.OnExplosion();

            var damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                var obj = other.gameObject.GetComponent<Transform>();
                damageable.TakeDamage(_damage, (obj.forward * -1) * _explotionForce);
            }
        }
    }
}
