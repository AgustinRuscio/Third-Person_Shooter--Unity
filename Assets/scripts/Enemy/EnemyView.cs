//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView
{
    private MonoBehaviour _mono;
    private Animator _animator;

    public EnemyView(MonoBehaviour monobehavourParent, Animator animator)
    {
        _mono = monobehavourParent;
        _animator = animator;
    }

    public void AttackAnim()
    {
        string parameter = "Attack";
        _animator.SetBool(parameter, true);

        _mono.StartCoroutine(setFalse(parameter, 1f));
    }

    public void GetHurtkAnim()
    {
        string parameter = "GetHurt";
        _animator.SetBool(parameter, true);

        _mono.StartCoroutine(setFalse(parameter, 1f));
    }

    IEnumerator setFalse(string parameter, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        _animator.SetBool(parameter, false); 
    }

    public void DetectionAnim(bool detectionStatus) => _animator.SetBool("Detection", detectionStatus);

    public void Death() => _animator.SetBool("Death", true);
}