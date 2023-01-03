//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioInstance : MonoBehaviour
{
     private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void IntancePlay(SoundData audio)
    {
        _audioSource.loop = audio.loop;
        _audioSource.spatialBlend = audio.sound3D ? 1 : 0;

        _audioSource.maxDistance = audio.maxDistance;

        _audioSource.clip = audio.clip;
        _audioSource.outputAudioMixerGroup = audio.miixerGroup;

        _audioSource.Play();

        if (!audio.loop)
            Destroy(gameObject, audio.clip.length);
    }

    private void OnDestroy()
    {
        AudioManager.instance.RemoveFromList(_audioSource.clip);
    }
}
