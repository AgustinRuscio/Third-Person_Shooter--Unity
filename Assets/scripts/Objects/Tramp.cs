//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tramp : MonoBehaviour
{
    [SerializeField]
    private float _damage;

    private void OnTriggerEnter(Collider other)
    {
        var otherCollider = other.gameObject.GetComponent<IDamageable>();

        if (otherCollider != null)
            otherCollider.TakeDamage(_damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherCollider = collision.gameObject.GetComponent<IDamageable>();

        if (otherCollider != null)
            otherCollider.TakeDamage(_damage);
    }

    public void ChangeDamage(float newDamage) => _damage = newDamage;
}
