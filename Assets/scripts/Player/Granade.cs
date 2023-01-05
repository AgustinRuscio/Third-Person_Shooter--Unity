//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionParticles;

    [SerializeField]
    private float delay = 3f;

    [SerializeField]
    private float _explosionForce = 10f;

    [SerializeField]
    private float _radius = 20f;

    [SerializeField]
    private SoundData _sound;

    void Start() => Invoke("Explode", delay);
    

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        
        foreach(Collider near in colliders)
        {
            Rigidbody rig = near.GetComponent<Rigidbody>();

            if(rig != null)
                rig.AddExplosionForce(_explosionForce, transform.position, _radius, 1f, ForceMode.Impulse);
        }

        Instantiate(_explosionParticles, transform.position, transform.rotation);
        AudioManager.instance.AudioPlay(_sound, transform.position);

        Destroy(gameObject);
    }
}
