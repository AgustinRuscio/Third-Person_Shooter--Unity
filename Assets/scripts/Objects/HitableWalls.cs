//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableWalls : MonoBehaviour, IHitable
{
    [SerializeField]
    private ParticleSystem _particles;

    public void OnHit(Vector3 hit)
    {
        Instantiate(_particles.gameObject, hit, Quaternion.Euler(hit));
        _particles.Play();
    }
}
