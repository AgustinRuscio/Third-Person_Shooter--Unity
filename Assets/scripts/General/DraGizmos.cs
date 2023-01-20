//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraGizmos : MonoBehaviour
{
    [SerializeField]
    private float _distance;

    [SerializeField]
    private Transform _startPoint;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 dir = _startPoint.forward * _distance;

        Gizmos.DrawRay(_startPoint.position, dir);
    }
}