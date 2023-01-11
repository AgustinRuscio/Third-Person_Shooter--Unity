//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwingTrap : MonoBehaviour
{
    private bool _isActive;

    [SerializeField]
    private Vector3 a;

    public void Activate(SoundData audio) => StartCoroutine(Move(audio));
    
    private void Update()
    {
        if (_isActive)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(a), Time.deltaTime * 7);
    }

    IEnumerator Move(SoundData audio)
    {
        yield return new WaitForSeconds(1);
        _isActive = true;

        AudioManager.instance.AudioPlay(audio, transform.position);
    }
}