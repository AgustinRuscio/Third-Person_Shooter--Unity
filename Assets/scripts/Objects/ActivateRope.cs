//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRope : MonoBehaviour
{
    [SerializeField]
    private ActivateSwingTrap _swingObject;

    [SerializeField]
    private SoundData _trapSound;

    [SerializeField]
    private SoundData _ropeSound;

    private int _use = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(_use == 0)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();

            AudioManager.instance.AudioPlay(_ropeSound, transform.position);

            if (damageable != null)
                _swingObject.Activate(_trapSound);

            _use++;
        }   
    }
}