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
        _damage = (float)parameter[1];
        _explotionForce = (float)parameter[2];
        Explotion();
    } 

    public void Explotion()
    {
        Collider[] hitsObject = Physics.OverlapSphere(transform.position, 7f);

        foreach (Collider hitObject in hitsObject)
        {
            var damageable = hitObject.GetComponent<IDamageable>();

            if(damageable != null)
            {
                var obj = hitObject.gameObject.GetComponent<Transform>();
                damageable.TakeDamage(_damage, (obj.forward * -1) * _explotionForce);
            }               
        }
    }
}