//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    //This Script is use only for Method that will be call by the anims Event

    [SerializeField]
    private PlayerModel _player;

    [SerializeField]
    private SoundData _landSound;

    [SerializeField]
    private SoundData _stepOne;

    [SerializeField]
    private SoundData _stepTwo;

    [SerializeField]
    private SoundData _breathOne;

    [SerializeField]
    private SoundData _breathTwo;

    private void LauchGranade()
    {
        _player.ThoughGranade();

        StartCoroutine(BackToMovement());
    }

    IEnumerator BackToMovement()
    {
        yield return new WaitForSeconds(0.5f);

        _player.SetLauchGranadeFalse();
    }

    private void LandingSound()
    {
        AudioManager.instance.AudioPlay(_landSound, transform.position);
    }

    private void StepOne()
    {
        AudioManager.instance.AudioPlay(_stepOne, transform.position);
    }

    private void StepTwo()
    {
        AudioManager.instance.AudioPlay(_stepTwo, transform.position);
    }

    private void BreathOne()
    {
        AudioManager.instance.AudioPlay(_breathOne, transform.position);
    }

    private void BreathTwo()
    {
        AudioManager.instance.AudioPlay(_breathTwo, transform.position);
    }
}
