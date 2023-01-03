using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IExplosive
{
    [SerializeField]
    private ParticleSystem _explosiveParticles;

    [SerializeField]
    private GameObject _barrelModel;

    [SerializeField]
    private Collider _myCollider;

    [SerializeField]
    private ExplosiveDamagelogic _damageLogic;

    public void OnExplosion()
    {
        _myCollider.enabled = false;
        _barrelModel.SetActive(false);

        _explosiveParticles.Play();
        

        _damageLogic.Activate();
        StartCoroutine(EndExplosion());
    }

    IEnumerator EndExplosion()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }
}
