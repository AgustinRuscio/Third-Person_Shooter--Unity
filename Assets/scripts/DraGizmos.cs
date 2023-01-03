using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraGizmos : MonoBehaviour
{
    [SerializeField]
    private float _shootDistance;

    [SerializeField]
    private Transform _shootPoint;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 dir = _shootPoint.forward * _shootDistance;

        Gizmos.DrawRay(_shootPoint.position, dir);
    }
}
