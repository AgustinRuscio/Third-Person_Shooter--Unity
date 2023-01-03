//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IExplosive
{
    [SerializeField]
    private float _explosionForce;

    [SerializeField]
    private float _damage;

    private bool _isExploting;

    [SerializeField]
    private ParticleSystem _explosiveParticles;

    [SerializeField]
    private GameObject _barrelModel;

    [SerializeField]
    private Collider _myCollider;

    public void OnExplosion()
    {
        _myCollider.enabled = false;
        _barrelModel.SetActive(false);

        _explosiveParticles.Play();

        _isExploting = true;

        EventManager.Trigger(ManagerKeys.ExplosionEvent, _isExploting, _damage, _explosionForce);
        StartCoroutine(EndExplosion());
    }

    IEnumerator EndExplosion()
    {
        yield return new WaitForSeconds(3.5f);

        _isExploting = false;

        Destroy(gameObject);
    }
}
