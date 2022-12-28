//--------------------------------------//
//          Agustin Ruscio             //
//------------------------------------//


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{
    private Animator _animator;

    private MonoBehaviour _mono;

    private bool _sprint;

    public PlayerView(Animator playerAnimator, MonoBehaviour playerMonoBehaviour)
    {
        _animator = playerAnimator;
    
        _mono = playerMonoBehaviour;
    }

    public void Run(float x, float z, bool onSprint, bool aim)
    {
        _sprint = onSprint;

        if(aim) 
        {
            _animator.SetBool("Sprint", false);
        }
        else if(_sprint && z !> 0)
        {
            _animator.SetBool("Sprint", true);
        }
        else
        {
            _animator.SetFloat("Horizontal", x);
            _animator.SetFloat("Vertical", z);

            _animator.SetBool("Sprint", false);
        }
    }

    public void SetAimAnim(bool _isAiming)
    {
        _animator.SetBool("Aim", _isAiming);
    }

    public void SetShootAnim(bool shooting)
    {
        string parameter = "Shoot";
        _animator.SetBool(parameter, shooting);

        //_mono.StartCoroutine(ChangeBoolParameter(parameter));
    }

    IEnumerator ChangeBoolParameter(string parameterModificated)
    {
        yield return new WaitForSeconds(.2f);
        _animator.SetBool(parameterModificated, false);
    }
}
