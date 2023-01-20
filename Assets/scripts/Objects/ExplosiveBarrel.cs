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

    [SerializeField]
    private SoundData _soundFx;

    public void OnExplosion()
    {
        _myCollider.enabled = false;
        _barrelModel.SetActive(false);

        _explosiveParticles.Play();
        AudioManager.instance.AudioPlay(_soundFx, transform.position);

        _isExploting = true;

        EventManager.Trigger(ManagerKeys.ExplosionEvent, _isExploting, _damage, _explosionForce);
        StartCoroutine(EndExplosion());
    }

    IEnumerator EndExplosion()
    {
        yield return new WaitForSeconds(_explosiveParticles.main.duration);

        _isExploting = false;

        Destroy(gameObject);
    }
}