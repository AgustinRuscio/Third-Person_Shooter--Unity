using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tramp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var a = other.gameObject.GetComponent<IDamageable>();

        if (a != null)
        {
            a.TakeDamage(15);
        }
    }
}
