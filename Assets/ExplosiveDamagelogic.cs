using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamagelogic : MonoBehaviour
{
    [SerializeField]
    private float _damage;

    public bool _isExplosive;

    float timesUsed = 0;

    public void Activate() => _isExplosive = true;

    private void OnTriggerStay(Collider other)
    {
        if (_isExplosive && timesUsed == 0)
        {
            timesUsed++;
            Debug.Log("Exploting");
            var damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }

            
        }
    }
}
